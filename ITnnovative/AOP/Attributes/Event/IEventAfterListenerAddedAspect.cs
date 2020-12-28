using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IEventAfterListenerAddedAspect : IEventAddedListenerAspect
    {
        void AfterEventListenerAdded(EventExecutionArguments arguments);
    }
}