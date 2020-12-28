using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IBeforeEventInvokedAspect : IEventInvokedAspect
    {
        void BeforeEventInvoked(EventExecutionArguments args);
    }
}