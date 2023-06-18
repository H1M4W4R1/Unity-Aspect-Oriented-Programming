using System;
using System.Collections.Generic;
using System.Threading;
using ITnnovative.AOP.Sample.Aspects;
using UnityEngine;

namespace ITnnovative.AOP.Sample.Objects
{
    /// <summary> 
    /// This class is a custom class that while object is created calculates time of Start method.
    /// For this purpose it uses <see cref="MethodTimeLogger"/> aspect.
    /// </summary>
    public class MethodTimeLoggerMonoBehaviour : MonoBehaviour
    {

        [MethodTimeLogger]
        public void Start()
        {
            Debug.Log("Hello there!"); 
            Thread.Sleep(10);
            TestMethod();  
        }

        public void OnMouseDown()
        {
            TestMethod();
        }

        [MethodTimeLogger]
        [ContextMenu("TestMethod")]
        public void TestMethod()
        {
            Debug.Log("Sleeping for 50ms"); 
            Thread.Sleep(50);
            TestMethod(100);
        } 


        [MethodTimeLogger]
        public void TestMethod<T>(T sleepTime)
        {
            Debug.Log("Sleeping for: " + sleepTime); 
            if (sleepTime is int)
            {
                Thread.Sleep(Convert.ToInt32(sleepTime));
            }
        }
    }
}
