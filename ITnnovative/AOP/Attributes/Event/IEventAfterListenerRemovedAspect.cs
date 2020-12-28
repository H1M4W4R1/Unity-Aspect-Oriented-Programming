using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IEventAfterListenerRemovedAspect : IEventRemovedListenerAspect
    {
        void AfterEventListenerRemoved(EventExecutionArguments arguments);
    }
}