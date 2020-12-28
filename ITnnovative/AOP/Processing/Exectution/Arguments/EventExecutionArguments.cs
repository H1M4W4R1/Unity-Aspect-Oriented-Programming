using System.Reflection;
using ITnnovative.AOP.Processing.Exectution.Arguments.Enums;

namespace ITnnovative.AOP.Processing.Exectution.Arguments
{
    public class EventExecutionArguments : BaseExecutionArgs
    {

        /// <summary>
        /// Event action type
        /// </summary>
        public EventExecutionType executionType; 
        
        /// <summary>
        /// Describes listener modifications or event parameters for invocation
        /// </summary>
        public object[] arguments;

        /// <summary>
        /// Event
        /// </summary>
        public EventInfo eventObject;

    }
}