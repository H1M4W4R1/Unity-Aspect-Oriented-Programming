using System;
using System.Reflection;
using ITnnovative.AOP.Testing.Framework.Enums;

namespace ITnnovative.AOP.Testing.Framework.Data
{
    [Serializable]
    public class AOPTest
    {

        public TestState passed;

        public MethodInfo method;

        /// <summary>
        /// Run the test
        /// </summary>
        public void Run()
        {
            try
            {
                method.Invoke(null, Array.Empty<object>());
                passed = TestState.Passed;
            }
            catch
            {
                passed = TestState.Failed;
                throw;
            }
        }

    }
}