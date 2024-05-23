using Dialogue.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dialogue.Data
{
    public class DialogueCondition : DialogueLogic
    {
        public List<string> Trues;
        public List<string> Falses;
        public string Name;
        public bool Value;

        public bool CurrentValue;

        public DialogueCondition() : base()
        {
            Trues = new();
            Falses = new();
            Name = "Name";
            Value = false;
            CurrentValue = Value;
        }

        protected override T CallNext<T>(DialogueGraph graph, T defaultValue, Func<DialogueBase, T> method)
        {
            List<string> allNodes = CurrentValue ? Trues : Falses;

            List<string> nodes = allNodes.FindAll(q => graph.Nodes[q].IsCurrent(graph)).ToList();

            if (nodes.Count == 0)
            {
                nodes = allNodes.FindAll(q => graph.Nodes[q].IsRunning(graph)).ToList();
            }

            if (nodes.Count == 0)
            {
                nodes = allNodes.FindAll(q => graph.Nodes[q].IsWaiting(graph)).ToList();
            }

            string question = nodes.OrderBy(question => graph.Nodes[question].Position.y).DefaultIfEmpty("").First();
            return string.IsNullOrEmpty(question) ? defaultValue : method(graph.Nodes[question]);
        }

        public override DialogueBase GetNextQuestion(DialogueGraph graph)
        {
            if (State == DialogueNodeState.Waiting)
            {
                State = DialogueNodeState.Running;
            }

            CurrentState = State;
            CurrentValue = Value;

            return CallNext(graph, null, q => q.GetNextQuestion(graph));
        }

        public override List<DialogueBase> GetChoices(DialogueGraph graph)
        {
            List<string> nodes = CurrentValue ? Trues : Falses;
            nodes = nodes.FindAll(question => !graph.Nodes[question].IsCompleted(graph)).OrderBy(question => graph.Nodes[question].Position.y).ToList();
            List<DialogueBase> choices = new();

            foreach (string node in nodes)
            {
                choices.AddRange(graph.Nodes[node].GetChoices(graph));
            }

            return choices;
        }

        public override int Updates(DialogueGraph graph)
        {
            CurrentValue = Value;
            return base.Updates(graph);
        }

        public override int Forget(DialogueGraph graph)
        {
            return base.Forget(graph) + Trues.Aggregate(0, (acc, q) => acc + graph.Nodes[q].Forget(graph)) + Falses.Aggregate(0, (acc, q) => acc + graph.Nodes[q].Forget(graph));
        }
    }
}
