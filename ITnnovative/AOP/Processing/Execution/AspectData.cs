using System;
using System.Collections.Generic;
using System.Linq;
using ITnnovative.AOP.Processing.Execution.Arguments;
using UnityEngine;

namespace ITnnovative.AOP.Processing.Execution
{
    public class AspectData
    {
        private object _source;
        private Exception _exception;
        private object _returnValue;
        private ExceptionSource _currentExceptionSource = ExceptionSource.None;
        
        public bool isGenerated = false;

        /// <summary>
        /// Arguments for methods
        /// </summary>
        private List<MethodArgument> Arguments { get; } = new List<MethodArgument>();

        /// <summary>
        /// All thrown exceptions
        /// </summary>
        private List<ThrownException> Exceptions { get; } = new List<ThrownException>();

        /// <summary>
        /// Get argument at position
        /// </summary>
        public object GetArgument(int i) => Arguments[i].value;

        /// <summary>
        /// Get argument by name
        /// </summary>
        public object GetArgument(string name) => Arguments.FirstOrDefault(a => a.name.Equals(name));

        /// <summary>
        /// Get arguments
        /// </summary>
        public List<MethodArgument> GetArguments() => Arguments;

        /// <summary>
        /// Arguments amount
        /// </summary>
        public int ArgumentsCount => Arguments.Count;
        
        /// <summary>
        /// Set argument name and value (create if does not exist)
        /// </summary>
        public void SetArgument(int index, string name, object value)
        {
            if (ArgumentsCount <= index)
                Arguments.Add(new MethodArgument(name, value));
            else
                Arguments[index].value = value;
        }

        /// <summary>
        /// Set argument value using position
        /// </summary>
        public void SetArgument(int index, object value)
        {
            if(ArgumentsCount >= index) 
                throw new NullReferenceException($"Argument at index {index} is out of bounds!");
            
            var arg = Arguments[index];
            arg.value = value;
        }

        /// <summary>
        /// Set argument value using name
        /// </summary>
        public void SetArgument(string name, object value)
        {
            var arg = Arguments.FirstOrDefault(a => a.name.Equals(name));
            if(arg == null)  throw new NullReferenceException($"Argument with name `{name}` has not been found!");
            arg.value = value;
        }
        
        /// <summary>
        /// Custom variables for storing data
        /// </summary>
        private List<CustomVariable> _variables = new List<CustomVariable>();

        /// <summary>
        /// Get source object that this aspect is called from
        /// </summary>
        public object GetSource() => _source;
        
        /// <summary>
        /// Set source object that this aspect is called from (only for internal usage)
        /// </summary>
        public void SetSource(object obj) => _source = obj;

        /// <summary>
        /// Exception to throw by aspect`ed method
        /// </summary>
        public Exception GetException() => _exception;
        
        /// <summary>
        /// Register new exception
        /// </summary>
        public void SetExceptionThrow(ExceptionSource source, Exception exception)
        {
            SetException(source, exception);
            
            // If should throw this exception
            HasErrored = true;
        }

        /// <summary>
        /// Register new exception
        /// </summary>
        public void SetException(ExceptionSource source, Exception exception)
        {
            _currentExceptionSource = source;
            _exception = exception;
            
            // Register exception in stack
            Exceptions.Add(new ThrownException(source, exception));

        }

        /// <summary>
        /// True if <see cref="Throw"/> has been used
        /// </summary>
        public bool HasErrored { get; private set; }

        /// <summary>
        /// Value to be returned by aspect`ed method
        /// </summary>
        public object GetReturnValue() => _returnValue;
        
        /// <summary>
        /// True if <see cref="Return"/> has been used
        /// </summary>
        public bool HasReturned { get; private set; }

        /// <summary>
        /// Throw exception from this aspect (requires return statement afterwards)
        /// </summary>
        public void Throw(Exception ex)
        {
            _currentExceptionSource = ExceptionSource.Aspect;
            _exception = ex;
            HasErrored = true;
        }

        /// <summary>
        /// Return value from this aspect (requires return statement afterwards)
        /// </summary>
        public void Return(object value)
        {
            _returnValue = value;
            HasReturned = true;
        }

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

        public void SetExceptionSource(ExceptionSource src)
        {
            _currentExceptionSource = src;
        }
    }
}