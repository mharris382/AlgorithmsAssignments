using System;
using UnityEngine;

namespace Assignment4
{
    public static partial class Quickhull
    {
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
                return (P1.y - P0.y) / (P1.x - P0.x);
            }

            public float GetXIntercept()
            {
                return P0.y - (P0.x * GetSlope());
            }

            public bool IsPointLeftOfLine(Vector3 point)
            {
                return DetermineSideOfLine(point.x, point.y) > 0;
            }

            public float DistanceTo(Vector2 p, bool debug =false)
            {
                if (Math.Abs(P0.x - P1.x) < Mathf.Epsilon)//check if p0 and p1 have approximately same x value, therefore m == NaN
                {
                    return Mathf.Abs(p.x - P0.x);//since the line is perp to the x axis the shortest distance is equal to x
                }
                else if (Math.Abs(P0.y - P1.y) < Mathf.Epsilon)
                {
                    return Mathf.Abs(p.y - P0.y);
                }
                
                float m = this.GetSlope();
                float b = this.GetXIntercept();
                var n = (-m * p.x) + p.y - b;
                var d = Mathf.Sqrt(m*m + 1);
                var distance = n / d;
                if (debug)
                {
                    Debug.DrawLine(P0, P1, Color.magenta, 0.25f);
                    var n2 = Vector2.Perpendicular(LineDirection);
                    Debug.DrawRay(P0, n2, Color.blue, 0.25f);
                    Debug.DrawRay(p, n2*distance, Color.green, 2);
                }
                return Mathf.Abs(distance);
            }

            public int DetermineSideOfLine(Vector2 p)
            {
                var b = P1;
                var a = P0;
                return (int) Mathf.Sign((b.x - a.x) * (p.y - a.y) - (b.y - a.y) * (p.x - a.x));
            }

            public int DetermineSideOfLine(float x, float y)
            {
                var p = new Vector2(x, y);
                return DetermineSideOfLine(p);
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


            public static Vector3[] GetArrowPoints(Vector3 from, Vector3 to, float arrowLength = 0.4f, float arrowWidth = 0.5f, float arrowOffset = 0.2f)
            {
                var pnts = new Vector3[7];
                pnts[0] = @from;
                pnts[1] = pnts[5] = to;
                Vector2 direction = (to - @from);
                var dN = direction.normalized;
                var perp = Vector2.Perpendicular(dN);
                var distance = direction.magnitude;
                var arrowStart = (Vector2) to - (dN * (arrowLength));
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
}