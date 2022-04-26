using System.Collections;
using System.Collections.Generic;
using Assignment2;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GraphTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void GraphTypesAreCorrect()
    {
        var dw = new Graph<char>(GraphType.DirectedWeighted);
        var graph = dw;
        Assert.IsTrue(graph.IsDirected);
        Assert.IsTrue(graph.IsWeighted);
        
        var du = new Graph<char>(GraphType.DirectedUnweighted);
        graph = du;
        Assert.IsTrue(graph.IsDirected);
        Assert.IsFalse(graph.IsWeighted);
        
        var uu = new Graph<char>(GraphType.UndirectedUnweighted);
        graph = uu;
        Assert.IsFalse(graph.IsDirected);
        Assert.IsFalse(graph.IsWeighted);
        
        var uw = new Graph<char>(GraphType.UndirectedWeighted);
        graph = uw;
        Assert.IsFalse(graph.IsDirected);
        Assert.IsTrue(graph.IsWeighted);
    }

    
}
