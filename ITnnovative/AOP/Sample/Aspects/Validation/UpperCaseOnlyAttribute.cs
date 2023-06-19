using System;
using System.IO;
using System.Linq;
using ITnnovative.AOP.Attributes.Property;
using ITnnovative.AOP.Processing.Execution;
using ITnnovative.AOP.Processing.Execution.Arguments;
using UnityEngine;

namespace ITnnovative.AOP.Sample.Aspects.Validation
{
    public class UpperCaseOnlyAttribute : Attribute, IPropertySetEnterAspect
    {
        public void OnPropertySetEnter(AspectData args)
        {
            var text = args.GetArgument(0);
            if (!(text is string)) throw new InvalidOperationException("Validator is valid only on string property");
            var txt = (string) text;
            if (!txt.All(c => char.IsUpper(c) || char.IsDigit(c) || char.IsPunctuation(c) || char.IsSeparator(c)))
            {
                args.SetExceptionThrow(ExceptionSource.Aspect,
                    new InvalidDataException("This property should have only upper characters!"));
                return;
            }
        }
    }
}