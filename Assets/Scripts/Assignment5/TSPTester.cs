using System.Collections.Generic;
using System.Text;
using Assignment2;
using UnityEngine;

namespace Assignment5
{
    public class TSPTester : MonoBehaviour
    {
        [SerializeField]
        private string testFileLocalPath = "Scripts/Assignment5/Test Data/t4.gl";

        [SerializeField, Multiline(20)]
        private string inputGraph;

        private string GetTestFilePath() => $"{Application.dataPath}/{testFileLocalPath}";



        [ContextMenu("Solve TSP")]
        public void RunTest()
        {
            var parser = new GraphParser(GetTestFilePath());
            var graph = parser.ParseGraph();
            inputGraph = graph.ToString();
            TravelingSalesman.TSP(graph, "A", SolutionFound );
        }

        private void SolutionFound(List<string> solutionPath, float solutionCost)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"<b>Best Cost: {solutionCost}</b>");
            sb.AppendLine("<b>Path:</b>");
            foreach (var node in solutionPath) 
                sb.AppendLine($"\t<i>{node}</i>");
        }
    }
}