using ITnnovative.AOP.Processing.Execution;

namespace ITnnovative.AOP.Attributes.Exceptions
{
    public interface ICatchExceptionEnterAspect
    {
        void ExceptionCatchBegan(AspectData arguments);
    }
}