using System;

namespace ITnnovative.AOP.Processing.Execution.Arguments
{
    public class ThrownException
    {
        public ExceptionSource source;
        public Exception exceptionObject;

        public ThrownException(ExceptionSource src, Exception obj)
        {
            source = src;
            exceptionObject = obj;
        }
    }
}