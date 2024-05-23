using System;

namespace Dialogue.Data
{
    public class DialogueGroup : DialogueBase
    {
        public string Name;

        public DialogueGroup() : base()
        {
            Name = "Group";
        }

        protected override T CallNext<T>(DialogueGraph graph, T defaultValue, Func<DialogueBase, T> method)
        {
            throw new NotSupportedException();
        }
    }
}
