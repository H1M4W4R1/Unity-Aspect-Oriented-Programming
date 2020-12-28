using System;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Property
{
    public interface IPropertyExceptionThrownAspect : IMethodAspect
    {

        void OnExceptionThrown(Exception exception, PropertyExecutionArguments args);

    }
}