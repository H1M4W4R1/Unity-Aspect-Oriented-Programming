using ITnnovative.AOP.Sample.Testing.Enums;
using UnityEngine.Assertions;

namespace ITnnovative.AOP.Sample.Testing.Objects
{
    public class ReturnValueTests : TestScript
    {
        public static void OverrideReturnValueEnterTest()
        {
            Assert.IsTrue(_OverrideReturnValueEnterTest());
        }
 
        public static void OverrideReturnValueExitTest()
        {
            Assert.IsTrue(_OverrideReturnValueExitTest());
        }
        
        [TestAspectOverrideReturnValue(ExecuteIn.Enter)]
        public static bool _OverrideReturnValueEnterTest()
        {
            return false;
        }
     
        [TestAspectOverrideReturnValue(ExecuteIn.Exit)]
        public static bool _OverrideReturnValueExitTest()
        {
            return false;
        }

        public static void Run()
        {
            new ReturnValueTests().RunTests();
        }

        public override void RunTests()
        {
            OverrideReturnValueEnterTest();
            OverrideReturnValueExitTest();
        }
    }
}