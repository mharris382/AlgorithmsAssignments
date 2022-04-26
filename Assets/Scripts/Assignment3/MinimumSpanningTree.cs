using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MinimumSpanningTree 
{
    
}


public static class TestData
{
    public static string[] GetTestFiles()
    {
        var path = $"{Application.dataPath}/Scripts/Assignment3/TestData/";
        var files = new string[]
        {
            $"{path}/Prim_000-1.gl",
            $"{path}/Prim_001.gl",
            $"{path}/Prim_002.gl"
        };
        
        foreach (var file in files)
            Debug.Assert(File.Exists(file), $"File does not exist({file})");
        
        return files;
    }
}

public class PrimVertex : Vertex<string> { }

public class Vertex<T>
{
    public T Value { get; set; }
    public float Cost { get; set; }
    public Vertex<T> Parent { get; set; }
}