using Dialogue.Data;
using Dialogue.Utilities;
using Dialogue.Windows;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Elements
{
    public class DialogueQuestionView : DialogueNodeView
    {
        public DialogueQuestion Data { get; set; }

        private Port choicePort;

        public override void Init(DialogueGraphView graphView, DialogueBase node)
        {
            title = "Question";
            Data = (DialogueQuestion)node;

            base.Init(graphView, node);
        }

        protected override Color GetTitleColor()
        {
            return new Color32(151, 206, 0, 255);
        }

        protected override void CreateInputPorts()
        {
            InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            InputPort.portName = "Choice";
            inputContainer.Add(InputPort);
        }

        protected override void CreateOutputPorts()
        {
            choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            choicePort.portName = "Choice";
            outputContainer.Add(choicePort);
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

            TextField choiceTextField = DialogueElementUtility.CreateTextArea(so, Data.Choice, "Choice", callback =>
            {

            });

            choiceTextField.AddClasses(
                    "dialogue-node__text-field",
                    "dialogue-node__quote-text-field"
            );

            customDataContainer.Add(choiceTextField);
        }

        public override void ConnectOut(Port port, DialogueNodeView node)
        {
            if (port == choicePort)
            {
                Data.Choices.Add(node.GetID());
            }
        }

        public override void DisconnectOut(Port port, DialogueNodeView node)
        {
            if (port == choicePort)
            {
                Data.Choices.Remove(node.GetID());
            }
        }

        public override void LoadConnections(DialogueGraphView graphView, Dictionary<string, IDialogueView> nodes)
        {
            foreach (string choice in Data.Choices)
            {
                DialogueNodeView choiceNode = (DialogueNodeView)nodes[choice];

                Edge edge = choicePort.ConnectTo(choiceNode.InputPort);

                graphView.AddElement(edge);

                RefreshPorts();
            }
        }
    }
}
