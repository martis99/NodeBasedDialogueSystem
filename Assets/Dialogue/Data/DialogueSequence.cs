using Dialogue.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dialogue.Data
{
    public class DialogueSequence : DialogueLogic
    {
        public List<string> Questions;
        public DialogueSequenceType Sequence;

        public string CurrentQuestion;

        public DialogueSequence() : base()
        {
            Questions = new();
            Sequence = DialogueSequenceType.Random;
        }

        public override void Init()
        {
            base.Init();

            CurrentQuestion = "";
        }

        protected override T CallNext<T>(DialogueGraph graph, T defaultValue, Func<DialogueBase, T> method)
        {
            if (string.IsNullOrEmpty(CurrentQuestion))
            {
                Forget(graph);
                CurrentQuestion = Questions[UnityEngine.Random.Range(0, Questions.Count)];
                CallNext(graph, null, q => q.GetNextQuestion(graph));
            }

            return string.IsNullOrEmpty(CurrentQuestion) ? defaultValue : method(graph.Nodes[CurrentQuestion]);
        }

        public override DialogueBase GetNextQuestion(DialogueGraph graph)
        {
            if (State == DialogueNodeState.Waiting)
            {
                State = DialogueNodeState.Running;
            }

            if (Questions.Count == 0)
            {
                return null;
            }

            Forget(graph);
            CurrentQuestion = Questions[UnityEngine.Random.Range(0, Questions.Count)];

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

        public override int Updates(DialogueGraph graph)
        {
            Forget(graph);
            CurrentQuestion = Questions[UnityEngine.Random.Range(0, Questions.Count)];
            CallNext(graph, null, q => q.GetNextQuestion(graph));
            return base.Updates(graph);
        }

        public override int Forget(DialogueGraph graph)
        {
            CurrentQuestion = "";
            return base.Forget(graph) + Questions.Aggregate(0, (acc, q) => acc + graph.Nodes[q].Forget(graph));
        }
    }
}
