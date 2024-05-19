using Dialogue.Enumerations;

namespace Dialogue.Data
{
    public abstract class DialogueNode : DialogueBase
    {
        public string GroupID;
        public DialogueNodeState State;

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
