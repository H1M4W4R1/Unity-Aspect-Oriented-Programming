using System;

namespace ITnnovative.AOP.Processing.Execution
{
    public class AspectReturnData
    {

        public object[] arguments;

        public Exception thrownException;
        
        public bool hasErrored;

        public object returnValue;
        
        public bool hasReturned;

    }
}