using System;
using System.Diagnostics;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Processing.Execution;
using Debug = UnityEngine.Debug;

namespace ITnnovative.AOP.Sample.Aspects
{
    /// <summary>
    /// This is an aspect that prints execution time of a method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)] 
    public class MethodTimeLogger : Attribute, IMethodBoundaryAspect
    {
        private string _title;
        
        /// <summary>
        /// Name of the Stopwatch variable
        /// </summary>
        public const string NAME_STOPWATCH_VARIABLE = "timer";
        
        public void OnMethodExit(AspectData args)
        {
            // Get Stopwatch variable
            var sw = args.GetVariable<Stopwatch>(NAME_STOPWATCH_VARIABLE);
            sw.Stop();
            Debug.Log($"[MethodTimeLogger] Method {_title} execution took {sw.ElapsedMilliseconds}ms.");
        }

        public void OnMethodEnter(AspectData args)
        {
            // Create stopwatch to measure time and start it
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            // Register variable in arguments
            args.AddVariable(NAME_STOPWATCH_VARIABLE, stopwatch);
        }

        public MethodTimeLogger(string varTitle)
        {
            _title = varTitle;
        }
    }
}
