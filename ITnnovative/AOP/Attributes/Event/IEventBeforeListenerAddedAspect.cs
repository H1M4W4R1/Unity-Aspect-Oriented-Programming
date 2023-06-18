﻿using ITnnovative.AOP.Processing.Execution;
using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Event
{
    public interface IEventBeforeListenerAddedAspect : IEventAddedListenerAspect
    {
        void BeforeEventListenerAdded(AspectData arguments);
    }
}