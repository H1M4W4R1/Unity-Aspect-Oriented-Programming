using ITnnovative.AOP.Testing.Enums;
using ITnnovative.AOP.Testing.Framework.Attributes;
using UnityEngine.Assertions;

namespace ITnnovative.AOP.Testing.Objects
{
    public class ReturnValueTests
    {
        [Test]
        public static void OverrideReturnValueEnterTest()
        {
            Assert.IsTrue(_OverrideReturnValueEnterTest());
        }
 
        [Test]
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

    }
}