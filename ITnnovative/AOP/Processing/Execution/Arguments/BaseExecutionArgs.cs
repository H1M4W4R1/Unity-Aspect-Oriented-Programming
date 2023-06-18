﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ITnnovative.AOP.Processing.Execution.Arguments
{
    public class BaseExecutionArgs
    {
        /// <summary>
        /// Source object of this call (null if method is static)
        /// </summary>
        public object source;

        public List<MethodArgument> arguments;

        public object returnValue;
        
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
        public bool HasErrored => exception != null;

        /// <summary>
        /// Defines if args have return value
        /// </summary>
        public bool hasReturned = false;
        
        /// <summary>
        /// Custom variables for storing data
        /// </summary>
        private List<CustomVariable> _variables = new List<CustomVariable>();
        
        /// <summary>
        /// Method exception
        /// </summary>
        public Exception exception = null;
        
        /// <summary>
        /// Invoked when exception is raised
        /// </summary>
        public Action<Exception> onException;

        /// <summary>
        /// Throw exception before method is executed
        /// </summary>
        /// <param name="ex"></param>
        public void Throw(Exception ex)
        {
            exception = ex;
        }
    }
}