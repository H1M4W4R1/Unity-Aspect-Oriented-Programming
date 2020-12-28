using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ITnnovative.AOP.Processing.Exectution.Arguments
{
    public class MethodExecutionArguments : BaseExecutionArgs
    {

        /// <summary>
        /// Method that is executed, includes DeclaringType information, so it's ommited
        /// </summary>
        public MethodInfo method;
        
        /// <summary>
        /// Arguments for this method
        /// </summary>
        public List<MethodArgument> arguments = new List<MethodArgument>();

        /// <summary>
        /// Return value
        /// </summary>
        public object returnValue;

        /// <summary>
        /// Gets argument for method execution
        /// </summary>
        public T GetArgument<T>(string name)
        {
            return (T) arguments.FirstOrDefault(a => a.name.Equals(name))?.value;
        }

     
    }
}