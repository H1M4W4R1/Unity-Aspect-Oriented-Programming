using System;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IEventExceptionThrownAspect : IMethodAspect
    {

        void OnExceptionThrown(Exception exception, EventExecutionArguments args);

    }
}