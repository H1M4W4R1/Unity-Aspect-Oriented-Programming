using System;
using ITnnovative.AOP.Attributes.Event;
using ITnnovative.AOP.Processing.Exectution.Arguments;
using UnityEngine;

namespace ITnnovative.AOP.Sample
{
    [AttributeUsage(AttributeTargets.Event)]
    public class SampleEventAspect : Attribute, IEventBeforeListenerAddedAspect,
        IBeforeEventInvokedAspect, IAfterEventInvokedAspect
    {
        public void BeforeEventListenerAdded(EventExecutionArguments arguments)
        {
            Debug.Log("Event listener added!");
            Debug.Log(arguments.arguments.Length);
        }

        void IBeforeEventInvokedAspect.BeforeEventInvoked(EventExecutionArguments args)
        {
            Debug.Log("Before event");
        }

        void IAfterEventInvokedAspect.BeforeEventInvoked(EventExecutionArguments args)
        {
            Debug.Log("After event");
        }
    }
}