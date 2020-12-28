using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Property
{
    public interface IPropertySetExitAspect : IPropertySetAspect
    {
        void OnPropertySetExit(PropertyExecutionArguments args);
    }
}