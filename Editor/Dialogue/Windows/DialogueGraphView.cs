using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Dialogue.Elements;
using Dialogue.Data;
using System.Reflection;
using Dialogue.Utilities;

namespace Dialogue.Windows
{
    public class DialogueGraphView : GraphView
    {
        public Action<IDialogueView> OnNodeSelected;

        private readonly DialogueEditor editorWindow;
        private DialogueSearchWindow searchWindow;

        public DialogueGraph graph;

        private static Dictionary<string, IDialogueView> nodeViews;

        public DialogueGraphView(DialogueEditor editorWindow)
        {
            this.editorWindow = editorWindow;

            AddManipulators();
            AddSearchWindow();
            AddGridBackground();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupedElementsRemoved();
            OnGroupRenamed();

            AddStyles();
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort =>
                endPort != startPort && endPort.direction != startPort.direction && endPort.node != startPort.node
            ).ToList();
        }

        private void AddManipulators()
        {
            SetupZoom(0.01f, 5f);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            var nodes = TypeCache.GetTypesDerivedFrom<DialogueBase>();
            foreach (var type in nodes)
            {
                if (type.GetTypeInfo().IsAbstract)
                {
                    continue;
                }

                evt.menu.AppendAction(type.Name, actionEvent => CreateNode(type, GetLocalMousePosition((actionEvent.eventInfo.localMousePosition))));
            }
        }

        public void SetGraph(DialogueGraph graph)
        {
            this.graph = graph;
            nodeViews = new();

            graphViewChanged -= OnGraphVewChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphVewChanged;

            graph.Nodes.Values.ToList().ForEach(node => AddNodeView(node));
            nodeViews.Values.ToList().ForEach(nodeView => PlaceNodeView(nodeView));
            nodeViews.Values.ToList().ForEach(nodeView => nodeView.LoadConnections(this, nodeViews));
        }

        public void CreateNode(Type type, Vector2 position)
        {
            DialogueBase node = graph.CreateNode(type, position);
            CreateNodeView(node);
        }

        public void CreateNodeView(DialogueBase node)
        {
            IDialogueView view = AddNodeView(node);
            PlaceNodeView(view);
        }

        public void DeleteNodeView(IDialogueView view)
        {
            if (view is DialogueGroupView groupView)
            {
                List<DialogueNodeView> groupNodes = groupView.containedElements.OfType<DialogueNodeView>().ToList();

                groupView.RemoveElements(groupNodes);

                RemoveElement(groupView);

                graph.DeleteNode(groupView.Data);
            }
            else if (view is DialogueNodeView nodeView)
            {
                nodeView.Group?.RemoveElement(nodeView);

                nodeView.DisconnectAllPorts();

                RemoveElement(nodeView);

                graph.DeleteNode(nodeView.Node);
            }
        }

        private IDialogueView AddNodeView(DialogueBase node)
        {
            Type nodeType = Type.GetType($"Dialogue.Elements.{node.GetType().Name}View");

            IDialogueView view = (IDialogueView)Activator.CreateInstance(nodeType);
            view.SetOnNodeSelected(OnNodeSelected);
            view.Init(this, node);

            nodeViews.Add(node.ID, view);

            return view;
        }

        private void PlaceNodeView(IDialogueView view)
        {
            if (view is DialogueGroupView groupView)
            {
                AddElement(groupView);

                foreach (DialogueNodeView nodeView in selection.OfType<DialogueNodeView>())
                {
                    groupView.AddElement(nodeView);
                }
            }
            else if (view is DialogueNodeView nodeView)
            {
                AddElement(nodeView);

                if (!string.IsNullOrEmpty(nodeView.Node.GroupID) && nodeViews[nodeView.Node.GroupID] is DialogueGroupView nodeGroupView)
                {
                    nodeGroupView.AddElement(nodeView);
                }
            }
        }

        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                DeleteElements(selection.OfType<Edge>().ToList());

                foreach (IDialogueView view in selection.OfType<IDialogueView>().ToList())
                {
                    DeleteNodeView(view);
                }
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                DialogueGroupView nodeGroup = (DialogueGroupView)group;

                foreach (DialogueNodeView node in elements.OfType<DialogueNodeView>())
                {
                    node.Group = nodeGroup;
                    node.Node.GroupID = nodeGroup.GetID();
                }
            };
        }

        private void OnGroupedElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                DialogueGroupView nodeGroup = (DialogueGroupView)group;

                foreach (DialogueNodeView node in elements.OfType<DialogueNodeView>())
                {
                    node.Group = null;
                    node.Node.GroupID = "";
                }
            };
        }

        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                DialogueGroupView nodeGroup = (DialogueGroupView)group;

                nodeGroup.Data.Name = newTitle;
                nodeGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

                nodeGroup.OldTitle = nodeGroup.title;
            };
        }

        private GraphViewChange OnGraphVewChanged(GraphViewChange changes)
        {
            if (changes.edgesToCreate != null)
            {
                foreach (Edge edge in changes.edgesToCreate)
                {
                    DialogueNodeView inNode = (DialogueNodeView)edge.input.node;
                    DialogueNodeView outNode = (DialogueNodeView)edge.output.node;

                    inNode.ConnectIn(edge.input, outNode);
                    outNode.ConnectOut(edge.output, inNode);
                }
            }

            if (changes.elementsToRemove != null)
            {
                foreach (Edge edge in changes.elementsToRemove.OfType<Edge>())
                {
                    DialogueNodeView inNode = (DialogueNodeView)edge.input.node;
                    DialogueNodeView outNode = (DialogueNodeView)edge.output.node;

                    inNode.DisconnectIn(edge.input, outNode);
                    outNode.DisconnectOut(edge.output, inNode);
                }
            }
            return changes;

        }

        private void AddSearchWindow()
        {
            if (searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<DialogueSearchWindow>();
                searchWindow.Initialize(this);
            }

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new();

            gridBackground.StretchToParentSize();

            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            this.AddStyleSheets(
                "Packages/martis99.node-based-dialogue-system/Editor Default Resources/Dialogue/DialogueGraphViewStyles.uss",
                "Packages/martis99.node-based-dialogue-system/Editor Default Resources/Dialogue/DialogueNodeStyles.uss"
            );
        }

        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition -= editorWindow.position.position;
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }
    }
}
