namespace Dialogue.Data
{
    public abstract class DialogueLogic : DialogueNode
    {
        public DialogueLogic() : base()
        {

        }

        public override bool IsWaiting(DialogueGraph graph)
        {
            return CallNext(graph, false, q => q.IsWaiting(graph));
        }

        public override bool IsCurrent(DialogueGraph graph)
        {
            return CallNext(graph, false, q => q.IsCurrent(graph));
        }

        public override bool IsRunning(DialogueGraph graph)
        {
            return CallNext(graph, false, q => q.IsRunning(graph));
        }

        public override bool IsCompleted(DialogueGraph graph)
        {
            return CallNext(graph, false, q => q.IsCompleted(graph));
        }
    }
}
