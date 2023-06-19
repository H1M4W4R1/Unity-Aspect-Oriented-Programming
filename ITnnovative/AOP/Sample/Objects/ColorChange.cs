using System;
using ITnnovative.AOP.Sample.Aspects;
using ITnnovative.AOP.Sample.Aspects.Validation;
using UnityEngine;

namespace ITnnovative.AOP.Sample.Objects 
{
    public class ColorChange : MonoBehaviour 
    {
        private int _counter = 0; 
        
        [UpperCaseOnly] 
        private string Property { get;  set; }

        [ContextMenu("EAI")]
        [ColorChangeAspect(1, 0, 0, 1)]
        public int ExampleAspectInt()     
        { 
            return 32;
        }
 
        [ContextMenu("EAO")]
        [ColorChangeAspect(1, 0, 0, 1)]
        public object ExampleAspectObject()
        {
            return new object();
        }
                   
        public void OnMouseDown()  
        { 
            if (_counter == 0)
            {  
                SetRed();
                _counter++;
            }
            else  
            {
                SetWhite();  
                _counter--;
            }  
        } 

        [ColorChangeAspect(1, 0, 0, 1)]
        public void SetRed()   
        {     
        }        

        [ColorChangeAspect(1, 1, 1, 1)]
        public void SetWhite(){}  

        [ColorChangeAspect(0, 1, 0, 1)]
        public void SetColor(int r, int g, int b, int a)   
        {
            // Just for testing :D
        }

        [ContextMenu("A")] 
        public void TextA()
        {
            Property = "HELLO OK";
        }

        [ContextMenu("B")]
        public void TextB()
        {
            Property = "HELLO Ok";
        }
    }
}
