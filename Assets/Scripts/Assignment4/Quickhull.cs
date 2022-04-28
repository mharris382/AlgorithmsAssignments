using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment4
{
    public class Hull
    {
        public void AddPoint(Vector3 point)
        {
            
        }
    }
    public static class Quickhull
    {


        public static Hull FindConvexHull(Vector3[] points)
        {
           
            
          
            void FindLeftAndRightmostPoints(out Vector3 rightMost, out Vector3 leftMost)
            {
                Vector3 leftMostPoint, rightMostPoint;
                leftMostPoint = rightMostPoint = Vector3.zero;
                float smallestX = float.MaxValue, largestX = float.MinValue;
                foreach (var point in points)
                {
                    if (point.x < smallestX)
                    {
                        smallestX = point.x;
                        leftMostPoint = point;
                    }

                    if (point.x > largestX)
                    {
                        largestX = point.x;
                        rightMostPoint = point;
                    }
                }

                leftMost = leftMostPoint;
                rightMost = rightMostPoint;
            }
            
            var hull = new Hull();
            List<Vector3> pnts = new List<Vector3>(points.Length);
            List<Vector3> S1, S2;
            Vector3 A, B;
            
            FindLeftAndRightmostPoints(out A, out B);
            
            hull.AddPoint(A);
            hull.AddPoint(B);
            
            var segment = new LineSegment(A, B);
            
            
            var leftPoints = S1 = new List<Vector3>();
            var rightPoints = S2 = new List<Vector3>();
            foreach (var point in points)
            {
                if (IsPointLeftOfOrientedLine(segment, point))
                {
                    leftPoints.Add(point);
                }
                else
                {
                    rightPoints.Add(point);
                }
            }
            
            
            FindHull(S1, A, B, hull);
            FindHull(S2, B, A, hull);
            return hull;
        }

        public  static void FindHull(List<Vector3> pointSet, Vector3 p, Vector3 q, Hull hull)
        {
            if(pointSet.Count == 0)
                return;

            Vector3 FindFarthestPoint()
            {
                Vector3 pnt = p;
                float farthestDistance = 0;
                var segment = new LineSegment(p, q);
                foreach (var point in pointSet)
                {
                    var distance = DistanceToLine(segment, point);
                    if (distance > farthestDistance)
                    {
                        farthestDistance = distance;
                        pnt = point;
                    }
                }

                return pnt;
            }

            var farthestPoint = FindFarthestPoint();
            hull.AddPoint(farthestPoint);
            
        }

       public static float SignOfPoint(LineSegment segment, Vector3 point)
        {
            var p1 = segment.P0;
            var p2 = segment.P1;
            var p3 = point;
            var d1 = p1 - p3;
            var d2 = p2 - p3;
            return d1.x * d2.y - d2.x * d1.y;
        }
        public static bool IsPointLeftOfOrientedLine(LineSegment segment, Vector3 point)
        {
            float slope = segment.GetSlope();
            if (float.IsPositiveInfinity(slope))
            {
                return point.x < segment.P0.x;
            }
            else 
            {
                var px = point.x;
                var b = segment.GetXIntercept();
                var actualY = point.y;
                var lineY = (px * slope) + b;
                bool t = actualY < lineY;
                return (t && slope > 0) || (!t && slope < 0);
            }
            
            var direction = segment.LineDirection;
            Debug.Assert(direction.x > 0, "Line not oriented in positive direction");
            var orientedLineNormal = Vector3.Cross(direction, Vector3.forward);
            var compareDirection = point - segment.P0;
            orientedLineNormal.z = 0;compareDirection.z = 0;
            orientedLineNormal.Normalize();
            compareDirection.Normalize();
            //returns value between -1 and 1.  1 if same direction as normal, -1 if opposite direction as normal, 0 is perp to normal
            return Vector2.Dot(orientedLineNormal, compareDirection) > 0;
        }
        
        static float DistanceToLine(LineSegment segment, Vector3 p)
        {
            var m = segment.GetSlope();
            var b = segment.GetXIntercept();
            return ((-m * p.x) + p.y - b) / Mathf.Sqrt(Mathf.Pow(m, 2) + 1);
        }
        
        public struct LineSegment
        {
            public LineSegment(Vector3 p0, Vector3 p1)
            {
                if (p0.x > p1.x)
                {
                    P1 = p0;
                    P0 = p1;
                }
                else
                {     
                    P0 = p0;
                    P1 = p1;
                }
            }

            public Vector3 P0 { get; set; }

            public Vector3 P1 { get; set; }

            
            public Vector2 LineDirection => (P1 - P0).normalized;

            public float GetSlope()
            {
                if (LineDirection.x == 0) return float.PositiveInfinity;
                return (LineDirection.y / LineDirection.x);
            }

            public float GetXIntercept()
            {
                var x0 = P0.x;
                var y0 = P0.y;
                var slope = GetSlope();
                var b = -((slope * x0) - y0);
                Debug.Assert(Math.Abs(((slope * P1.x) + b) - P1.y) < 0.001f, "Slope or intercept was not calculated correctly");
                return b;
            }
        }

        public static bool IsPointInsideTriangle(Triangle triangle, Vector3 point)
        {
            
            var seg1 = new LineSegment(triangle.P0, triangle.P1);
            var seg2 = new LineSegment(triangle.P1, triangle.P2);
            var seg3 = new LineSegment(triangle.P2, triangle.P0);
            var d1 = SignOfPoint(seg1, point);
            var d2 = SignOfPoint(seg2, point);
            var d3 = SignOfPoint(seg3, point);
            
            bool left1 = IsPointLeftOfOrientedLine(seg1, point);
            bool left2 = IsPointLeftOfOrientedLine(seg2, point);
            bool left3 = IsPointLeftOfOrientedLine(seg3, point);
            bool hasLeft = (d1 < 0) || (d2 < 0) || (d3<0);
            bool hasRight = (d1 >0) || (d2 > 0) || (d3 > 0);
            return !(hasLeft && hasRight);
        }
        
        public struct Triangle
        {
            public Vector3 P0, P1, P2;
        }

        public static void TestPoints()
        {
            var segment = new LineSegment(new Vector3(0, 0, 0), new Vector3(1, 1, 0));
            
            var left = new Vector3(1, 4, 0);
            var right = new Vector3(1, -4, 0);
            Debug.Assert(IsPointLeftOfOrientedLine(segment, left));
            Debug.Assert(!IsPointLeftOfOrientedLine(segment, right));
        }

        public static void TestTriangle()
        {
            var testTriangle = new Triangle()
            {
                P0 = new Vector3(0, 0),
                P1 = new Vector3(1, 2),
                P2 = new Vector3(2, 0)
            };
            var inside = new Vector2(1, 1);
            var outside = new Vector2(1, 3);
            Debug.Assert(IsPointInsideTriangle(testTriangle, inside));
            Debug.Assert(!IsPointInsideTriangle(testTriangle, outside));
        }
    }
}
