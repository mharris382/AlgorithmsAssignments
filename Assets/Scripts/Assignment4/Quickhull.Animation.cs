using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assignment4
{
    public delegate void QuickHullRecursiveStep(List<Vector3> pointSet, Vector3 p, Vector3 q);
    public static partial class Quickhull
    {
        private const float stepDuration = 1f;
        public static event QuickHullRecursiveStep recursiveStepOccured;

        public static IEnumerator AnimateQuickHull(Vector3[] points, Hull hull)
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

            List<Vector3> S1, S2;
            Vector3 A, B;
            List<Vector3> pnts = new List<Vector3>(points.Length);
            pnts.AddRange(points.Distinct()); //if 2 points are in the same position it is redundant to test both


            FindLeftAndRightmostPoints(out A, out B);
            yield return AddPoint(hull, A);
            yield return AddPoint(hull, B);
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

            recursiveStepOccured?.Invoke(S1, A, B);
            yield return AddPoint(hull, A);
            yield return AddPoint(hull, B);

            yield return AnimateQuickHull(S1, A, B, hull);

            recursiveStepOccured?.Invoke(S1, B, A);
            yield return AnimateQuickHull(S2, B, A, hull);
        }

        private static IEnumerator AnimateQuickHull(List<Vector3> pointSet, Vector3 p, Vector3 q, Hull hull)
        {
            if (pointSet.Count == 0)
                yield break;

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

            var c = FindFarthestPoint();
            List<Vector3> s1 = new List<Vector3>(pointSet.Count);
            List<Vector3> s2 = new List<Vector3>(pointSet.Count);
            foreach (var point in pointSet)
            {
                var group = DetermineGroup(p, q, c, point);
                switch (@group)
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

            recursiveStepOccured?.Invoke(s1, p, c);
            yield return AnimateQuickHull(s1, p, c, hull);

            recursiveStepOccured?.Invoke(s2, c, q);
            yield return AnimateQuickHull(s2, c, q, hull);
        }

        private static IEnumerator AddPoint(Hull hull, Vector3 point)
        {
            hull.AddPoint(point);
            yield return new WaitForSeconds(stepDuration);
        }
    }
}