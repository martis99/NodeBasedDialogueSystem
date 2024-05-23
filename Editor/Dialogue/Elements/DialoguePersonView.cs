using Dialogue.Data;
using Dialogue.Utilities;
using Dialogue.Windows;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Elements
{
    public class DialoguePersonView : DialogueNodeView
    {
        public DialoguePerson Data { get; set; }

        private Port questionPort;

        public override void Init(DialogueGraphView graphView, DialogueBase node)
        {
            Data = (DialoguePerson)node;

            base.Init(graphView, node);
        }

        protected override Color GetTitleColor()
        {
            return new Color32(255, 81, 84, 255);
        }

        protected override void CreateInputPorts()
        {

        }

        protected override void CreateOutputPorts()
        {
            questionPort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            questionPort.portName = "Question";
            outputContainer.Add(questionPort);
        }

        protected override void CreateCustomData()
        {
            TextField nameTextField = DialogueElementUtility.CreateTextField(so, Data.Name, "Name", callback =>
            {

            });

            customDataContainer.Add(nameTextField);
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
