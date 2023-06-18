using ITnnovative.AOP.Processing.Execution.Arguments;

namespace ITnnovative.AOP.Attributes.Property
{
    public interface IPropertyGetExitAspect : IPropertyGetAspect
    {
        void OnPropertyGetExit(BaseExecutionArgs args);
    }
}