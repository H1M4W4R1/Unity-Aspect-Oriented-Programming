using System;
using ITnnovative.AOP.Sample.Aspects;
using UnityEngine;

namespace ITnnovative.AOP.Sample.Objects
{
    public class ColorChange : MonoBehaviour
    {
        private int _counter = 0;

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
        public void SetRed(){} 

        [ColorChangeAspect(1, 1, 1, 1)]
        public void SetWhite(){}

        [ColorChangeAspect(1, 1, 1, 1)]
        public void SetColor(ref int r, ref int g, ref int b, ref int a)   
        {
            // Just for testing :D
        }
        
        [ColorChangeAspect(1, 1, 1, 1)]
        public void SetColor(out int r, out int g, out int b, out int a, bool q)  
        {
            // Just for testing :D
            q = true;
            r = g = b = a = 2;
        }
    }
}
