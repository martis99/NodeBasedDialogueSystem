using Dialogue.Data;
using Dialogue.Utilities;
using Dialogue.Windows;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Elements
{
    public class DialogueConditionView : DialogueNodeView
    {
        public DialogueCondition Data { get; set; }

        private Port truePort;
        private Port falsePort;

        public override void Init(DialogueGraphView graphView, DialogueBase node)
        {
            Data = (DialogueCondition)node;

            base.Init(graphView, node);
        }

        protected override Color GetTitleColor()
        {
            return new Color32(246, 176, 45, 255);
        }

        protected override void CreateInputPorts()
        {
            InputPort = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(bool));
            InputPort.portName = "In";
            inputContainer.Add(InputPort);
        }

        protected override void CreateOutputPorts()
        {
            truePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            truePort.portName = "True";
            outputContainer.Add(truePort);

            falsePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            falsePort.portName = "False";
            outputContainer.Add(falsePort);
        }

        protected override void CreateCustomData()
        {
            TextField nameTextField = DialogueElementUtility.CreateTextField(so, Data.Name, "Name", callback =>
            {

            });

            customDataContainer.Add(nameTextField);

            Toggle valueToggle = DialogueElementUtility.CreateToggle(so, Data.Value, "Value", callback =>
            {

            });

            customDataContainer.Add(valueToggle);
        }

        public override void ConnectOut(Port port, DialogueNodeView node)
        {
            if (port == truePort)
            {
                Data.Trues.Add(node.GetID());
                return;
            }
            if (port == falsePort)
            {
                Data.Falses.Add(node.GetID());
                return;
            }
        }

        public override void DisconnectOut(Port port, DialogueNodeView node)
        {
            if (port == truePort)
            {
                Data.Trues.Remove(node.GetID());
                return;
            }
            if (port == falsePort)
            {
                Data.Falses.Remove(node.GetID());
                return;
            }
        }

        public override void LoadConnections(DialogueGraphView graphView, Dictionary<string, IDialogueView> nodes)
        {
            foreach (string nodeId in Data.Trues)
            {
                DialogueNodeView node = (DialogueNodeView)nodes[nodeId];

                Edge edge = truePort.ConnectTo(node.InputPort);

                graphView.AddElement(edge);

                RefreshPorts();
            }

            foreach (string nodeId in Data.Falses)
            {
                DialogueNodeView node = (DialogueNodeView)nodes[nodeId];

                Edge edge = falsePort.ConnectTo(node.InputPort);

                graphView.AddElement(edge);

                RefreshPorts();
            }
        }
    }
}
