using System;
using System.Collections.Generic;
using System.Reflection;
using ITnnovative.AOP.Testing.Framework.Enums;

namespace ITnnovative.AOP.Testing.Framework.Data
{
    [Serializable]
    public class AOPAssemblyTestGroup
    {
        public bool show;
        
        public TestState passed;

        public List<AOPTypeTestGroup> typeTests = new List<AOPTypeTestGroup>();

        public Assembly src;
        
        public void Run()
        {
            foreach (var tt in typeTests)
            {
                try
                {
                    tt.Run();
                    passed = TestState.Passed;
                }
                catch
                {
                    passed = TestState.Failed;
                }
            }
        }

        public void AddTypeGroup(AOPTypeTestGroup testGroup)
        {
            typeTests.Add(testGroup);
        }
    }
}