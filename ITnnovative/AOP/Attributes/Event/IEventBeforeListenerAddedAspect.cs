using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IEventBeforeListenerAddedAspect : IEventAddedListenerAspect
    {
        void BeforeEventListenerAdded(EventExecutionArguments arguments);
    }
}