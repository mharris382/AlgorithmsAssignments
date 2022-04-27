using UnityEngine;

namespace Assignment3
{
    public class PrimsAlgorithmTest : MonoBehaviour
    {
        public string[] filePaths = new[]
        {
            "Scripts/Assignment3/TestData/Prim_000-1.gl",
            "Scripts/Assignment3/TestData/Prim_001.gl",
            "Scripts/Assignment3/TestData/Prim_002.gl"
        };

        string GetFilePath(int index) => $"{Application.dataPath}/{filePaths[index]}";


        [ContextMenu("Test Prim\'s Algorithm")]
        public void TestAll()
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                TestPrimsAlgorithm(GetFilePath(i));
            }
        }

        public void TestPrimsAlgorithm(string filePath)
        {
            Debug.Log("<i>Starting Prims Test............</i>");
            var primsGraph = new PrimsGraph(filePath);

            if (primsGraph.Solve(out var minimumSpanningTree))
            {
                Debug.Log($"<color=green>Successfully Solved!\n{filePath}\n</color>\n{minimumSpanningTree.ToString()}");
            }
            else
            {
                Debug.Log($"<color=red>Failed to Solve!\n<b>{filePath}!</b></color>");
            }
        }
    }
}