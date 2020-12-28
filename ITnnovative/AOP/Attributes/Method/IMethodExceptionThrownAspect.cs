using System;
using ITnnovative.AOP.Processing.Exectution;
using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Method
{
    public interface IMethodExceptionThrownAspect : IMethodAspect
    {

        void OnExceptionThrown(Exception exception, MethodExecutionArguments args);

    }
}