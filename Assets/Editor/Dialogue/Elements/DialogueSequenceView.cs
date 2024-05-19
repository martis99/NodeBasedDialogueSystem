using Dialogue.Data;
using Dialogue.Utilities;
using Dialogue.Windows;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

using UnityEngine;

namespace Dialogue.Elements
{
    public class DialogueSequenceView : DialogueNodeView
    {
        public DialogueSequence Data { get; set; }

        private Port questionPort;

        public override void Init(DialogueGraphView graphView, DialogueBase node)
        {
            Data = (DialogueSequence)node;

            base.Init(graphView, node);
        }

        protected override Color GetTitleColor()
        {
            return new Color32(246, 176, 45, 255);
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
            UnityEngine.UIElements.EnumField sequenceField = DialogueElementUtility.CreateEnumField(so, Data.Sequence, "Sequence", callback =>
            {
                Update();
            });

            customDataContainer.Add(sequenceField);
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
