using System;
using UnityEngine;

namespace Assignment2
{
    
    [ExecuteAlways]
    public class GraphDrawer : MonoBehaviour
    
    {
        
        
        public string filePath = "C:/Users/Admin/Documents/Unity Projects/_Repos/AlgorithmsAssignments/Assets/Scripts/Assignment2/Test Data/du.gl";

        public string[] testFiles = new string[]
        {
            "C:/Users/Admin/Documents/Unity Projects/_Repos/AlgorithmsAssignments/Assets/Scripts/Assignment2/Test Data/du.gl",
            "C:/Users/Admin/Documents/Unity Projects/_Repos/AlgorithmsAssignments/Assets/Scripts/Assignment2/Test Data/dw.gl",
            "C:/Users/Admin/Documents/Unity Projects/_Repos/AlgorithmsAssignments/Assets/Scripts/Assignment2/Test Data/uu.gl",
            "C:/Users/Admin/Documents/Unity Projects/_Repos/AlgorithmsAssignments/Assets/Scripts/Assignment2/Test Data/uw.gl"
        };
        
        public NodeVisual node1;
        public NodeVisual node2;
        public EdgeVisual edge;

        private Exception exception;
        [ContextMenu("Test Graph Parser")]
        void TestParser()
        {
            exception = null;
            foreach (var file in testFiles)
            {
                TestParser(file);
            }
        }

        [ContextMenu("Test Graph Parser - Unsafe")]
        void TestParserUnsafe()
        {
            foreach (var file in testFiles)
            {
                TestParserUnsafe(file);
            }
        }
        
        void TestParserUnsafe(string filePath)
        {
            Debug.Log($"Testing New File............................\n<b>{filePath}</b>");
            var parser = new GraphParser(filePath);
            
            var graph = parser.ParseGraph();
            Debug.Log(graph.ToString());
            Debug.Log("<color=green><i>Finished Parsing File......................\n</i></color>");
        }
        
        void TestParser(string filePath)
        {
            string header = "";
            try
            {
                Debug.Log($"Testing New File............................\n<b>{filePath}</b>");
                var parser = new GraphParser(filePath);
                header = parser.header;
                var graph = parser.ParseGraph();
                Debug.Log(graph.ToString());
                Debug.Log("<color=green><i>Finished Parsing File......................\n</i></color>");
            }
            catch (Exception e)
            {
                exception = e;
                Debug.Log(header);
                Debug.LogError($" <color=red><b>{e.GetType().Name}</b> occured while parsing <i>{filePath}</i></color>");
                Debug.Log("<color=red><i>Failed to Parse File......................\n</i></color>");
            }
        }
        
        private void Update()
        {
            if (node1 == null || node2 == null || edge == null) return;
            
            edge.SetCost(1);
            edge.SetDrawMode(GraphType.DirectedWeighted);
            edge.SetTargets(node1, node2);
        }
        
    }
}