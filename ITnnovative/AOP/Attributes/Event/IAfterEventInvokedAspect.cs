using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IAfterEventInvokedAspect : IEventInvokedAspect
    {
        void BeforeEventInvoked(EventExecutionArguments args);
    }
}