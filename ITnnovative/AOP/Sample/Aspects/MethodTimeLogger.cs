using System;
using System.Diagnostics;
using System.Text;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Processing.Execution.Arguments;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace ITnnovative.AOP.Sample.Aspects
{
    /// <summary>
    /// This is an aspect that prints execution time of a method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodTimeLogger : Attribute, IMethodBoundaryAspect
    {
        /// <summary>
        /// Name of the Stopwatch variable
        /// </summary>
        public const string NAME_STOPWATCH_VARIABLE = "timer";
        
        public void OnMethodExit(MethodExecutionArguments args)
        {
            // Get Stopwatch variable
            var sw = args.GetVariable<Stopwatch>(NAME_STOPWATCH_VARIABLE);
            sw.Stop();
            
            // Create list of arguments for method
            var paramList = new StringBuilder();
            var sbGeneric = new StringBuilder();
            var num = 0;
            
            // Generate generic string
            foreach (var genericArgument in args.method.GetGenericArguments())
            {
                if (num > 0)
                    sbGeneric.Append(", ");
                sbGeneric.Append(genericArgument);
                num++;
            }

            num = 0;
            
            // Generate param string
            args.arguments.ForEach(a =>
            {
                if(num > 0)
                    paramList.Append(", ");
                paramList.Append(a.name);
                num++;
            });
            Debug.Log("[MethodTimeLogger] Method '" +
                      args.rootMethod.Name + (!string.IsNullOrEmpty(sbGeneric.ToString()) ? "<" + sbGeneric + ">" : "") + "(" + paramList + ")' took " + sw.ElapsedMilliseconds + "ms.");
        }

        public void OnMethodEnter(MethodExecutionArguments args)
        {
            // Create stopwatch to measure time and start it
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            // Register variable in arguments
            args.AddVariable(NAME_STOPWATCH_VARIABLE, stopwatch);
        }
    }
}
