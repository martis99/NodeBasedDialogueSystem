using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.Data
{
    public abstract class DialogueBase : ScriptableObject
    {
        public string ID;
        public Vector2 Position;

        public DialogueBase()
        {
            ID = Guid.NewGuid().ToString();
            Position = Vector2.zero;
        }

        public virtual void Init()
        {

        }

        protected abstract T CallNext<T>(DialogueGraph graph, T defaultValue, Func<DialogueBase, T> method);

        public virtual DialogueBase GetNextQuestion(DialogueGraph graph)
        {
            return GetQuestion(graph);
        }

        public virtual DialogueBase GetQuestion(DialogueGraph graph)
        {
            return CallNext(graph, null, q => q.GetQuestion(graph));
        }

        public virtual List<DialogueBase> GetChoices(DialogueGraph graph)
        {
            return new List<DialogueBase>();
        }

        public virtual bool IsWaiting(DialogueGraph graph)
        {
            return true;
        }

        public virtual bool IsCurrent(DialogueGraph graph)
        {
            return false;
        }

        public virtual bool IsRunning(DialogueGraph graph)
        {
            return false;
        }

        public virtual bool IsCompleted(DialogueGraph graph)
        {
            return false;
        }

        public virtual int Updates(DialogueGraph graph)
        {
            return CallNext(graph, 0, q => q.Updates(graph));
        }

        public virtual int Forget(DialogueGraph graph)
        {
            return 0;
        }
    }
}
