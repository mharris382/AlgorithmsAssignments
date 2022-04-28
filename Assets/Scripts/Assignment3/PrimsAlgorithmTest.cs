using System;
using System.Collections;
using Assignment1;
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
        public TreeVisualizer Visualizer;

        private void Awake()
        {
            if(testProcess==false)
                TestVisualizer();
        }

        private void Start()
        {
            if (testProcess)
                StartCoroutine(DoSteps());
        }

        public bool testProcess;

        public void TestVisualizer()
        {
            var filePath = GetFilePath(0);
            var primsGraph = new PrimsGraph(filePath);
            if (Visualizer != null)
            {
               var queue = new PriorityQueue<PrimVertex>();
               queue.InitCapacity(1000);
               foreach (var verts in primsGraph.GetOriginal().GetVertices())
               {
                   queue.Enqueue(verts, verts.IsRoot ? 0 : float.PositiveInfinity);
               }
               Visualizer.BuildTree(queue.GetInternalHeap());
            }
        }

        public void TestSteps()
        {
            StartCoroutine(DoSteps());
        }

        public float delay = 4;
        public float timeBetweenSteps = 1;
        IEnumerator DoSteps()
        {
            var filePath = GetFilePath(0);
            var primsGraph = new PrimsGraph(filePath);
            if (Visualizer == null) yield break;
            
            var queue = new PriorityQueue<PrimVertex>();
            queue.InitCapacity(1000);
            foreach (var verts in primsGraph.GetOriginal().GetVertices())
            {
                queue.Enqueue(verts, verts.IsRoot ? 0 : float.PositiveInfinity);
            }
            Visualizer.BuildTree(queue.GetInternalHeap());
            Debug.Log("Start Delay");
            yield return new WaitForSeconds(delay);
            Debug.Log("Starting");
            int step = 0;
            while (queue.Count > 0)
            {
                yield return new WaitForSeconds(timeBetweenSteps);
                Debug.Log($"Step {step++}");
                queue.ExtractMin();
                Debug.Log(queue.Count);
                Debug.Log(queue.GetInternalHeap().Count);
                Visualizer.BuildTree(queue.GetInternalHeap());
            }
        }

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
            Debug.Log($"<i>Starting Prims Test............</i>{filePath}");
            var primsGraph = new PrimsGraph(filePath);

            if (primsGraph.Solve(out var minimumSpanningTree))
            {
                Debug.Log($"<color=green>Successfully Solved!</color>\n{minimumSpanningTree.ToString()}");
                
            }
            else
            {
                Debug.Log($"<color=red>Failed to Solve!\n<b>{filePath}!</b></color>");
            }
        }
    }
}