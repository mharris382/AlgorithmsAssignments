using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static Assignment4.Quickhull;

namespace Assignment4
{
    public class QuickHullSteps
    {
  

        public static (int, int) Split(LineSegment segment, List<Vector3> points, out List<Vector3> leftSideGroup, out List<Vector3> rightSideGroup)
        {
            leftSideGroup = new List<Vector3>(points.Count);
            rightSideGroup = new List<Vector3>(points.Count);
            int rCount = 0, lCount = 0;

            foreach (var point in points)
            {
                if (segment.IsPointLeftOfLine(point))
                {
                    lCount++;
                    leftSideGroup.Add(point);
                }
                else
                {
                    rCount++;
                    rightSideGroup.Add(point);
                }
            }
            
            return (lCount, rCount);
        }
        static void FindOuterPoints(IEnumerable<Vector3> allPoints, out Vector3 rightPoint, out Vector3 leftPoint)
        {
            leftPoint = rightPoint = Vector3.zero;
            float xMin = float.MaxValue;
            float xMax = float.MinValue;
            foreach (var point in allPoints)
            {
                if (point.x < xMin)
                {
                    xMin = point.x;
                    leftPoint = point;
                }

                if (point.x > xMax)
                {
                    xMax = point.x;
                    rightPoint = point;
                }
            }
            
        }
        public static (Step, List<RecursiveStep>) GetSteps(List<Vector3> points)
        {
            var los = new List<RecursiveStep>();
            var hull = new Hull();
            List<Vector3> S1, S2;
            Vector3 A, B;
            
            FindOuterPoints(points, out A, out B);
            var segment = new LineSegment(A, B);
            var cnt = Split(segment, points, out S1, out S2);
            var firstStep = new Step()
            {
                S1 = S1,
                S2 = S2,
                segement = segment
            };
            
            SolveRecursiveSteps(firstStep, false, ref los);
            SolveRecursiveSteps(firstStep, true, ref los);
            return (firstStep, los);
        }

        private static void SolveRecursiveSteps(Step parentStep, bool isLeftSide, ref List<RecursiveStep> recursiveSteps)
        {
         
            List<Vector3> points;
            Vector3 A, B;
            
            if (isLeftSide)
            {
                A = parentStep.segement.P0;
                B = parentStep.segement.P1;
                points = parentStep.S1;
            }
            else
            {
                A = parentStep.segement.P1;
                B = parentStep.segement.P0;
                points = parentStep.S2;
            }
            
            if (points == null || points.Count == 0)
                return;
            
            var segement = new LineSegment(A, B);
            int FindFurthestPoint()
            {
                
                int furthestIndex = -1;
                float furthestDistance = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    var pnt = points[i];
                    var dist = segement.DistanceTo(pnt);
                    if (dist > furthestDistance)
                    {
                        furthestDistance = dist;
                        furthestIndex = i;
                    }
                }
                return furthestIndex;
            }
            
            
            
            var S1 = new List<Vector3>(points.Count); 
            var S2 = new List<Vector3>(points.Count);
            int iFurthest = FindFurthestPoint();
            var vFurthest = points[iFurthest];
            var triangle = new Triangle(A, B, vFurthest);
            for (int i = 0; i < points.Count; i++)
            {
                var pnt = points[i];
                if (i == iFurthest || IsPointInsideTriangle(triangle, pnt)) 
                    continue;
                var segmentPC = new LineSegment(A, vFurthest);
                var segmentCQ = new LineSegment(vFurthest, B);
                if (segmentPC.IsPointLeftOfLine(pnt))
                {
                    S1.Add(pnt);
                }
                else
                {
                    Debug.Assert(segmentCQ.IsPointLeftOfLine(pnt));
                    S2.Add(pnt);
                }
            }
            
            
            
            var step = new RecursiveStep() {
                isRightGroup = !isLeftSide,
                segement = segement,
                furthestPoint = vFurthest,
                S1 = S1,
                S2 = S2
            };
            recursiveSteps.Add(step);
            SolveRecursiveSteps(step, false, ref recursiveSteps);
            SolveRecursiveSteps(step, true, ref recursiveSteps);
        }

        public class Step
        {
            public LineSegment segement;
            public List<Vector3> S1;
            public List<Vector3> S2;
            
