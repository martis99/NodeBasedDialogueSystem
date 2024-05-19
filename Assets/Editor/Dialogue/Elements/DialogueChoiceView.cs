using Dialogue.Data;
using Dialogue.Utilities;
using Dialogue.Windows;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Elements
{
    public class DialogueChoiceView : DialogueNodeView
    {
        public DialogueChoice Data { get; set; }

        private Port questionPort;

        public override void Init(DialogueGraphView graphView, DialogueBase node)
        {
            Data = (DialogueChoice)node;

            base.Init(graphView, node);
        }

        protected override Color GetTitleColor()
        {
            return new Color32(137, 166, 251, 255);
        }

        protected override void CreateInputPorts()
        {
            InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            InputPort.portName = "Question";
            inputContainer.Add(InputPort);
        }

        protected override void CreateOutputPorts()
        {
            questionPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            questionPort.portName = "Question";
            outputContainer.Add(questionPort);
        }

        protected override void CreateCustomData()
        {
            TextField textTextField = DialogueElementUtility.CreateTextArea(so, Data.Text, "Text", callback =>
            {

            });

            textTextField.AddClasses(
                    "dialogue-node__text-field",
                    "dialogue-node__quote-text-field"
            );

            customDataContainer.Add(textTextField);
        }

        public override void ConnectOut(Port port, DialogueNodeView node)
        {
            if (port == questionPort)
            {
                Data.Questions.Add(node.GetID());
                return;
            }
        }

        public override void DisconnectOut(Port port, DialogueNodeView node)
        {
            if (port == questionPort)
            {
                Data.Questions.Remove(node.GetID());
                return;
            }
        }

        public override void LoadConnections(DialogueGraphView graphView, Dictionary<string, IDialogueView> nodes)
        {
            foreach (string question in Data.Questions)
            {
                DialogueNodeView questionNode = (DialogueNodeView)nodes[question];

                Edge edge = questionPort.ConnectTo(questionNode.InputPort);

                graphView.AddElement(edge);

                RefreshPorts();
            }
        }
    }
}
