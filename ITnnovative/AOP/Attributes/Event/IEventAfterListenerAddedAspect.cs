using ITnnovative.AOP.Processing.Execution;
using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IEventAfterListenerAddedAspect : IEventAddedListenerAspect
    {
        void AfterEventListenerAdded(AspectData arguments);
    }
}