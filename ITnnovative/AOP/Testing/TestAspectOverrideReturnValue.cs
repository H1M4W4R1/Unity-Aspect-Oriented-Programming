using System;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Processing.Execution;
using ITnnovative.AOP.Testing.Enums;

namespace ITnnovative.AOP.Testing
{
    public class TestAspectOverrideReturnValue : Attribute, IMethodBoundaryAspect
    {
        private ExecuteIn _execIn;
        
        public void OnMethodEnter(AspectData args)
        {
            if (_execIn != ExecuteIn.Enter) return;
            InternalCode(args);
        }

        public TestAspectOverrideReturnValue(ExecuteIn exec)
        {
            _execIn = exec;
        }

        public void OnMethodExit(AspectData args)
        {
            if (_execIn != ExecuteIn.Exit) return;
            InternalCode(args);
        }

        private void InternalCode(AspectData args)
        {
            args.Return(true);
        }
    }
}