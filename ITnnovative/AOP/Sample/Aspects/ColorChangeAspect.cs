using System;
using ITnnovative.AOP.Attributes.Method;
using ITnnovative.AOP.Processing.Execution.Arguments;
using UnityEngine;

namespace ITnnovative.AOP.Sample.Aspects
{
    /// <summary>
    /// Changes object color to red
    /// </summary>
    public class ColorChangeAspect : Attribute, IMethodEnterAspect
    {
        private Color _newColor;
        
        public void OnMethodEnter(MethodExecutionArguments args)
        {
            var go = (MonoBehaviour) args.source;
            go.GetComponent<Renderer>().material.color = _newColor;
        }

        public ColorChangeAspect(float r, float g, float b, float a = 0)
        {
            _newColor = new Color(r, g, b, a);
        }
    }
}
