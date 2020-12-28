using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IEventBeforeListenerRemovedAspect : IEventRemovedListenerAspect
    {
        void BeforeEventListenerRemoved(EventExecutionArguments arguments);
    }
}