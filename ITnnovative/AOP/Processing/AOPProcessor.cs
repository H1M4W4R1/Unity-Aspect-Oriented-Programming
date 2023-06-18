using System;
using System.Collections.Generic;
using System.Linq;
using ITnnovative.AOP.Attributes.Event;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Attributes.Property;
using ITnnovative.AOP.Processing.Execution;
using ITnnovative.AOP.Processing.Execution.Arguments;
using UnityEngine;

namespace ITnnovative.AOP.Processing
{
    /// <summary> 
    /// AOP Processor is a static class responsible for processing AOP Operations
    /// </summary>
    public static class AOPProcessor
    {
        public static void OnT<T>(object instance, AspectData data, Type type, string methodName, object[] args)
        {
            data.SetSource(instance);
            
            var method = type.GetMethod(methodName);
            if (method == null) throw new Exception("We don't know what went wrong... But we know that it went terribly wrong.");

            // Parse parameters
            var mParam = method.GetParameters();

            // Generate arguments for execution args
            for (var index = 0; index < args.Length; index++)
            {
                var pValue = args[index];
                var pName = mParam[index].Name;
                
                // Register argument or update value
                data.SetArgument(index, pName, pValue);
            }

            var aspects = method.GetCustomAttributes(typeof(T), true);
            foreach (var aspect in aspects)
            {
                // A nice tree of available aspect bases ;)
                if (typeof(IMethodEnterAspect).IsAssignableFrom(typeof(T)))
                    ((IMethodEnterAspect) aspect).OnMethodEnter(data);
                if (typeof(IMethodExitAspect).IsAssignableFrom(typeof(T)))
                    ((IMethodExitAspect) aspect).OnMethodExit(data);
                if (typeof(IPropertyGetEnterAspect).IsAssignableFrom(typeof(T)))
                    ((IPropertyGetEnterAspect) aspect).OnPropertyGetEnter(data);
                if (typeof(IPropertyGetExitAspect).IsAssignableFrom(typeof(T)))
                    ((IPropertyGetExitAspect) aspect).OnPropertyGetExit(data);
                if (typeof(IPropertySetEnterAspect).IsAssignableFrom(typeof(T)))
                    ((IPropertySetEnterAspect) aspect).OnPropertySetEnter(data);
                if (typeof(IPropertySetExitAspect).IsAssignableFrom(typeof(T)))
                    ((IPropertySetExitAspect) aspect).OnPropertySetExit(data);
                if (typeof(IEventAfterInvokedAspect).IsAssignableFrom(typeof(T)))
                    ((IEventAfterInvokedAspect) aspect).AfterEventInvoked(data);
                if (typeof(IEventBeforeInvokedAspect).IsAssignableFrom(typeof(T)))
                    ((IEventBeforeInvokedAspect) aspect).BeforeEventInvoked(data);
                if (typeof(IEventBeforeListenerAddedAspect).IsAssignableFrom(typeof(T)))
                    ((IEventBeforeListenerAddedAspect) aspect).BeforeEventListenerAdded(data);
                if (typeof(IEventAfterListenerAddedAspect).IsAssignableFrom(typeof(T)))
                    ((IEventAfterListenerAddedAspect) aspect).AfterEventListenerAdded(data);
                if (typeof(IEventBeforeListenerRemovedAspect).IsAssignableFrom(typeof(T)))
                    ((IEventBeforeListenerRemovedAspect) aspect).BeforeEventListenerRemoved(data);
                if (typeof(IEventAfterListenerRemovedAspect).IsAssignableFrom(typeof(T)))
                    ((IEventAfterListenerRemovedAspect) aspect).AfterEventListenerRemoved(data);
            }

         
        }

        public static void OnMethodStart(object instance, AspectData data, Type type, string methodName, object[] args) =>
            OnT<IMethodEnterAspect>(instance, data, type, methodName, args);
        public static void OnMethodComplete(object instance, AspectData data, Type type, string methodName, object[] args) =>
            OnT<IMethodExitAspect>(instance, data, type, methodName, args);
        
        public static void OnPropertyGetEnter(object instance, AspectData data, Type type, string methodName, object[] args) =>
            OnT<IPropertyGetEnterAspect>(instance, data, type, methodName, args);

        public static void OnPropertyGetExit(object instance, AspectData data, Type type, string methodName, object[] args) =>
            OnT<IPropertyGetExitAspect>(instance, data, type, methodName, args);
        
        public static void OnPropertySetEnter(object instance, AspectData data, Type type, string methodName, object[] args) =>
            OnT<IPropertySetEnterAspect>(instance, data, type, methodName, args);

        public static void OnPropertySetExit(object instance, AspectData data, Type type, string methodName, object[] args) =>
            OnT<IPropertySetExitAspect>(instance, data, type, methodName, args);
        
        public static void OnEventAddListenerEnter(object instance, AspectData data, Type type, string methodName, object[] args) =>
            OnT<IEventBeforeListenerAddedAspect>(instance, data, type, methodName, args);

        public static void OnEventAddListenerExit(object instance, AspectData data, Type type, string methodName, object[] args) =>
            OnT<IEventAfterListenerAddedAspect>(instance, data, type, methodName, args);
        
        public static void OnEventRemoveListenerEnter(object instance, AspectData data, Type type, string methodName, object[] args) =>
            OnT<IEventBeforeListenerRemovedAspect>(instance, data, type, methodName, args);

        public static void OnEventRemoveListenerExit(object instance, AspectData data, Type type, string methodName, object[] args) =>
            OnT<IEventAfterListenerRemovedAspect>(instance, data, type, methodName, args);
        
        public static void OnEventInvokeEnter(object instance, AspectData data, Type type, string methodName, object[] args) =>
            OnT<IEventBeforeInvokedAspect>(instance, data, type, methodName, args);

        public static void OnEventInvokeExit(object instance, AspectData data, Type type, string methodName, object[] args) =>
            OnT<IEventAfterInvokedAspect>(instance, data, type, methodName, args);
    }
}