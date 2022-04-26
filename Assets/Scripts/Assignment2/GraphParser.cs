using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Assignment2
{
    public class GraphParser
    {
        private readonly string[] _lines;
        public readonly bool IsDirected;
        public readonly bool IsWeighted;
        
        private Func<string, object> _parseObjectFunction;
        Dictionary<string, object> _createdObjectCache = new Dictionary<string, object>();
        public readonly GraphType graphType;

        public GraphParser(string filePath)
        {
            
            if (!File.Exists(filePath))
            {
                Debug.LogError($"Invalid File: {filePath}");
                throw new FileNotFoundException(filePath);
            }

            var extension = Path.GetExtension(filePath);
            if (extension != ".gl")
            {
                Debug.LogError($"can only parse .gl files, given file was <b>{extension}</b> file");
                throw new FormatException($"can only parse .gl files, given file was <b>{extension}</b> file");
            }

            var fs = File.Open(filePath, FileMode.Open);
            var reader = new StreamReader(fs);
            int cnt = 0;
            List<string> lines = new List<string>();
           
            
            while (!reader.EndOfStream)
            {
                if (cnt == 0)
                {
                    header = reader.ReadLine();
                    if (string.IsNullOrEmpty(header))
                    {
                        throw new FormatException($"Invalid Header in file {filePath}");
                    }
                    var graphType = header.Split(' ');
                    if (graphType.Length != 2)
                    {
                        throw new FormatException("Header is formatted incorrectly");
                    }
                    
                    IsDirected = graphType[0] == "directed";
                    IsWeighted = graphType[1] == "weighted";
                    this.graphType = (GraphType)((IsDirected ? (int)GraphType.Directed : 0) + (IsWeighted ? (int)GraphType.Weighted : 0));
                  //  Debug.Log("Parsed: " + this.graphType.ToString());
                }
                else
                {
                    lines.Add(reader.ReadLine());
                }
                cnt++;
            }
            
            _lines = lines.ToArray();
            Debug.Assert(_lines.Length > 0, $"Didn't read any lines in file {filePath}!");
            reader.Close();
            fs.Close();
        }
        public GraphParser(string[] lines)
        {
            if (lines.Length < 1)
            {
                return;
            }

            this.header = lines[0];
            var info = header.Split(' ');
            IsDirected = info[0] == "directed";
            IsWeighted = info[1] == "weighted";
            _lines = new string[lines.Length - 1];
            for (int i = 1; i < lines.Length; i++)
            {
                _lines[i - 1] = lines[i];
            }
        }

        public string header { get; set; }

        public GraphType GetGraphType()
        {
            int d = IsDirected ? 1 : 0;
            int w = IsWeighted ? 1 : 0;
            return (GraphType) ((d << 1) | (w));
        }

        public Graph<string> ParseGraph()
        {
            List<(string from, string to)> unweightedEdges = new List<(string From, string to)>();
            List<(string from, string to, float weight)> weightedEdges = new List<(string From, string to, float weight)>();
            HashSet<string> nodesFound = new HashSet<string>();
            
            var graph = new Graph<string>(GetGraphType());
            ParseNodesAndEdges();
            AddVerts();
            AddEdges();
            //Debug.Log(graph.ToString());
            return graph;
            
            void ParseNodesAndEdges()
            {
                foreach (var line in _lines)
                {
                    if (IsWeighted)
                    {
                        var ew = ParseEdgeWeighted(line);
                        VerifyHasNode(ew.from, ew.to);
                        weightedEdges.Add(ew);
                    }
                    else
                    {
                        var eu = ParseEdgeUnweighted(line);
                        VerifyHasNode(eu.from, eu.to);
                        unweightedEdges.Add(eu);
                    }
                }
            }
            void VerifyHasNode(string n1, string n2)
            {
                if (!nodesFound.Contains(n1)) nodesFound.Add(n1);
                if (!nodesFound.Contains(n2)) nodesFound.Add(n2);
            }
            void AddVerts()
            {
                foreach (var vert in nodesFound)
                {
                    graph.AddVertex(vert);
                }
            }
            void AddEdges()
            {
                if (IsWeighted)
                {
                    Debug.Assert(weightedEdges.Count != 0); 
                    
                    foreach (var weightedEdge in weightedEdges)
                    {
                        graph.AddEdge(weightedEdge.from, weightedEdge.to, weightedEdge.weight);
                    }
                }
                else
                {
                    Debug.Assert(unweightedEdges.Count != 0);
                    
                    foreach (var unweightedEdge in unweightedEdges)
                    {
                        graph.AddEdge(unweightedEdge.from, unweightedEdge.to);
                    }
                }
            }

        }
        
   

        
        public Graph<T> ParseGraph<T>(Func<string, T> parseObjectFunction) 
        {
            _parseObjectFunction = (string str) => parseObjectFunction(str);
            List<(T from, T to)> unweightedEdges = new List<(T From, T to)>();
            List<(T from, T to, float weight)> weightedEdges = new List<(T From, T to, float weight)>();
            HashSet<T> nodesFound = new HashSet<T>();
            
            var graph = new Graph<T>(GetGraphType());
            ParseNodesAndEdges();
            AddVerts();
            AddEdges();
            //Debug.Log(graph.ToString());
            return graph;
            
      

            void ParseNodesAndEdges()
            {
                foreach (var line in _lines)
                {
                    if (IsWeighted)
                    {         
                        var ew = ParseObjectEdgeWeighted(line);
                        VerifyHasNodes(ew.from, ew.to);
                        weightedEdges.Add(ew);
                        
                        (T from, T to, float weight) ParseObjectEdgeWeighted(string line)
                        {
                            var strParsed = ParseEdgeWeighted(line);
                            var objFrom = GetNodeObject<T>(strParsed.from);
                            var objTo = GetNodeObject<T>(strParsed.to);
                            return (objFrom, objTo, strParsed.weight);
                        }
                    }
                    else
                    {
                        var eu = ParseObjectEdgeUnweighted(line);
                        VerifyHasNodes(eu.from, eu.to);
                        unweightedEdges.Add(eu);
                        
                        (T from, T to) ParseObjectEdgeUnweighted(string line)
                        {
                            var strParsed = ParseEdgeUnweighted(line);
                            var objFrom = GetNodeObject<T>(strParsed.from);
                            var objTo = GetNodeObject<T>(strParsed.to);
                            return (objFrom, objTo);
                        }
                    }
                }
            }
            void VerifyHasNodes(T n1, T n2)
            {   
                if (!nodesFound.Contains(n1)) nodesFound.Add(n1);
                if (!nodesFound.Contains(n2)) nodesFound.Add(n2);
            }
            void AddVerts()
            {
                foreach (var vert in nodesFound)
                {
                    graph.AddVertex(vert);
                }
            }
            void AddEdges()
            {
                if (IsWeighted)
                {
                    Debug.Assert(weightedEdges.Count != 0); 
                    
                    foreach (var weightedEdge in weightedEdges)
                    {
                        graph.AddEdge(weightedEdge.from, weightedEdge.to, weightedEdge.weight);
                    }
                }
                else
                {
                    Debug.Assert(unweightedEdges.Count != 0);
                    
                    foreach (var unweightedEdge in unweightedEdges)
                    {
                        graph.AddEdge(unweightedEdge.from, unweightedEdge.to);
                    }
                }
            }
        }
        
        
        (string from, string to) ParseEdgeUnweighted(string line)
        {
            var parts = line.Split('=');
            if (parts.Length != 2)
                throw new FormatException($"Expected unweighted edge to have 2 parts but found {parts.Length}!\nLine: {line}");
            var from = parts[0];
            var to = parts[1];
            return (from, to);
        }
        (string from, string to, float weight) ParseEdgeWeighted(string line)
        {
            var parts = line.Split('=');
            if (parts.Length != 3)
                throw new FormatException($"Expected weighted edge to have 3 parts but found {parts.Length}!\nLine: {line}");
            
            var from = parts[0];
            var to = parts[1];
            bool success = float.TryParse(parts[2], out float cost);
            if (!success)
            {
                throw new FormatException($"unable to parse float value from {parts[2]}");
            }
            return (from, to, cost);
        }
        object GetNodeObject(string str)
        {
            if (_parseObjectFunction == null)
            {
                throw new NullReferenceException("Cannot convert node to object from string without a parse object strategy assigned!");
            }

            if (_createdObjectCache.ContainsKey(str))
            {
                return (_createdObjectCache[str] = _createdObjectCache[str] ?? _parseObjectFunction(str));
            }
            _createdObjectCache.Add(str, _parseObjectFunction(str));
            return _createdObjectCache[str];
        }

        T GetNodeObject<T>(string str) => (T) GetNodeObject(str);
        
    }


  
    
    
    
    public static class TestFiles
    {
        public static string[] GetTestFilePaths()
        {
            if (Application.isEditor == false)
            {
                throw new UnityException("Cannot access test files from Builds");
            }
            var path = $"{Application.dataPath}/Scripts/Assignment2/Test Data/";
            var files = TestFileNames;
            for (int i = 0; i < files.Length; i++)
            {
                var filename = files[i];
                files[i] = $"{path}/{filename}";
                Debug.Log($"TestFile = {files[i]}");
            }
            return files;
        }

        private static string[] TestFileNames => new[]
        {
            "du.gl",
            "dw.gl",
            "uu.gl",
            "uw.gl"
        };

        public static Graph<string> ParseTestGraphDu()
        {
            var path = GetTestDuFileName();
            throw new NotImplementedException();
        }

        public static Graph<int> ParseTestGraphUu()
        {
            var path = GetTestUuFileName();
            throw new NotImplementedException();
        }

        public static Graph<char> ParseTestGraphUw()
        {
            var path = GetTestUwFileName();
            throw new NotImplementedException();
        }

        public static Graph<char> ParseTestGraphUDw()
        {
            var path = GetTestDwFileName();
            throw new NotImplementedException();
        }

        public static string GetTestDuFileName()
        {
            var path = $"{Application.dataPath}/Scripts/Assignment2/Test Data/";
            return $"{path}/du.gl";
        }
        public static string GetTestUuFileName()
        {
            var path = $"{Application.dataPath}/Scripts/Assignment2/Test Data/";
            return $"{path}/uu.gl";
        }
        public static string GetTestDwFileName()
        {
            var path = $"{Application.dataPath}/Scripts/Assignment2/Test Data/";
            return $"{path}/dw.gl";
        }
        public static string GetTestUwFileName()
        {
            var path = $"{Application.dataPath}/Scripts/Assignment2/Test Data/";
            return $"{path}/uw.gl";
        }
        
        public static class ParserTestData
        {
            public static Graph<string> GetExpectedDirectedUnweightedGraph()
            {
                var graph = new Graph<string>(GraphType.DirectedUnweighted);
                (string, string[])[] vertices = new []
                {
                   ("Alpha", new string[]{"Hotel", "Beta"}),
                   ("Beta", new string[]{"Charlie"}),
                   ("Charlie", new string[]{"Juliett"}),
                   ("Delta", new string[]{"Foxtrot","Golf"}),
                   ("Echo", new string[]{"Charlie", "Delta"}),
                   ("Foxtrot", new string[]{"Golf"}),
                   ("Golf", new string[]{"Juliett", "Alpha"}),
                   ("Hotel", new string[]{"Echo", "India"}),
                   ("India", new string[]{"Beta", "Golf"}),
                   ("Juliett", new string[]{"Golf"})
                };

                foreach (var tuple in vertices)
                {
                    graph.AddVertex(tuple.Item1);
                }

                foreach (var tuple in vertices)
                {
                    foreach (var edge in tuple.Item2)
                    {
                        graph.AddEdge(tuple.Item1,edge);
                    }
                }
                return graph;
            }
        }
        
        
        
    }
}