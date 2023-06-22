using System;
using System.Collections.Generic;
using ITnnovative.AOP.Testing.Framework.Enums;
using UnityEngine.Assertions;

namespace ITnnovative.AOP.Testing.Framework.Data
{
    [Serializable]
    public class AOPTypeTestGroup
    {
        public bool show;
        
        public TestState passed;

        public List<AOPTest> tests = new List<AOPTest>();

        public Type src;
        
        public void Run()
        {
            var anyFail = false;
            foreach (var t in tests)
            {
                try
                {
                    t.Run();
                    passed = TestState.Passed;
                }
                catch
                {
                    passed = TestState.Failed;
                    anyFail = true;
                }
            }

            if (anyFail) throw new AssertionException("Tests failed", "See test runner.");
        }

        public void AddTest(AOPTest test)
        {
            tests.Add(test);
        }
    }
}