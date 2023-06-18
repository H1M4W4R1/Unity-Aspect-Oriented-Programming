using System;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IEventExceptionThrownAspect
    {

        void OnExceptionThrown(Exception exception, AspectExecutionArgs args);

    }
}