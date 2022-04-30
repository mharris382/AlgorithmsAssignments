using System.IO;
using System.Linq;
using Assignment2;
using NUnit.Framework;

namespace Assignment5.Assignment5.Tests
{
    public class Assignment5Tests
    {

        [Test]
        public void AssertTestDataIsFound()
        {
            var alLTestPaths = TravelingSalesman.AllTests();
            foreach (var testPath in alLTestPaths) Assert.IsTrue(File.Exists(testPath), $"File.Exists({testPath})");
        }
        
        [Test]
        public void TestDataCanBeParsed()
        {
            var files = TravelingSalesman.AllTests();
            foreach (var filePath in files)
            {
                var graphParser = new GraphParser(filePath);
                var graph = graphParser.ParseGraph();
                
                Assert.IsNotNull(graph);
                Assert.AreNotEqual(0 , graph.VertexCount, $"Graph has no vertices. \nPath={filePath}");
                foreach (var vertex in graph.GetVertices()) 
                    Assert.IsNotEmpty(graph.GetEdges(vertex), $"Vertex {vertex} has no edges!\nPath={filePath}");
            }
        }
    }
}