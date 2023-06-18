using System;
using System.Collections.Generic;
using System.Linq;

namespace ITnnovative.AOP.Processing.Execution.Arguments
{
    public class AspectExecutionArgs
    {
        /// <summary>
        /// Source object of this call (null if method is static)
        /// </summary>
        public object source;

        /// <summary>
        /// Arguments of this execution
        /// </summary>
        public List<MethodArgument> arguments;

        /// <summary>
        /// Custom variables for storing data
        /// </summary>
        private List<CustomVariable> _variables = new List<CustomVariable>();
        
        /// <summary>
        /// Adds variable to arguments
        /// </summary>
        public void AddVariable(string name, object value)
        {
            _variables.Add(new CustomVariable(name, value));
        }

        /// <summary>
        /// Gets variable from arguments
        /// </summary>
        public T GetVariable<T>(string name)
        {
            return (T) _variables.FirstOrDefault(var => var.name.Equals(name))?.value;
        }
        
        /// <summary>
        /// Checks if method has errored
        /// </summary>
        public bool HasErrored => _exception != null;

        /// <summary>
        /// Defines if args have return value
        /// </summary>
        private bool _hasReturned = false;

        /// <summary>
        /// Value to return from this object on <see cref="HasReturned"/>
        /// </summary>
        private object _returnValue;

        /// <summary>
        /// Defines if args have return value
        /// </summary>
        public bool HasReturned => _hasReturned;
        
        /// <summary>
        /// Method exception to throw at <see cref="HasErrored"/> <see cref="Throw"/>
        /// </summary>
        private Exception _exception = null;

        /// <summary>
        /// Get exception
        /// </summary>
        /// <returns></returns>
        public Exception GetException() => _exception;
        
        /// <summary>
        /// Return value of this execution
        /// </summary>
        public object GetReturnValue() => _returnValue;
        
        /// <summary>
        /// Throw exception before method is executed
        /// </summary>
        /// <param name="ex"></param>
        public void Throw(Exception ex)
        {
            _exception = ex;
        }
    }
}