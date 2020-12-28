using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Property
{
    public interface IPropertySetEnterAspect : IPropertySetAspect
    {
        void OnPropertySetEnter(PropertyExecutionArguments args);
    }
}