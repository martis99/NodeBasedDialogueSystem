using Dialogue.Enumerations;

namespace Dialogue.Data
{
    /// <summary>
    /// Base dialogue node data class
    /// </summary>
    public abstract class DialogueNode : DialogueBase
    {
        /// <summary>
        /// Group ID which node belongs to
        /// </summary>
        public string GroupID;

        /// <summary>
        /// Initial node state
        /// </summary>
        public DialogueNodeState State;

        /// <summary>
        /// Current node state
        /// </summary>
        public DialogueNodeState CurrentState;

        public DialogueNode()
        {
            GroupID = "";
            State = DialogueNodeState.Waiting;
        }

        public override void Init()
        {
            State = DialogueNodeState.Waiting;
            CurrentState = State;
        }

        public override DialogueBase GetQuestion(DialogueGraph graph)
        {
            return CallNext(graph, null, q => q.GetQuestion(graph));
        }

        public override int Updates(DialogueGraph graph)
        {
            CurrentState = State;
            return base.Updates(graph);
        }

        public override int Forget(DialogueGraph graph)
        {
            State = DialogueNodeState.Waiting;
            CurrentState = State;
            return base.Forget(graph);
        }
    }
}
