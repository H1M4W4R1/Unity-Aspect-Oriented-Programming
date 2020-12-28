using ITnnovative.AOP.Processing.Exectution.Arguments;

namespace ITnnovative.AOP.Attributes.Property
{
    public interface IPropertyGetEnterAspect : IPropertyGetAspect
    {

        void OnPropertyGetEnter(PropertyExecutionArguments args);

    }
}