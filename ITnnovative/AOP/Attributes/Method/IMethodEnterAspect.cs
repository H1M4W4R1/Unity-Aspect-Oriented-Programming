using System;
using ITnnovative.AOP.Processing.Exectution;
using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Method
{
    public interface IMethodEnterAspect : IMethodAspect
    {
        /// <summary>
        /// Invoked when the method is entered
        /// </summary>
        void OnMethodEnter(MethodExecutionArguments args);

    }
}