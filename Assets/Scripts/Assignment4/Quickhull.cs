using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Assignment4
{
    public static partial class Quickhull
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
            pnts.AddRange(points.Distinct()); //if 2 points are in the same position it is redundant to test both


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

        public static void FindHull(List<Vector3> pointSet, Vector3 p, Vector3 q, Hull hull)
        {
            if (pointSet.Count == 0)
                return;

            Vector3 FindFarthestPoint()
            {
                Vector3 pnt = p;
                float farthestDistance = 0;
                var segment = new LineSegment(p, q);
                foreach (var point in pointSet)
                {
                    var distance = segment.DistanceTo(point);
                    if (distance > farthestDistance)
                    {
                        farthestDistance = distance;
                        pnt = point;
                    }
                }

                return pnt;
            }

            var c = FindFarthestPoint();
            hull.AddPoint(c);

            List<Vector3> s1 = new List<Vector3>(pointSet.Count);
            List<Vector3> s2 = new List<Vector3>(pointSet.Count);
            foreach (var point in pointSet)
            {
                var group = DetermineGroup(p, q, c, point);
                switch (group)
                {
                    case PointGroup.In_Triangle:
                        break;
                    case PointGroup.Right_of_PC:
                        s1.Add(point);
                        break;
                    case PointGroup.Right_of_CQ:
                        s2.Add(point);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            FindHull(s1, p, c, hull);
            FindHull(s2, c, q, hull);
        }
    }
}