using System;

namespace ITnnovative.AOP.Processing.Execution.Arguments
{
    [Flags]
    public enum ExceptionSource
    {
        None = 0x0,
        Method = 0x1, // Do not change! May cause issues if not 1.
        Aspect = 0x2,
        Any = Method | Aspect
    }
}