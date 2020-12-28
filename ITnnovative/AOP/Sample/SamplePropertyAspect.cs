using System;
using ITnnovative.AOP.Attributes.Property;
using ITnnovative.AOP.Processing.Exectution.Arguments;
using UnityEngine;

namespace ITnnovative.AOP.Sample
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SamplePropertyAspect : Attribute, IPropertyGetEnterAspect, IPropertyGetExitAspect,
        IPropertySetEnterAspect, IPropertySetExitAspect 
    {
        public void OnPropertyGetEnter(PropertyExecutionArguments args)
        {
            Debug.Log("Getting property Value!");
        }

        public void OnPropertyGetExit(PropertyExecutionArguments args)
        {
            Debug.Log("GOT PROPERTY VALUE!");
        }

        public void OnPropertySetEnter(PropertyExecutionArguments args)
        {
            Debug.Log("Property SET BEGAN!");
            Debug.Log(args.newValue);
        }

        public void OnPropertySetExit(PropertyExecutionArguments args)
        {
            Debug.Log("Property SET!");
        }
    }
}