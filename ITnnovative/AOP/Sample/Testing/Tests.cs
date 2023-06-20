using ITnnovative.AOP.Sample.Testing.Objects;
using UnityEngine;

namespace ITnnovative.AOP.Sample.Testing
{
    public class Tests : MonoBehaviour
    {

        [ContextMenu("Run Tests")]
        public void RunTests()
        {
            ReturnValueTests.Run();
        }
        
    }
}