using System;
using System.Collections.Generic;
using System.Linq;
using Assignment2;
using UnityEngine;

class PathTree<T> where T : class
{
    private Graph<T> graph;
    private TreeNode root;
    private T StartNode => root.Value;


    private List<TreeNode> leafNodes = new List<TreeNode>();
    private readonly Action<T> callback;

    

    public PathTree(T startNode, Graph<T> graph, Action<T> onLeafFoundCallback=null, Action<List<TreeNode>> onAllPathsExhausted=null)
    {
        if (startNode == null) throw new NullReferenceException("Start Node cannot be null!");
        this.callback = onLeafFoundCallback;
        this.graph = graph;
        root = new TreeNode(startNode, graph, OnLeafFound);
        onAllPathsExhausted?.Invoke(leafNodes);
    }


    void OnLeafFound(TreeNode leafNode)
    {
        leafNodes.Add(leafNode);
        callback?.Invoke(leafNode.Value);
        Debug.Log($"Found Path: {leafNode.Key}\nCost: {leafNode.Cost}");
    }
               
        
     public class TreeNode
    {
        private string key;
            
        private TreeNode parent;
        private List<TreeNode> children;
            
        private T value;
        private float pathCost;
            
        private HashSet<T> unvisited;

        public string Key => key;
        public float Cost => pathCost;
        public List<TreeNode> Children => children;
        public T Value => value;
        public TreeNode Parent => parent;
            
        public TreeNode(T value, Graph<T> graph, Action<TreeNode> foundLeaf)
        {
            parent = null;
            this.value = value;
            this.key = this.value.ToString();
            this.unvisited = new HashSet<T>();
            children = new List<TreeNode>();
            this.pathCost = 0;

            Debug.Log($"Starting from node {value}");
            foreach (var vert in graph.GetVertices())
            {
                if (vert.ToString() != this.value.ToString())
                {
                    Debug.Log($"Adding Unvisited Node {vert}");
                    this.unvisited.Add(vert);
                }
            }

            foreach (var edge in graph.GetEdges(value))
            {
                Debug.Log($"Found path from {value} to {edge}!");
                children.Add(new TreeNode(edge, this, graph, foundLeaf));
            }
        }

        public TreeNode(T value, TreeNode parent, Graph<T> graph, Action<TreeNode> foundLeaf)
        {
            this.parent = parent;
            this.value = value;
            this.key = $"{this.parent.key},{this.value.ToString()}";
                
            unvisited = new HashSet<T>();
            children = new List<TreeNode>();
                
            this.pathCost = parent.pathCost + graph.GetEdgeWeight(parent.value, value);


            foreach (var e in parent.unvisited)
            {
                if (!e.Equals(value))
                    this.unvisited.Add(e);
            }

            var options = from n in graph.GetEdges(value) where unvisited.Contains(n) select n;
            if (!options.IsEmpty())
            {
                foreach (var option in options) 
                    children.Add(new TreeNode(option, this, graph, foundLeaf));
            }
            else
            {
                foundLeaf(this);
            }
        }
    }
}