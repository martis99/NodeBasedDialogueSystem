using Dialogue.Enumerations;

namespace Dialogue.Data
{
    public abstract class DialogueInteract : DialogueNode
    {
        public DialogueInteract() : base()
        {

        }

        public override bool IsWaiting(DialogueGraph graph)
        {
            return CurrentState == DialogueNodeState.Waiting;
        }

        public override bool IsCurrent(DialogueGraph graph)
        {
            return CurrentState == DialogueNodeState.Current;
        }

        public override bool IsRunning(DialogueGraph graph)
        {
            return CurrentState == DialogueNodeState.Running;
        }

        public override bool IsCompleted(DialogueGraph graph)
        {
            return CurrentState == DialogueNodeState.Completed;
        }
    }
}
