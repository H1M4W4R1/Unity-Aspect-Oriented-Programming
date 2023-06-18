using System;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Processing.Execution;
using UnityEngine;

namespace ITnnovative.AOP.Sample.Aspects
{
    public class LogArgumentsAttribute : Attribute, IMethodEnterAspect
    {
        public void OnMethodEnter(AspectData args)
        {
            Debug.Log("Logging arguments...");
            var list = args.GetArguments();
            for (var index = 0; index < list.Count; index++)
            {
                var arg = list[index];
                Debug.Log($"[{index}] {arg.name} -> {arg.value}");
            }
        }
    }
}