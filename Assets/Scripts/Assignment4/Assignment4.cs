using UnityEngine;

namespace Assignment4
{
    public class Assignment4 : MonoBehaviour
    {
        [ContextMenu("Test/Triangle")]
        void RunTriangleTest()
        {
            Quickhull.TestTriangle();
        }

        
        [ContextMenu("Test/Points")]
        void RunPointTests()
        {
            Quickhull.TestPoints();
        }
    }
}