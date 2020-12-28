using System;
using ITnnovative.AOP.Processing.Exectution;

namespace ITnnovative.AOP.Attributes.Method
{
    public interface IMethodBoundaryAspect : IMethodExitAspect, IMethodEnterAspect
    {
    }
}