using System;
using System.Collections;
using System.Collections.Generic;
using Assignment4;
using UnityEngine;

public class HullTester : MonoBehaviour
{

    public GameObject hullPointPrefab;
    public Transform testParent;
    public int numAreas = 1;
    public int pointsPerArea = 25;
    public float radius = 40;
    public Vector2 spreadRange = new Vector2(4, 10);

    private Vector3[] testPoints;
    [SerializeField]
    private TestVisualizer visualizer;

    public Vector2 angleRange = new Vector2(-15, 15);
    public bool negateAngleOnOdd;
    
    [System.Serializable]
    public class TestVisualizer
    {
        public GameObject pointPrefab;
        public GameObject hullPointPrefab;
        [NonSerialized]
        public bool ready = false;
        private Dictionary<Vector3, GameObject> _points = new Dictionary<Vector3, GameObject>();
        public void VisualizePoints(Transform parent, Vector3[] points)
        {
            if (_points.Count > 0) 
                Reset();
            
            ready = true;
            for (int i = 0; i < points.Length; i++)
            {
                var pos = points[i];
                if (!_points.ContainsKey(pos))
                {
                    _points.Add(pos, Instantiate(pointPrefab, pos, Quaternion.identity, parent));
                    _points[pos].hideFlags = HideFlags.DontSave;
                }
            }
        }

        public void Solve(Transform parent)
        {
            if (!ready)
            {
                Debug.LogError("Not ready to solve yet");
                return;
            }
            var points = new List<Vector3>();
            foreach (var point in _points)
            {
                points.Add(point.Key);
            }

            var hull = Quickhull.FindConvexHull(points.ToArray());
            var result = hull.GetHullPoints();
            foreach (var point in result)
            {
                if (Application.isEditor && !Application.isPlaying)
                {
                    DestroyImmediate(_points[point]);
                }
                else
                {
                    Destroy(_points[point]);
                }
                _points[point] = Instantiate(hullPointPrefab, point, Quaternion.identity, parent);
                _points[point].hideFlags = HideFlags.DontSaveInEditor;

            }
            ready = false;
        }
 
        public void Reset()
        {
            foreach (var kvp in _points)
            {
                if (Application.isEditor && !Application.isPlaying)
                    DestroyImmediate(kvp.Value);
                else
                    Destroy(kvp.Value);
            }
            _points.Clear();
            ready = false;
        }
    }

    
    public static void GenerateTestPoints(Vector3 centerPoint, Vector2 spreadRange, int startIndex, int points, Vector3[] array)
    {
        Debug.Assert(startIndex + points < array.Length);
        for (int i = startIndex; i < startIndex +points; i++)
        {
            var rDist = UnityEngine.Random.Range(spreadRange.x, spreadRange.y);
            var rAngle = UnityEngine.Random.Range(0, 360f);
            var offset = (Quaternion.Euler(0, 0, rAngle) * Vector3.right) * rDist;
            array[i] = centerPoint + offset;
            
        }
        var rDist2 = UnityEngine.Random.Range(spreadRange.x, spreadRange.y);
        var rAngle2 = UnityEngine.Random.Range(0, 360f);
        var offset2 = (Quaternion.Euler(0, 0, rAngle2) * Vector3.right) * rDist2;
        array[array.Length-1] = centerPoint + offset2;
    }

    
    

    [ContextMenu("Generate Test Points")]
    public void DoHullTest()
    {

        ResetPointVisuals();
        
        var totalPoints = numAreas * pointsPerArea;
        Vector3[] points = new Vector3[totalPoints+1];

        GenPoints(points);
        
        visualizer.VisualizePoints(testParent, points);
        //var hullPoints = Quickhull.FindConvexHull(points).HullPoints;
        
        //DrawResult();
        
        
        
        void GenPoints(Vector3[] vector3s)
        {
            testPoints = new Vector3[numAreas];
            testParent = new GameObject("Points").transform;

            Vector3 direction = Vector3.up;
            Vector3 origin = transform.position;
            for (int i = 0; i < numAreas; i++)
            {
                int startIndex = (i * pointsPerArea);
                var angle = UnityEngine.Random.Range(angleRange.x, angleRange.y);
                if (negateAngleOnOdd && i % 2 == 0) angle *= -1;
                direction = Quaternion.Euler(0, 0, angle) * direction;
                origin += direction.normalized * (UnityEngine.Random.Range(radius, radius * 2));
                testPoints[i] = origin;
                if(origin == Vector3.zero)Debug.LogWarning("Gen");
                HullTester.GenerateTestPoints(origin, spreadRange, startIndex, pointsPerArea, vector3s);
            }
        }
       
        void ResetPointVisuals()
        {
            if (testParent != null)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    Destroy(testParent.gameObject);
                }
                else
                {
                    DestroyImmediate(testParent.gameObject);
                }
#else
            Destroy(testParent.gameObject);
#endif
            }
        }

    }

    [ContextMenu("Reset Test")]
    public void ResetTest()
    {
        visualizer.Reset();
    }

    [ContextMenu("Solve Hull")]
    public void RunTest()
    {
        visualizer.Solve(testParent);
    }
}