            public virtual void DrawSegment(Color p1Color, Color p2Color, float pointRadius = 0.5f,
                bool wire = false, bool withArrow = true)
            {
                var prev = Gizmos.color;
                
                
                Vector3 offset;
                
                var mp = Vector3.Lerp(segement.P0, segement.P1, 0.5f);
                
                Gizmos.color = p1Color;
                DrawPoint(segement.P0, pointRadius, wire);
                Gizmos.DrawLine(segement.P0, mp);
                
                Gizmos.color = p2Color;
                DrawPoint(segement.P1, pointRadius, wire);
                Gizmos.DrawLine(mp, segement.P1);

         

                if (withArrow)
                {
                    var color = Color.gray;
                    color.a = 0.5f;
                    Gizmos.color = color;
                    var arrowPoints = LineSegment.GetArrowPoints(segement.P0, segement.P1);

                    
                    for (int i = 1; i < arrowPoints.Length; i++) 
                        Gizmos.DrawLine(arrowPoints[i - 1], arrowPoints[i]);
                }

            
                
                Gizmos.color = prev;
            }
            private void DrawPoint(Vector2 point, float pointRadius, bool wire)
            {
                if (wire) Gizmos.DrawWireSphere(point, pointRadius);
                else Gizmos.DrawSphere(point, pointRadius);
            }
            public void DrawS1(Color color, float pRadius=0.1f, bool wire = true) => DrawGroup(S1, color, pRadius, wire);
            public  void DrawS2(Color color, float pRadius = 0.1f, bool wire = true) => DrawGroup(S2, color, pRadius, wire);
            private void DrawGroup(List<Vector3> group, Color color, float pRadius=0.1f, bool wire = true)
            {
                var prev = Gizmos.color;
                Gizmos.color = color;
                
                foreach (var vector2 in S1) 
                    DrawPoint(vector2, pRadius, wire);

                Gizmos.color = prev;
            }
        }
        
        public class RecursiveStep : Step
        {
            private const float lineOffsetAmount = 0.05f;
            public bool isRightGroup;
            public Vector3 furthestPoint;
           

            public override void DrawSegment(Color p1Color, Color p2Color, float pointRadius = 0.5f,
                bool wire = false, bool withArrow = true)
            {
                var prev = Gizmos.color;
                
                
                Vector3 offset;
                if (segement.LineDirection.x > 0 && isRightGroup || (!(segement.LineDirection.x < 0 && !isRightGroup)))
                {
                    offset = Vector3.up *lineOffsetAmount;
                }
                else
                {
                    offset = Vector3.down *lineOffsetAmount;
                }
                
                var mp = Vector3.Lerp(segement.P0, segement.P1, 0.5f);
                mp += offset;
                
                Gizmos.color = p1Color;
                DrawPoint(segement.P0+offset, pointRadius, wire);
                Gizmos.DrawLine(segement.P0+offset, mp);
                
                Gizmos.color = p2Color;
                DrawPoint(segement.P1+offset, pointRadius, wire);
                Gizmos.DrawLine(mp, segement.P1);

         

                if (withArrow)
                {
                    var color = Color.gray;
                    color.a = 0.5f;
                    Gizmos.color = color;
                    var arrowPoints = LineSegment.GetArrowPoints(segement.P0, segement.P1);

                    for (int i = 0; i < arrowPoints.Length; i++)
                        arrowPoints[i] += offset * 2;

                    for (int i = 1; i < arrowPoints.Length; i++) 
                        Gizmos.DrawLine(arrowPoints[i - 1], arrowPoints[i]);
                }

            
                
                Gizmos.color = prev;
            }

            private void DrawPoint(Vector2 point, float pointRadius, bool wire)
            {
                if (wire) Gizmos.DrawWireSphere(point, pointRadius);
                else Gizmos.DrawSphere(point, pointRadius);
            }


            private void DrawGroup(List<Vector3> group, Color color, float pRadius=0.1f, bool wire = true)
            {
                var prev = Gizmos.color;
                Gizmos.color = color;
                
                foreach (var vector2 in S1) 
                    DrawPoint(vector2, pRadius, wire);

                Gizmos.color = prev;
            }
            public void DrawFurthestPoint(Color color, float pRadius = 0.5f, bool wire = false)
            {
                var prev = Gizmos.color;
                Gizmos.color = color;
                DrawPoint(furthestPoint,pRadius, wire);
                Gizmos.color = prev;
            }
            
           
        }
        
        
        public class QuickHullStep
        {
            public bool IsFirstStep { get; set; }
            public QuickHullStep parentStep;

            public LineSegment Segment;
            public Vector3? C;

            public bool StepTerminatedRecursiveTree { get; private set; }

            private QuickHullStep leftChild;
            private QuickHullStep rightChild;
            public List<Vector3> stepPoints;

            public QuickHullStep(Vector3[] points)
            {
                IsFirstStep = true;
                stepPoints = new List<Vector3>(points.Length);
                stepPoints.AddRange(points);
                C = null;
                parentStep = null;
                StepTerminatedRecursiveTree = false;
            }

            public QuickHullStep(QuickHullStep parent, Vector3 p, Vector3 q)
            {
                Segment = new LineSegment(p, q);
                var pnts = parent.stepPoints;
                if (pnts.Count == 0)
                {
                    StepTerminatedRecursiveTree = true;
                    return;
                }

                StepTerminatedRecursiveTree = false;
            }

            public int GetFurthestPoint(List<Vector3> pnts, LineSegment segment)
            {
                int res = -1;
                float furthestDistance = 0;
                for (int i = 0; i < pnts.Count; i++)
                {
                    var point = pnts[i];
                }

                return res;
            }

            public List<Vector3> GetLeftChild()
            {
                var leftPoints = new List<Vector3>();

                return leftPoints;
            }

            public List<Vector3> GetRightPoints()
            {
                var rightPoints = new List<Vector3>();

                return rightPoints;
            }
            
            
            
        }
    }
}