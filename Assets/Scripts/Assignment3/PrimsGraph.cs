using System;
using System.Linq;
using Assignment1;
using Assignment2;
using UnityEngine;

public class PrimsGraph
{
    private Graph<PrimVertex> _originalGraph;
    private Graph<PrimVertex> _minimumSpanningTree;
    private PriorityQueue<PrimVertex> _queue;
    private PrimVertex _root;
    public PrimsGraph(string filePath)
    {
        var parser = new GraphParser(filePath);
        var graphType = parser.GetGraphType();
        _minimumSpanningTree = new Graph<PrimVertex>(graphType);
        _originalGraph = parser.ParseGraph<PrimVertex>(ParseStringToVertex);
        
        
        
        foreach (var vertex in _originalGraph.GetVertices()) 
            _minimumSpanningTree.AddVertex(vertex);
        
        foreach (var vertex in _minimumSpanningTree.GetVertices()) 
            Debug.Assert(_originalGraph.ContainsVertex(vertex));
        
        _queue = new PriorityQueue<PrimVertex>();
        _queue.InitCapacity(100);
        
        try
        {
            _root = _minimumSpanningTree.GetVertices().First(t => t.IsRoot);
        }
        catch (Exception e)
        {
            Debug.LogError($"No Root found in Prim Graph .gl file\n{filePath}");
            throw;
        }
    }

    public void Solve(out Graph<PrimVertex> minimumSpanningTree)
    {
        minimumSpanningTree = _minimumSpanningTree;
        _root.Cost = 0;
        
    }

    private PrimVertex ParseStringToVertex(string str)
    {
        return new PrimVertex()
        {
            Value = str,
            Cost = float.PositiveInfinity,
            Parent = null
        };
    }
}