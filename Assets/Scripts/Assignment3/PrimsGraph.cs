using System;
using System.Collections.Generic;
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
    public Graph<PrimVertex> GetOriginal() => _originalGraph;
    public PrimsGraph(string filePath)
    {
        var parser = new GraphParser(filePath);
        var graphType = parser.GetGraphType();
        _minimumSpanningTree = new Graph<PrimVertex>(GraphType.DirectedWeighted);
        _originalGraph = parser.ParseGraph<PrimVertex>(ParseStringToVertex);
        _queue = new PriorityQueue<PrimVertex>();
        _queue.InitCapacity(100);
        
        try
        {
            _root = _originalGraph.GetVertices().First(t => t.IsRoot);
        }
        catch (Exception e)
        {
            Debug.LogError($"No Root found in Prim Graph .gl file\n{filePath}");
            throw;
        }
    }

    public bool Solve(out Graph<PrimVertex> mst)
    {
        PrimVertex root = null;
        mst = new Graph<PrimVertex>(GraphType.DirectedWeighted);
        foreach (var primVertex in _originalGraph.GetVertices())
        {
            if (primVertex.IsRoot) root = primVertex;
            primVertex.Cost = primVertex.IsRoot ? 0 : float.PositiveInfinity;
            _queue.Enqueue(primVertex, primVertex.Cost);
        }
        Debug.Assert(root != null, "Did not find a root node!");
        Debug.Assert(_queue.Count == _originalGraph.VertexCount, "Queue does not contain N vertices!");
        Debug.Assert(mst.VertexCount == 0, "MST is not empty!");
        

        IEnumerable<PrimVertex> GetAdjacent(PrimVertex vertex) => _originalGraph.GetEdges(vertex);
        float GetEdgeWeight(PrimVertex from, PrimVertex to) => _originalGraph.GetEdgeWeight(@from, to);

        int step = 0;
        while (_queue.Count > 0)
        {
            var minNode = _queue.ExtractMin();
            mst.AddVertex(minNode);
            if (minNode.Parent != null)
            {
                mst.AddEdge((PrimVertex)minNode.Parent, minNode,
                    _originalGraph.GetEdgeWeight((PrimVertex)minNode.Parent, minNode));
            }
            
            float computeCost = minNode.Cost;
            var n = minNode;
            while (n.Parent != null)
            {
                computeCost += n.Cost;
                n = (PrimVertex)n.Parent;
            }
            Debug.Log($"Step: {step++}\nMin Node = {minNode}\tCost = {computeCost}");
            
            foreach (var node in _originalGraph.GetEdges(minNode))
            {
                //Debug.Log($"Found edge: {minNode}->{node}");
                float edgeWeight = GetEdgeWeight(minNode, node);
                float attachCost = computeCost + edgeWeight;
  
                if (attachCost < node.Cost)
                {
                    node.Cost = attachCost;
                    _queue.UpdateKey(node, attachCost);
                    node.Parent = minNode;
                }
            }
        }
        
        Debug.Log($"Original: {_originalGraph}");
        Debug.Log($"MST: {mst}");
        return true;
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