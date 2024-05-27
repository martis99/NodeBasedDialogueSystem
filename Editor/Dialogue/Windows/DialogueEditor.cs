using Dialogue.Data;
using Dialogue.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Windows
{
    public class DialogueEditor : EditorWindow
    {
        private DialogueGraphView graphView;

        [MenuItem("Window/Dialogue/Graph")]
        public static void ShowEditor()
        {
            GetWindow<DialogueEditor>("Dialogue Graph");
        }

        private void CreateGUI()
        {
            AddGraphView();

            AddStyles();
        }

        private void OnSelectionChange()
        {
            DialogueGraph graph = Selection.activeObject as DialogueGraph;

            if (!graph && Selection.activeGameObject)
            {
                DialogueManager runner = Selection.activeGameObject.GetComponent<DialogueManager>();
                if (runner)
                {
                    graph = runner.Dialogue;
                }
            }

            if (!graph)
            {
                return;
            }

            if (!Application.isPlaying && !AssetDatabase.CanOpenAssetInEditor(graph.GetInstanceID()))
            {
                return;
            }

            graphView.SetGraph(graph);
        }

        private void AddGraphView()
        {
            graphView = new(this);

            graphView.StretchToParentSize();

            rootVisualElement.Add(graphView);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets(
                "Packages/martis99.node-based-dialogue-system/Editor Default Resources/Dialogue/DialogueVariables.uss"
            );
        }
    }
}
