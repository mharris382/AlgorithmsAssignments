using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assignment2;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;
using Object = System.Object;

// ReSharper disable PossibleMultipleEnumeration

public class TravelingSalesman : MonoBehaviour
{
    private const string Path_T11 ="Scripts/Assignment5/Test Data/t11.gl";
    private const string Path_T4 = "Scripts/Assignment5/Test Data/t4.gl";
    private const string Path_T5 = "Scripts/Assignment5/Test Data/t5.gl";
    private const string Path_T6 = "Scripts/Assignment5/Test Data/t6.gl";
    private const string Path_T7 = "Scripts/Assignment5/Test Data/t7.gl";

    public static string GetTestData(int id)
    {
        string localPath;
        switch (id)
        {
            case 4:
                localPath = Path_T4;
                break;
            case 5:
                localPath = Path_T5;
                break;
            case 6:
                localPath = Path_T6;
                break;
            case 7:
                localPath = Path_T7;
                break;
            case 11:
                localPath = Path_T11;
                break;
            default:
                throw new Exception($"No id for that traveller! {id}");
        }
        return $"{Application.dataPath}/{localPath}";
    }
    
    public static string[] AllTests()
    {
        return new[] {GetTestData(11),GetTestData(4),GetTestData(5), GetTestData(6),GetTestData(7)};
    }
    

    public static MinimumPath<T> Solve<T>(Graph<T> graph, T startNode)
    {
        var path = new List<T>();
        var cost = 0.0f;


        return new MinimumPath<T>()
        {
            totalCost = cost,
            startNode = startNode,
            path = path
        };
    }

    public static void TSP<T>(Graph<T> graph, T startNode, TSPSolutionFound<T> solutionFound, bool debug = true) where T: class
    {

        var paths = new PathTree<T>(startNode, graph, onAllPathsExhausted: OnFinished);


        void OnFinished(List<PathTree<T>.TreeNode> allPaths)
        {
            if (allPaths == null || allPaths.Count == 0)
            {
                Debug.LogError("Did not find any paths!");
                return;
            }
            if(debug) Debug.Log($"Found <b>{allPaths.Count}</b> unique paths!");
            Output(allPaths);
            float lowestCost = float.MaxValue;
            PathTree<T>.TreeNode bestPath = null;
            foreach (var path in allPaths)
            {
                var cost = path.Cost;
                cost += graph.GetEdgeWeight(path.Value, startNode);
                if (cost < lowestCost)
                {
                    bestPath = path;
                    lowestCost = cost;
                }
            }

            LinkedList<T> solutionPath = new LinkedList<T>();
            var next = solutionPath.AddLast(startNode);
            var current = bestPath;
            string p = "";
            while (current != null)
            {
                p = $"<i>{current.Value}</i>\n<i>{p}</i>";
                next = solutionPath.AddBefore(next, current.Value);
                current = current.Parent;
            }
            Debug.Log($"<b>Best Cost: {lowestCost}</b>\n<b>Best Path: </b>\n{p}");
            solutionFound.Invoke(solutionPath.ToList(), lowestCost);
        }
        
        void Output( List<PathTree<T>.TreeNode> allPaths)
        {
            if (debug == false) return;
            var localPath = "Scripts/Assignment5/Output.txt";
            var fullPath = $"{Application.dataPath}/{localPath}";
            if (File.Exists(fullPath))
            {
                var fs = File.Open(fullPath, FileMode.Create);
                var writer = new StreamWriter(fs);
                
                for (int i = 0; i < allPaths.Count; i++)
                {
                    var leafNode = allPaths[i];
                    writer.WriteLine($"{leafNode.Cost + graph.GetEdgeWeight(leafNode.Value, startNode)}\t\t{leafNode.Key},{startNode}");
                }
                writer.Close();
                fs.Close();
                Debug.Log("Finished");
            }
        }
    }

    


    public class InvalidGraphException : Exception
    {
        public InvalidGraphException(string msg) : base(msg)
        {
        }
    }



}

public delegate void TSPSolutionFound<T>(List<T> solutionPath, float solutionCost);

static class EnumerableExtensions
{
    public static bool IsEmpty<T>(this IEnumerable<T> set)
    {
        try
        {
            var first = set.First();//throws exception if 
            return false;
        }
        catch (Exception e)
        {
            return true;
        }
    } 
}
public struct MinimumPath<T>
{
    public List<T> path;
    public T startNode;
    public float totalCost;
}