using System.Collections.Generic;
using UnityEngine;

namespace Assignment4
{
    public class QuickHullSteps
    {
        public class QuickHullStep
        {
            public bool IsFirstStep { get; set; }
            public QuickHullStep parentStep;

            public Quickhull.LineSegment Segment;
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
                Segment = new Quickhull.LineSegment(p, q);
                var pnts = parent.stepPoints;
                if (pnts.Count == 0)
                {
                    StepTerminatedRecursiveTree = true;
                    return;
                }

                StepTerminatedRecursiveTree = false;
            }

            public int GetFurthestPoint(List<Vector3> pnts, Quickhull.LineSegment segment)
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