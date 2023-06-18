using System;
using System.Collections.Generic;
using System.Linq;
using ITnnovative.AOP.Attributes.Event;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Attributes.Property;
using ITnnovative.AOP.Processing.Execution;
using ITnnovative.AOP.Processing.Execution.Arguments;
using ITnnovative.AOP.Processing.Execution.Arguments.Enums;
using UnityEngine;

namespace ITnnovative.AOP.Processing
{
    /// <summary> 
    /// AOP Processor is a static class responsible for processing AOP Operations
    /// </summary>
    public static class AOPProcessor
    {
      
        public const string APPENDIX = "_UAOP";

        public static void BeforeEventInvoked(object instance, Type type, string eventName)
        {
            var arguments = new EventExecutionArguments();
            arguments.source = instance;
            var evt = type?.GetEvent(eventName);
            arguments.executionType = EventExecutionType.Invoke;
            arguments.eventObject = evt;
            arguments.arguments = null;
            
            var startAspects = evt.GetCustomAttributes(typeof(IBeforeEventInvokedAspect), true);
            
            foreach (var aspect in startAspects)
            {
                ((IBeforeEventInvokedAspect) aspect).BeforeEventInvoked(arguments);
            }
        }

        public static void AfterEventInvoked(object instance, Type type, string eventName)
        {
            var arguments = new EventExecutionArguments();
            arguments.source = instance;
            var evt = type?.GetEvent(eventName);
            arguments.executionType = EventExecutionType.Invoke;
            arguments.eventObject = evt;
            arguments.arguments = null;
            
            var startAspects = evt.GetCustomAttributes(typeof(IAfterEventInvokedAspect), true);
            
            foreach (var aspect in startAspects)
            {
                ((IAfterEventInvokedAspect) aspect).BeforeEventInvoked(arguments);
            }
        } 
        
        public static AspectData OnMethodStart(object instance, Type type, string methodName, object[] args)
        {
            var method = type.GetMethod(methodName);
            
            var arguments = new MethodExecutionArguments();
            arguments.source = instance;

            // Parse parameters
            var mParam = method.GetParameters();

            // Generate arguments for execution args
            for (var index = 0; index < args.Length; index++)
            {
                var pValue = args[index];
                var pName = mParam[index].Name;
                arguments.arguments.Add(new MethodArgument(pName, pValue));
            }
            
            var aspects = method.GetCustomAttributes(typeof(IMethodEnterAspect), true);
            foreach (var aspect in aspects)
            {
                ((IMethodEnterAspect) aspect).OnMethodEnter(arguments);
            }
            
            // TODO: HasReturned IMPL
            return new AspectData()
            {
                arguments = arguments.arguments.Select(q => q.value).ToArray(),
                hasErrored = arguments.HasErrored,
                returnValue = arguments.returnValue,
                thrownException = arguments.exception
            };
        }
        
        public static AspectData OnMethodComplete(object instance, Type type, string methodName, object[] args)
        {
            var method = type.GetMethod(methodName);
            
            var arguments = new MethodExecutionArguments();
            arguments.source = instance;

            // Parse parameters
            var mParam = method.GetParameters();

            // Generate arguments for execution args
            for (var index = 0; index < args.Length; index++)
            {
                var pValue = args[index];
                var pName = mParam[index].Name;
                arguments.arguments.Add(new MethodArgument(pName, pValue));
            }
            
            var aspects = method.GetCustomAttributes(typeof(IMethodExitAspect), true);
            foreach (var aspect in aspects)
            {
                ((IMethodExitAspect) aspect).OnMethodExit(arguments);
            }
            
            // TODO: HasReturned IMPL
            return new AspectData()
            {
                arguments = arguments.arguments.Select(q => q.value).ToArray(),
                hasErrored = arguments.HasErrored,
                returnValue = arguments.returnValue,
                thrownException = arguments.exception
            };
        }

    }
}