using Dialogue.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dialogue.Data
{
    /// <summary>
    /// Question node data
    /// </summary>
    public class DialogueQuestion : DialogueInteract
    {
        /// <summary>
        /// List of available choices
        /// </summary>
        public List<string> Choices;

        /// <summary>
        /// Question
        /// </summary>
        public string Text;

        /// <summary>
        /// Selected choice ID
        /// </summary>
        public string Choice;

        public DialogueQuestion() : base()
        {
            Choices = new();
            Text = "Text";
            Choice = null;
        }

        protected override T CallNext<T>(DialogueGraph graph, T defaultValue, Func<DialogueBase, T> method)
        {
            return string.IsNullOrEmpty(Choice) ? defaultValue : method(graph.Nodes[Choice]);
        }

        public override DialogueBase GetNextQuestion(DialogueGraph graph)
        {
            if (State == DialogueNodeState.Waiting)
            {
                State = DialogueNodeState.Current;
                return this;
            }

            if (State == DialogueNodeState.Current)
            {
                State = DialogueNodeState.Running;
            }

            CurrentState = State;

            DialogueBase question = CallNext(graph, null, q => q.GetNextQuestion(graph));

            if (question != null)
            {
                List<DialogueBase> choices = question.GetChoices(graph);
                choices.OfType<DialogueChoice>().ToList().ForEach(c => c.State = DialogueNodeState.Current);
            }

            return question;
        }

        public override DialogueBase GetQuestion(DialogueGraph graph)
        {
            if (State == DialogueNodeState.Current)
            {
                return this;
            }

            return CallNext(graph, null, q => q.GetQuestion(graph));
        }

        public override List<DialogueBase> GetChoices(DialogueGraph graph)
        {
            List<string> nodes = Choices.FindAll(choice => !graph.Nodes[choice].IsCompleted(graph)).OrderBy(choice => graph.Nodes[choice].Position.y).ToList();
            List<DialogueBase> choices = new();

            foreach (string node in nodes)
            {
                choices.AddRange(graph.Nodes[node].GetChoices(graph));
            }

            return choices;
        }

        public override int Forget(DialogueGraph graph)
        {
            return base.Forget(graph) + Choices.Aggregate(0, (acc, q) => acc + graph.Nodes[q].Forget(graph));
        }
    }
}
