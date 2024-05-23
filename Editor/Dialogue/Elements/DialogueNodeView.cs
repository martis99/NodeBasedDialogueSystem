using Dialogue.Data;
using Dialogue.Enumerations;
using Dialogue.Utilities;
using Dialogue.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Elements
{
    public abstract class DialogueNodeView : Node, IDialogueView
    {
        public DialogueGroupView Group { get; set; }

        protected DialogueGraphView graphView;

        public Port InputPort { get; protected set; }

        protected VisualElement customDataContainer;

        public EnumField ActiveField;

        private Color defaultBorderBottomColor;
        private float defaultBorderBottomWidth;
        private Color defaultBorderLeftColor;
        private float defaultBorderLeftWidth;
        private Color defaultBorderTopColor;
        private float defaultBorderTopWidth;
        private Color defaultBorderRightColor;
        private float defaultBorderRightWidth;

        public DialogueNode Node;
        protected SerializedObject so;

        public Port output;

        public Action<IDialogueView> OnNodeSelected;

        public virtual void Init(DialogueGraphView graphView, DialogueBase node)
        {
            this.graphView = graphView;

            Node = (DialogueNode)node;
            so = new SerializedObject(Node);

            title = node.name;
            viewDataKey = node.ID;

            style.left = node.Position.x;
            style.top = node.Position.y;

            mainContainer.AddClasses(
                "dialogue-node__main-container"
            );
            extensionContainer.AddClasses(
                "dialogue-node__extension-container"
            );

            defaultBorderBottomColor = contentContainer.style.borderBottomColor.value;
            defaultBorderBottomWidth = contentContainer.style.borderBottomWidth.value;
            defaultBorderLeftColor = contentContainer.style.borderLeftColor.value;
            defaultBorderLeftWidth = contentContainer.style.borderLeftWidth.value;
            defaultBorderTopColor = contentContainer.style.borderTopColor.value;
            defaultBorderTopWidth = contentContainer.style.borderTopWidth.value;
            defaultBorderRightColor = contentContainer.style.borderRightColor.value;
            defaultBorderRightWidth = contentContainer.style.borderRightWidth.value;

            customDataContainer = new();

            customDataContainer.AddClasses(
                "dialogue-node__custom-data-container"
            );

            extensionContainer.Add(customDataContainer);

            ActiveField = DialogueElementUtility.CreateEnumField(so, Node.State, "State", callback =>
            {
                Update();
            });

            customDataContainer.Add(ActiveField);

            titleContainer.style.backgroundColor = GetTitleColor();

            CreateCustomData();
            CreateInputPorts();
            CreateOutputPorts();

            Update();

            RefreshExpandedState();
        }

        protected abstract Color GetTitleColor();

        protected void Update()
        {
            if (Node.State == DialogueNodeState.Current)
            {
                SetBorder(new Color32(52, 140, 235, 255));
            }
            else if (Node.State == DialogueNodeState.Running)
            {
                SetBorder(new Color32(255, 214, 0, 255));
            }
            else if (Node.State == DialogueNodeState.Completed)
            {
                SetBorder(new Color32(149, 235, 52, 255));
            }
            else
            {
                ResetBorder();
            }
        }

        protected virtual void CreateCustomData()
        {

        }

        protected virtual void CreateInputPorts()
        {

        }

        protected virtual void CreateOutputPorts()
        {

        }

        public void SetOnNodeSelected(Action<IDialogueView> value)
        {
            OnNodeSelected = value;
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);

            Node.Position.x = newPos.xMin;
            Node.Position.y = newPos.yMin;
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }

        public void DisconnectAllPorts()
        {
            DisconnectInputPorts();
            DisconnectOutputPorts();
        }

        private void DisconnectInputPorts()
        {
            DisconnectPorts(inputContainer);
        }

        private void DisconnectOutputPorts()
        {
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children().OfType<Port>())
            {
                if (!port.connected)
                {
                    continue;
                }

                graphView.DeleteElements(port.connections);
            }
        }

        public void SetBorder(Color color)
        {
            contentContainer.style.borderBottomColor = color;
            contentContainer.style.borderBottomWidth = 2f;
            contentContainer.style.borderLeftColor = color;
            contentContainer.style.borderLeftWidth = 2f;
            contentContainer.style.borderTopColor = color;
            contentContainer.style.borderTopWidth = 2f;
            contentContainer.style.borderRightColor = color;
            contentContainer.style.borderRightWidth = 2f;
        }

        public void ResetBorder()
        {
            contentContainer.style.borderBottomColor = defaultBorderBottomColor;
            contentContainer.style.borderBottomWidth = defaultBorderBottomWidth;
            contentContainer.style.borderLeftColor = defaultBorderLeftColor;
            contentContainer.style.borderLeftWidth = defaultBorderLeftWidth;
            contentContainer.style.borderTopColor = defaultBorderTopColor;
            contentContainer.style.borderTopWidth = defaultBorderTopWidth;
            contentContainer.style.borderRightColor = defaultBorderRightColor;
            contentContainer.style.borderRightWidth = defaultBorderRightWidth;
        }

        public bool IsStartingNode()
        {
            return !InputPort.connected;
        }

        public string GetID()
        {
            return Node.ID;
        }

        public virtual void ConnectIn(Port port, DialogueNodeView node)
        {

        }

        public virtual void ConnectOut(Port port, DialogueNodeView node)
        {


        }
        public virtual void DisconnectIn(Port port, DialogueNodeView node)
        {

        }

        public virtual void DisconnectOut(Port port, DialogueNodeView node)
        {

        }

        public abstract void LoadConnections(DialogueGraphView graphView, Dictionary<string, IDialogueView> nodes);
    }
}
