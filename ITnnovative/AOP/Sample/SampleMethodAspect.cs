using System;
using System.Diagnostics;
using ITnnovative.AOP.Attributes;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Processing.Exectution;
using ITnnovative.AOP.Processing.Exectution.Arguments;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ITnnovative.AOP.Sample
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SampleMethodAspect : Attribute, IMethodBoundaryAspect
    {
        public void OnMethodEnter(MethodExecutionArguments args)
        {
            Stopwatch sw = new Stopwatch();
            args.AddVariable("stopwatch", sw);
            sw.Start();
        }

        public void OnMethodExit(MethodExecutionArguments args)
        {
            var sw = args.GetVariable<Stopwatch>("stopwatch");
            sw.Stop();
            Debug.Log("Elapsed: " + sw.ElapsedMilliseconds + " ms");
        }
    }
}