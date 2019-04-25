namespace Wokarol.StateMachineSystem
{
    public interface IHasExitState
    {
        State ExitState { get; set; }
    }
}