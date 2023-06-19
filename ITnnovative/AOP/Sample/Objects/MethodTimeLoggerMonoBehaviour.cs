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

        [MethodTimeLogger("Start")]
        public void Start() 
        {
            Thread.Sleep(10);
            TestMethod();
        }

        public void OnMouseDown()
        {
            TestMethod();
        }

        [MethodTimeLogger("TestMethod")]
        [ContextMenu("TestMethod")]
        public void TestMethod()
        {
            Thread.Sleep(50);
        }
    }
}