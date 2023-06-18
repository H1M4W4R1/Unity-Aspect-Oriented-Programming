using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IEventAfterInvokedAspect : IEventInvokedAspect
    {
        void AfterEventInvoked(BaseExecutionArgs args);
    }
}