using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace Assignment2
{
    public interface IGraphParserStrategy
    {
        
    }
    
    public class GraphParser
    {
        private readonly string[] _lines;
        public readonly bool IsDirected;
        public readonly bool IsWeighted;
        public GraphParser(string filePath)
        {
            if (File.Exists(filePath))
            {
                Debug.LogError($"Invalid File: {filePath}");
                throw new FileNotFoundException(filePath);
            }

            var extension = Path.GetExtension(filePath);
            if (extension != "gl")
            {
                Debug.LogError("can only parse .gl files");
                throw new FormatException("can only parse .gl files");
            }

            var fs = File.Open(filePath, FileMode.Open);
            var reader = new StreamReader(fs);
            int cnt = 0;
            List<string> lines = new List<string>();
            while (!reader.EndOfStream)
            {
                if (cnt == 0)
                {
                    var header = reader.ReadLine();
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
                }
                else
                {
                    lines.Add(reader.ReadLine());
                }
                cnt++;
            }
            
            _lines = lines.ToArray();
            reader.Close();
            fs.Close();
        }
        public GraphParser(string[] lines)
        {
            if (lines.Length < 1)
            {
                return;
            }

            var header = lines[0];
            var info = header.Split(' ');
            IsDirected = info[0] == "directed";
            IsWeighted = info[1] == "weighted";
            _lines = new string[lines.Length - 1];
            for (int i = 1; i < lines.Length; i++)
            {
                _lines[i - 1] = lines[i];
            }
        }
        public GraphType GetGraphType()
        {
            int d = IsDirected ? 1 : 0;
            int w = IsWeighted ? 1 : 0;
            return (GraphType) ((d << 1) | (w));
        }

        public Graph<string> ParseGraph()
        {
            var graph = new Graph<string>(GetGraphType());
            
            throw new NotImplementedException();
            return graph;
        }

        public Graph<T> ParseGraph<T>(Func<string, T> parseObjectFunction)
        {
            var graph = new Graph<T>(GetGraphType());
            
            throw new NotImplementedException();
            return graph;
        }
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