using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ReSharper disable InconsistentNaming

namespace Assignment4
{
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
            List<Vector3> S1, S2;
            Vector3 A, B;
            List<Vector3> pnts = new List<Vector3>(points.Length);
            pnts.AddRange(points.Distinct());  //if 2 points are in the same position it is redundant to test both
            
            
         
            
            FindLeftAndRightmostPoints(out A, out B);
            
            hull.AddPoint(A);
            hull.AddPoint(B);
            
            var segment = new LineSegment(A, B);
            
            
            var leftPoints = S1 = new List<Vector3>();
            var rightPoints = S2 = new List<Vector3>();
            foreach (var point in pnts)
            {
                if (IsPointLeftOfOrientedLine(segment, point))
                    leftPoints.Add(point);
                else
                    rightPoints.Add(point);
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

            var c =FindFarthestPoint();
            hull.AddPoint(c);
            
            
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
            var sign = SignOfPoint(segment, point);
            return sign > 0;
        }
        
        public static float DistanceToLine(LineSegment segment, Vector3 p)
        {
            var m = segment.GetSlope();
            var b = segment.GetXIntercept();
            return ((-m * p.x) + p.y - b) / Mathf.Sqrt(Mathf.Pow(m, 2) + 1);
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

        public static PointGroup DetermineGroup(Vector3 P, Vector3 Q, Vector3 C, Vector3 point)
        {
            var triangle = new Triangle(P, Q, C);
            var PC = new LineSegment(P, C);
            var CQ = new LineSegment(C, Q);
            var QP = new LineSegment(Q, P);
            bool leftOfPC = IsPointLeftOfOrientedLine(PC, point);
            bool leftOfCQ = IsPointLeftOfOrientedLine(CQ, point);
            bool leftOfQP = IsPointLeftOfOrientedLine(QP, point);
            bool hasLeft = (leftOfPC) || (leftOfCQ) || (leftOfQP);
            bool hasRight = (!leftOfPC) || (!leftOfCQ) || (!leftOfQP);
            
            if (!(hasLeft && hasRight)) 
                return PointGroup.In_Triangle;
            
            Debug.Assert(!leftOfCQ || !leftOfPC, $"Was not right of CQ or right of PC! the point {point} should not be in this group");
            return !leftOfPC ? PointGroup.Right_of_PC : PointGroup.Right_of_CQ;
        }
        public static PointGroup DetermineGroup(Triangle triangle, Vector3 point)
        {
            var PC = triangle.PC;
            var CQ = triangle.CQ;
            var QP = triangle.QP;
            bool leftOfPC = IsPointLeftOfOrientedLine(PC, point);
            bool leftOfCQ = IsPointLeftOfOrientedLine(CQ, point);
            bool leftOfQP = IsPointLeftOfOrientedLine(QP, point);
            
            bool hasLeft = (leftOfPC) || (leftOfCQ) || (leftOfQP);
            bool hasRight = (!leftOfPC) || (!leftOfCQ) || (!leftOfQP);
            
            if (!(hasLeft && hasRight)) 
                return PointGroup.In_Triangle;
            if (!leftOfCQ || !leftOfCQ) throw new Exception($"Was not right of CQ or right of PC! the point {point} should not be in this group");
            //Debug.Assert(!leftOfCQ || !leftOfPC, $"Was not right of CQ or right of PC! the point {point} should not be in this group");
            return !leftOfPC ? PointGroup.Right_of_PC : PointGroup.Right_of_CQ;
        }
        public enum PointGroup
        {
            In_Triangle,
            Right_of_PC,
            Right_of_CQ
        }
        
        
        public struct Triangle
        {
            public Vector3 P0, P1, P2;

            public  Vector3 P => P0;
            public    Vector3 Q => P1;
            public   Vector3 C => P2;
            
            public LineSegment PC => new LineSegment(P, C);
            public LineSegment CQ => new LineSegment(C, Q);
            public LineSegment QP => new LineSegment(Q, P);

            public LineSegment[] PCQ => new[] {PC, CQ, QP};
            
            public Triangle(Vector3 p, Vector3 q, Vector3 c)
            {
                P0 = p;
                P1 = q;
                P2 = c;
            }
            public void DrawGizmos()
            {
                Gizmos.DrawLine(P0, P1);
                Gizmos.DrawLine(P1, P2);
                Gizmos.DrawLine(P2, P0);
            }
            public void DrawGizmos(Color color)
            {
                var prev = Gizmos.color;
                Gizmos.color = color;
                DrawGizmos();
                Gizmos.color = prev;
            }
        }
        
        public struct LineSegment
        {
            public LineSegment(Vector3 p0, Vector3 p1)
            {
                P0 = p0;
                P1 = p1;
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

            

            public void DrawSegmentGizmos()
            {
                var arrowPoints = GetArrowPoints(P0, P1);
                for (int i = 1; i < arrowPoints.Length; i++)
                {
                    var gp0 = arrowPoints[i - 1];
                    var gp1 = arrowPoints[i];
                    Gizmos.DrawLine(gp0, gp1);
                }
            }

            public void DrawSegmentGizmos(Color c)
            {
                var prev = Gizmos.color;
                Gizmos.color = c;
                DrawSegmentGizmos();
                Gizmos.color = prev;
            }
            
            
            static Vector3[] GetArrowPoints(Vector3 from, Vector3 to, float arrowLength = 0.4f, float arrowWidth = 0.5f, float arrowOffset = 0.2f)
            {
                var pnts = new Vector3[7];
                pnts[0] = from;
                pnts[1] = pnts[5] = to;
                Vector2 direction = (to - from);
                var dN = direction.normalized;
                var perp = Vector2.Perpendicular(dN);
                var distance = direction.magnitude;
                var arrowStart = (Vector2)to - (dN * (arrowLength));
                pnts[3] = pnts[6] = arrowStart;
                var offset = (perp.normalized * arrowWidth / 2f);
            
                var p1 = arrowStart + offset;
                var p2 = arrowStart - offset;
                pnts[2] = p1 - (arrowOffset * dN);
                pnts[4] = p2 - (arrowOffset * dN);
            
                pnts[3] = Vector3.Lerp(pnts[3], pnts[4], 0.1f);
            
                return pnts;
            }
        }


      
        
    }

    public static class LineUtil
    {
        public static float GetXIntercept(Vector2 line)
        {
            var slope = GetSlope(line);
            return (slope * line.x) - line.y;
        }

        public static float GetSlope(Vector2 line)
        {
            return  line.y == 0 ? float.PositiveInfinity : line.x / line.y;
        }
        
        public static Vector3 GetIntersection(Vector3 l1, Vector3 l2)
        {
            var m1 = LineUtil.GetSlope(l1);
            var m2 = LineUtil.GetSlope(l2);
            if (m1 == m2)
            {
                throw new Exception("Lines are parallel");
            }
            var c1 = LineUtil.GetXIntercept(l1);
            var c2 = LineUtil.GetXIntercept(l2);
            float b1 = l1.y, b2 = l2.y, a1 = l1.x, a2 = l2.x;
            var dy = (c1 * a2) - (c2 * a1);
            var dx = (b1 * c2) - (b2 * c1);
            var n = (a1 * b2) - (a2 * b1);
            return new Vector3(dx / n, dy / n);
        }
    }
}
