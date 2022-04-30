using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelingSalesman : MonoBehaviour
{
    private const string Path_T11 ="Scripts/Assignment5/Test Data/t11.gl";
    private const string Path_T4 = "Scripts/Assignment5/Test Data/t4.gl";
    private const string Path_T5 = "Scripts/Assignment5/Test Data/t5.gl";
    private const string Path_T6 = "Scripts/Assignment5/Test Data/t6.gl";
    private const string Path_T7 = "Scripts/Assignment5/Test Data/t7.gl";

    private static string GetTestData(int id)
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
    
}