using System;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Processing.Exectution;
using ITnnovative.AOP.Processing.Exectution.Arguments;
using UnityEngine;

namespace ITnnovative.AOP.Sample
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SampleExceptionTest : Attribute, IMethodExceptionThrownAspect
    {
        public void OnExceptionThrown(Exception exception, MethodExecutionArguments args)
        {
            Debug.Log("Hello HERE!");
            Debug.LogError(exception);
        }
    }
}