using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Property
{
    public interface IPropertyGetExitAspect : IPropertyGetAspect
    {
        void OnPropertyGetExit(PropertyExecutionArguments args);
    }
}