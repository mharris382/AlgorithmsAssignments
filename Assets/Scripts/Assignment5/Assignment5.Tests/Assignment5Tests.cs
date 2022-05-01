using System;
using System.Collections.Generic;
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
     
        [Test]
        public void CorrectSolutionForGraph4()
        {
            var validationSet = ValidationSet.GetSet4();
            AssertSolutionFromValidatationSet(validationSet);
        }
        [Test]
        public void CorrectSolutionForGraph5()
        {
            var validationSet = ValidationSet.GetSet5();
            AssertSolutionFromValidatationSet(validationSet);
        }
        [Test]
        public void CorrectSolutionForGraph6()
        {
            var validationSet = ValidationSet.GetSet6();
            AssertSolutionFromValidatationSet(validationSet);
        }

        void AssertSolutionFromValidatationSet(ValidationSet set)
        {
            string filePath = TravelingSalesman.GetTestData(set.setID);
            GraphParser parser = new GraphParser(filePath);
            var graph = parser.ParseGraph();
            var solution = TravelingSalesman.Solve(graph, "A");

    
            var expectedStart = set.startNode;
            var foundStart = solution.startNode;
            Assert.AreEqual(expectedStart, foundStart, $"Expected Start:{expectedStart} != Actual Start:{foundStart}");
            
            var expectedPath = set.path;
            var foundPath = solution.path;
            int expectedPathSize = expectedPath.Count;
            int actualPathSize = foundPath.Count;
            Assert.AreEqual(expectedPathSize, actualPathSize, $"Expected Path Size:{actualPathSize} != Actual Path Size:{expectedPathSize}");

            string expectPathStr = "";
            string actualPathStr = "";
            for (int i = 0; i < actualPathSize; i++)
            {
                var actualNext = foundPath[i];
                var expectNext = expectedPath[i];
                if (string.IsNullOrEmpty(expectPathStr) == false) expectPathStr += ",";
                if (string.IsNullOrEmpty(actualPathStr) == false) actualPathStr += ",";
                expectPathStr += expectNext;
                actualPathStr += actualNext;
            }
            
            float expectedCost = set.totalCost;
            float foundCost = solution.totalCost;
            Assert.AreEqual(expectedCost, foundCost,$"Expected Start:{expectedCost} != Actual Start:{foundCost}");

            Assert.AreEqual(expectPathStr, actualPathStr, $"Paths don't match!\nExpected:{expectPathStr}\nActual:{actualPathStr}");
        }
    }


    public class ValidationSet
    {
        public int setID;
        public string startNode;
        public float totalCost;
        public List<string> path;

        public ValidationSet(int id, string startNode, float totalCost, params string[] path)
        {
            this.path = new List<string>(path);
            this.setID = id;
            switch (id)
            {
                case 4:
                case 5:
                case 6:
                case 7:
                case 11:
                    break;
                default:
                    throw new Exception($"No test data for set: {id}");
            }
        }


        public static ValidationSet GetSet4()
        {
            return new ValidationSet(4, "A", 40, "A", "B", "D", "C");
        }

        public static ValidationSet GetSet5()
        {
            return new ValidationSet(5, "A", 782, "A", "D", "C", "B", "E", "A");
        }

        public static ValidationSet GetSet6()
        {
            return new ValidationSet(6, "A", 726, "A", "C", "B", "F", "D", "E", "A");
        }
    }
}