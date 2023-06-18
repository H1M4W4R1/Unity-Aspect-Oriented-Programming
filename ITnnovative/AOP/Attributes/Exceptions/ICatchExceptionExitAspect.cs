using ITnnovative.AOP.Processing.Execution;

namespace ITnnovative.AOP.Attributes.Exceptions
{
    public interface ICatchExceptionExitAspect
    {
        void ExceptionCatchEnded(AspectData arguments);
    }
}