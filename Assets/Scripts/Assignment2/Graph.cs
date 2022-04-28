using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assignment2
{
    public class Graph<T>
    {
        private readonly GraphType _type;
        private Dictionary<T, Dictionary<T, float>> _edgeLookup;
        private Dictionary<T, HashSet<T>> _incomingEdges;
        public GraphType GraphType => _type;
        private int _vertexCount;

        public int VertexCount => _edgeLookup.Count;
        
        public bool IsDirected => (((int) _type) & 2) != 0;
        public  bool IsWeighted => ((int)_type & 1) != 0;
        
        public Graph(GraphType type)
        {
            _type = type;
            _vertexCount = 0;
            _edgeLookup = new Dictionary<T, Dictionary<T, float>>();
            _incomingEdges = new Dictionary<T,  HashSet<T>>();
        }

        public void AddVertex(T vertex)
        {
            if (ContainsVertex(vertex)) return;
            _edgeLookup.Add(vertex, new Dictionary<T, float>());
            _incomingEdges.Add(vertex, new HashSet<T>());
            _vertexCount++;
        }

        public void RemoveVertex(T vertex)
        {
            var incomingEdges = GetIncomingEdges(vertex).ToArray();
            foreach (var incomingEdge in incomingEdges)
            {
                _edgeLookup[incomingEdge].Remove(vertex);
            }
            _edgeLookup.Remove(vertex);
            
        }
        
        /// <summary>
        /// iterator of outgoing edges starting from the given vertex
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public IEnumerable<T> GetEdges(T vertex)
        {
            if (!ContainsVertex(vertex)) yield break;
            var edges = _edgeLookup[vertex];
            foreach (var edge in edges)
                yield return edge.Key;
        }

        /// <summary>
        ///  iterator of outgoing weighted edges starting from the given vertex
        /// </summary>
        /// <param name="vertex"></param>
        /// <returns></returns>
        public IEnumerable<(T dest, float weight)> GetWeightedEdges(T vertex)
        {
            if (!ContainsVertex(vertex)) yield break;
            var edges = _edgeLookup[vertex];
            
            foreach (var edge in edges)
                yield return (edge.Key, edge.Value);
        }


        public IEnumerable<T> GetVertices()
        {
            foreach (var kvp in _edgeLookup)
            {
                yield return kvp.Key;
            }
        }
        
        public IEnumerable<T> GetIncomingEdges(T vertex)
        {
            if (!ContainsVertex(vertex)) yield break;
            var edges = _incomingEdges[vertex];
            foreach (var edge in edges)
            {
                yield return edge;
            }
        }

        public IEnumerable<(T, float)> GetIncomingWeightedEdge(T toVertex)
        {
            if (!ContainsVertex(toVertex)) yield break;
            var edges = _incomingEdges[toVertex];
            foreach (var edge in edges)
            {
                var from = edge;
                Debug.Assert(_edgeLookup[from].ContainsKey(toVertex));
                yield return (from, _edgeLookup[from][toVertex]);
            }
        }
        public bool ContainsVertex(T vertex) => _edgeLookup.ContainsKey(vertex);

        public bool HasEdge(T from, T to)
        {
            return _edgeLookup[from].ContainsKey(to);
        }

        public bool TryGetEdgeWeight(T from, T to, out float weight)
        {
            if (!HasEdge(from, to))
            {
                weight = 0;
                return false;
            }

            weight = _edgeLookup[from][to];
            return true;
        }

        public float GetEdgeWeight(T from, T to)
        {
            if (!HasEdge(from, to))
                throw new KeyNotFoundException($"Edge from {from} to {to} does not exist");
            return _edgeLookup[from][to];
        }
                
        
        public void AddEdge(T from, T to, float weight = 0)
        {
            //if not weighted, don't accept weighted input values
            if (!IsWeighted) weight = 0;
            
            //if not directed, also add the inverse edge
            if (!IsDirected) AddUndirectedEdge(from, to, weight);

            //if edge doesn't exist add one
            if (!HasEdge(from, to)) 
            {
                _edgeLookup[from].Add(to, weight);
                
            }
            else //just update the weight
            {
                _edgeLookup[from][to] = weight;
            }
        }
        
        
        private void AddUndirectedEdge(T from, T to, float weight = 0)
        {
            if (!IsWeighted) weight = 0;
            if (!HasEdge(from, to))
            {
                _edgeLookup[from].Add(to, weight);
            }
            else //edge exists so update weight
            {
                _edgeLookup[from][to] = weight;
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            var verts = new List<T>(GetVertices());
            verts.Sort((t1, t2)=> String.Compare(t1.ToString(), t2.ToString(), StringComparison.Ordinal));
            
            AddHeader();
            foreach (var from in verts)
            {
                if (IsWeighted) AddLinesWeighted(from);
                else AddLinesUnweighted(from);
            }
            return sb.ToString();

            void AddHeader()
            {
                sb.Append(IsDirected ? "directed" : "undirected");
                sb.Append(' ');
                sb.Append(IsWeighted ? "weighted" : "unweighted");
                sb.Append('\n');
            }
            void AddLinesWeighted(T from)
            {
                foreach (var edge in GetWeightedEdges(from))
                {
                    sb.AppendLine($"{from.ToString()}={edge.dest.ToString()}={edge.weight:F2}");
                }
            }
            void AddLinesUnweighted(T from)
            {
                foreach (var edge in GetEdges(from))
                {
                    sb.AppendLine($"{from.ToString()}={edge.ToString()}");
                }
            }
        }
    }

    public enum GraphType
    {
        DirectedWeighted = 0b11, 
        DirectedUnweighted = 0b10,
        UndirectedUnweighted = 0b00,
        UndirectedWeighted = 0b01,
        Directed = 0b10,
        Weighted = 0b01
    }
}