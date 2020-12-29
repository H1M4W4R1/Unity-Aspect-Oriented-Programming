﻿using System;
using System.Collections.Generic;
using System.Threading;
using ITnnovative.AOP.Processing.Exectution;
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
            Thread.Sleep(10);
            TestMethod();
        }

        [MethodTimeLogger]
        public void TestMethod()
        {
            Thread.Sleep(50);
            TestMethod(100);
        }

        [MethodTimeLogger]
        public void TestMethod(int sleepTime)
        {
            Thread.Sleep(sleepTime);
        }
    }
}