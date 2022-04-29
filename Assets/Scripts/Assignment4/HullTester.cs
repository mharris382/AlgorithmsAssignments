using System;
using System.Collections;
using System.Collections.Generic;
using Assignment4;
using UnityEngine;

public class HullTester : MonoBehaviour
{
    
    public LineRenderer hullVisual;
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

        public void VisualizePoints(Transform parent, Vector3[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                var pos = points[i];
                Instantiate(pointPrefab, pos, Quaternion.identity, parent);
            }
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
    }


    [ContextMenu("Generate Test Points")]
    public void DoHullTest()
    {

        ResetPointVisuals();
        
        var totalPoints = numAreas * pointsPerArea;
        Vector3[] points = new Vector3[totalPoints+1];

        GenPoints(points);
        
        visualizer.VisualizePoints(testParent, points);
        var hullPoints = Quickhull.FindConvexHull(points).HullPoints;
        
        DrawResult();
        
        
        
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
                HullTester.GenerateTestPoints(origin, spreadRange, startIndex, pointsPerArea, vector3s);
            }
        }
        void DrawResult()
        {
            hullVisual.enabled = true;
            hullVisual.positionCount = hullPoints.Length;
            hullVisual.SetPositions(hullPoints);
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

    private void OnDrawGizmosSelected()
    {
        if (testPoints != null && testPoints.Length > 0)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < testPoints.Length; i++)
            {
                Gizmos.DrawWireSphere(testPoints[i], 0.5f);
            }
        }
    }
}
