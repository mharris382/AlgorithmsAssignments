using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assignment2;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GraphParserTests
{

    void TestGraphParser<T>(Graph<T> expectedGraph, Graph<T> actualGraph)
    {
        var expectedVertices = expectedGraph.GetVertices().ToList();
        var actualVertices = actualGraph.GetVertices().ToList();
        Assert.AreEqual(expectedVertices.Count, actualVertices.Count);
        
        expectedVertices.Sort();
        actualVertices.Sort();
        for (int i = 0; i < expectedVertices.Count; i++)
        {
            Assert.AreEqual(expectedVertices[i], actualVertices[i]);
        }
        
        
        foreach (var expectedVertex in expectedVertices)
        {
            Assert.IsTrue(actualGraph.ContainsVertex(expectedVertex));
            
            var expectedEdges = expectedGraph.GetEdges(expectedVertex).ToArray();
            var actualEdges = actualGraph.GetEdges(expectedVertex).ToArray();
            Assert.AreEqual(expectedEdges.Length, actualEdges.Length);
            
            foreach (var expectedTo in expectedEdges)
            {
                Assert.IsTrue(actualGraph.HasEdge(expectedVertex, expectedTo));
            }
        }
    }
    
    [Test]
    public void TestGraphParserDirectedUnweighted()
    {
        var expectedGraph = TestFiles.ParserTestData.GetExpectedDirectedUnweightedGraph();
        var actualGraph = TestFiles.ParseTestGraphDu();
        
        Assert.IsTrue(expectedGraph.IsDirected);
        Assert.IsTrue(actualGraph.IsDirected);
        
        Assert.IsFalse(expectedGraph.IsWeighted);
        Assert.IsFalse(actualGraph.IsWeighted);
        
        TestGraphParser(expectedGraph, actualGraph);
    }

  
}
