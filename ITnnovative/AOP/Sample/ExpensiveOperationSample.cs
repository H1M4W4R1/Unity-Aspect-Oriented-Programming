using System;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ITnnovative.AOP.Sample
{
    
    public class ExpensiveOperationSample : MonoBehaviour
    {

        [SampleEventAspect] public event Action<int, string> testEvent = (a, b) => Debug.Log(b);

        
        public string RandomNumber()
        {
            return Random.Range(0, 100).ToString();
        }
        
        [SampleMethodAspect] 
        [ContextMenu("Test")]
        public void Test()
        {  
            testEvent += (i ,w) => Debug.Log(i * 2);
            testEvent(10, RandomNumber()); 
            //Debug.Log("Hello world!");
            //Thread.Sleep(1000);
        }

        [SampleExceptionTest]
        [ContextMenu("Test Exception")]
        public void Test2()
        { 
            Property = 128;   
            Debug.Log(Property); 

            Property2 = new Vector3(5, 5, 5);
            
            throw new NotImplementedException();
        }

        [SamplePropertyAspect] public Vector3 Property2 { get; set; } = Vector3.one;

        [SamplePropertyAspect] public int Property { get; set; } = 10; 
    }
}