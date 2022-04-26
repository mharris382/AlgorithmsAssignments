using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Graphs;
using UnityEngine;

namespace Assignment2
{
    public class ActualGraphDrawer : MonoBehaviour
    {
        public EdgeVisual edgePrefab;
        public NodeVisual nodePrefab;
        public float nodeSeparation = 2;
        public float angleOffset = 30;
        
        public string filePath = "C:/Users/Admin/Documents/Unity Projects/_Repos/AlgorithmsAssignments/Assets/Scripts/Assignment2/Test Data/du.gl";

        private Vector3 _nextDirection;

        private int _ringNumber;
        private float _prevAngle;
        private float _nextDistance;

        private Graph<NodeVisual> _graph;


        private Queue<EdgeVisual> _edges = new Queue<EdgeVisual>();
        private GraphType _type;
        private Queue<EdgeVisual> _unused = new Queue<EdgeVisual>();
        private DoubleKeyDictionary<NodeVisual, NodeVisual, EdgeVisual> _edgeLookups = new DoubleKeyDictionary<NodeVisual, NodeVisual, EdgeVisual>();
        
        private void Awake()
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError($"Couldn't find graph file at path {filePath}");
                
            }
            else
            {
                var parser = new GraphParser(filePath);
                _type = parser.GetGraphType();
                _graph = parser.ParseGraph<NodeVisual>(ParseFromString);
                var nodes = _graph.GetVertices();
                foreach (var from in nodes)
                {
                    var edges = _graph.GetEdges(from);
                    foreach (var to in edges)
                    {
                        var edgeVisual = GetNewEdgeVisual(from, to);
                        edgeVisual.gameObject.SetActive(true);
                        _edgeLookups.Add(from,to, edgeVisual);
                    }
                }
            }
        }

        void ClearEdgeVisuals()
        {
            while (_edges.TryDequeue(out var edge))
            {
                edge.ResetVisual();
                _unused.Enqueue(edge);
            }
        }

        EdgeVisual GetNewEdgeVisual(NodeVisual from, NodeVisual to)
        {
            if (!_unused.TryDequeue(out var edge))
            {
                var t = transform;
                edge = Instantiate(edgePrefab, t.position, t.rotation, t);
                edge.SetDrawMode(_type);
                edge.SetTargets(from, to);
            }
            _edges.Enqueue(edge);
            return edge;
        }
        
        NodeVisual ParseFromString(string str)
        {
            var transform1 = transform;
            var pos = transform1.position;
            
            if (_ringNumber > 0)
            {
                var angle = angleOffset + _prevAngle;
                if (angle > 360)
                {
                    angle = 0;
                    _ringNumber++;
                }
                var distance = _ringNumber * nodeSeparation;
                var offset = (Quaternion.Euler(0, 0, angle) * Vector3.right) * distance;
                pos += offset;
                _prevAngle = angle;
            }
            else
            {
                _ringNumber = 1;
            }
            var node = Instantiate(nodePrefab, pos, transform1.rotation, transform1);
            return node;
        }
    }
}