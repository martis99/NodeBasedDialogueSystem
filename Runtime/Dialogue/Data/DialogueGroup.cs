using System;

namespace Dialogue.Data
{
    /// <summary>
    /// Group node data
    /// </summary>
    public class DialogueGroup : DialogueBase
    {
        /// <summary>
        /// Name of the group
        /// </summary>
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
