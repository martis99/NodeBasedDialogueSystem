using Dialogue.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dialogue.Data
{
    /// <summary>
    /// Person node data which dialogue starts with
    /// </summary>
    public class DialoguePerson : DialogueInteract
    {
        public List<string> Questions;

        public string Name;

        public DialoguePerson() : base()
        {
            Questions = new();
            Name = "Name";
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

            CurrentState = State;

            return CallNext(graph, null, q => q.GetNextQuestion(graph));
        }

        public override List<DialogueBase> GetChoices(DialogueGraph graph)
        {
            List<string> nodes = Questions.FindAll(question => !graph.Nodes[question].IsCompleted(graph)).OrderBy(question => graph.Nodes[question].Position.y).ToList();
            List<DialogueBase> choices = new();

            foreach (string node in nodes)
            {
                choices.AddRange(graph.Nodes[node].GetChoices(graph));
            }

            return choices;
        }
    }
}
