using System;
using System.Collections.Generic;
using System.Linq;
using ITnnovative.AOP.Attributes.Event;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Attributes.Property;
using ITnnovative.AOP.Processing.Execution;
using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Processing
{
    /// <summary> 
    /// AOP Processor is a static class responsible for processing AOP Operations
    /// </summary>
    public static class AOPProcessor
    {
        public static AspectData OnT<T>(object instance, Type type, string methodName, object[] args)
        {
            var method = type.GetMethod(methodName);
            if (method == null) throw new Exception("We don't know what went wrong... But we know that it went terribly wrong.");
            
            // Parse parameters
            var mParam = method.GetParameters();

            var aa = new List<MethodArgument>();

            // Generate arguments for execution args
            for (var index = 0; index < args.Length; index++)
            {
                var pValue = args[index];
                var pName = mParam[index].Name;
                aa.Add(new MethodArgument(pName, pValue));
            }

            var arguments = new AspectExecutionArgs()
                {source = instance, arguments = aa};

            var aspects = method.GetCustomAttributes(typeof(T), true);
            foreach (var aspect in aspects)
            {
                // A nice tree of available aspect bases ;)
                if (typeof(IMethodEnterAspect).IsAssignableFrom(typeof(T)))
                    ((IMethodEnterAspect) aspect).OnMethodEnter(arguments);
                if (typeof(IMethodExitAspect).IsAssignableFrom(typeof(T)))
                    ((IMethodExitAspect) aspect).OnMethodExit(arguments);
                if (typeof(IPropertyGetEnterAspect).IsAssignableFrom(typeof(T)))
                    ((IPropertyGetEnterAspect) aspect).OnPropertyGetEnter(arguments);
                if (typeof(IPropertyGetExitAspect).IsAssignableFrom(typeof(T)))
                    ((IPropertyGetExitAspect) aspect).OnPropertyGetExit(arguments);
                if (typeof(IPropertySetEnterAspect).IsAssignableFrom(typeof(T)))
                    ((IPropertySetEnterAspect) aspect).OnPropertySetEnter(arguments);
                if (typeof(IPropertySetExitAspect).IsAssignableFrom(typeof(T)))
                    ((IPropertySetExitAspect) aspect).OnPropertySetExit(arguments);
                if (typeof(IEventAfterInvokedAspect).IsAssignableFrom(typeof(T)))
                    ((IEventAfterInvokedAspect) aspect).AfterEventInvoked(arguments);
                if (typeof(IEventBeforeInvokedAspect).IsAssignableFrom(typeof(T)))
                    ((IEventBeforeInvokedAspect) aspect).BeforeEventInvoked(arguments);
                if (typeof(IEventBeforeListenerAddedAspect).IsAssignableFrom(typeof(T)))
                    ((IEventBeforeListenerAddedAspect) aspect).BeforeEventListenerAdded(arguments);
                if (typeof(IEventAfterListenerAddedAspect).IsAssignableFrom(typeof(T)))
                    ((IEventAfterListenerAddedAspect) aspect).AfterEventListenerAdded(arguments);
                if (typeof(IEventBeforeListenerRemovedAspect).IsAssignableFrom(typeof(T)))
                    ((IEventBeforeListenerRemovedAspect) aspect).BeforeEventListenerRemoved(arguments);
                if (typeof(IEventAfterListenerRemovedAspect).IsAssignableFrom(typeof(T)))
                    ((IEventAfterListenerRemovedAspect) aspect).AfterEventListenerRemoved(arguments);
            }

            return new AspectData()
            {
                arguments = arguments.arguments.Select(q => q.value).ToArray(),
                hasErrored = arguments.HasErrored,
                returnValue = arguments.GetReturnValue(),
                thrownException = arguments.GetException(),
                hasReturned = arguments.HasReturned
            };
        }

        public static AspectData OnMethodStart(object instance, Type type, string methodName, object[] args) =>
            OnT<IMethodEnterAspect>(instance, type, methodName, args);
        public static AspectData OnMethodComplete(object instance, Type type, string methodName, object[] args) =>
            OnT<IMethodExitAspect>(instance, type, methodName, args);
        
        public static AspectData OnPropertyGetEnter(object instance, Type type, string methodName, object[] args) =>
            OnT<IPropertyGetEnterAspect>(instance, type, methodName, args);

        public static AspectData OnPropertyGetExit(object instance, Type type, string methodName, object[] args) =>
            OnT<IPropertyGetExitAspect>(instance, type, methodName, args);
        
        public static AspectData OnPropertySetEnter(object instance, Type type, string methodName, object[] args) =>
            OnT<IPropertySetEnterAspect>(instance, type, methodName, args);

        public static AspectData OnPropertySetExit(object instance, Type type, string methodName, object[] args) =>
            OnT<IPropertySetExitAspect>(instance, type, methodName, args);
        
        public static AspectData OnEventAddListenerEnter(object instance, Type type, string methodName, object[] args) =>
            OnT<IEventBeforeListenerAddedAspect>(instance, type, methodName, args);

        public static AspectData OnEventAddListenerExit(object instance, Type type, string methodName, object[] args) =>
            OnT<IEventAfterListenerAddedAspect>(instance, type, methodName, args);
        
        public static AspectData OnEventRemoveListenerEnter(object instance, Type type, string methodName, object[] args) =>
            OnT<IEventBeforeListenerRemovedAspect>(instance, type, methodName, args);

        public static AspectData OnEventRemoveListenerExit(object instance, Type type, string methodName, object[] args) =>
            OnT<IEventAfterListenerRemovedAspect>(instance, type, methodName, args);
        
        public static AspectData OnEventInvokeEnter(object instance, Type type, string methodName, object[] args) =>
            OnT<IEventBeforeInvokedAspect>(instance, type, methodName, args);

        public static AspectData OnEventInvokeExit(object instance, Type type, string methodName, object[] args) =>
            OnT<IEventAfterInvokedAspect>(instance, type, methodName, args);
    }
}