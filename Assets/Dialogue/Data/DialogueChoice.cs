using Dialogue.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dialogue.Data
{
    public class DialogueChoice : DialogueInteract
    {
        public List<string> Questions;
        public string Text;

        public DialogueChoice() : base()
        {
            Questions = new();
            Text = "Text";
        }

        protected override T CallNext<T>(DialogueGraph graph, T defaultValue, Func<DialogueBase, T> method)
        {
            List<string> questions = Questions.FindAll(q => graph.Nodes[q].IsCurrent(graph)).ToList();

            if (questions.Count == 0)
            {
                questions = Questions.FindAll(q => graph.Nodes[q].IsRunning(graph)).ToList();
            }

            if (questions.Count == 0)
            {
                questions = Questions.FindAll(q => graph.Nodes[q].IsWaiting(graph)).ToList();
            }

            string question = questions.OrderBy(q => graph.Nodes[q].Position.y).DefaultIfEmpty("").First();
            return string.IsNullOrEmpty(question) ? defaultValue : method(graph.Nodes[question]);
        }

        public override DialogueBase GetNextQuestion(DialogueGraph graph)
        {
            if (State == DialogueNodeState.Waiting)
            {
                State = DialogueNodeState.Running;
            }

            if (State == DialogueNodeState.Current)
            {
                State = DialogueNodeState.Running;
            }

            CurrentState = State;

            return CallNext(graph, null, q => q.GetNextQuestion(graph));
        }

        public override List<DialogueBase> GetChoices(DialogueGraph graph)
        {
            return CurrentState == DialogueNodeState.Completed ? new List<DialogueBase>() : new List<DialogueBase>() { this };
        }

        public override int Forget(DialogueGraph graph)
        {
            return base.Forget(graph) + Questions.Aggregate(0, (acc, q) => acc + graph.Nodes[q].Forget(graph));
        }
    }
}
