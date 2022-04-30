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
                return (LineDirection.y / LineDirection.x);
            }

            public float GetXIntercept()
            {
                var b1 = LineUtil.GetXIntercept(LineDirection, P0);
                var b2 = LineUtil.GetXIntercept(LineDirection, P1);
                //Debug.Assert(Math.Abs(b1 - b2) < 0.1f, $"Intercept of P0={b1:F2} != Intercept of P1={b2:F2}");
                return LineUtil.GetXIntercept(LineDirection, P0);
            }

            public bool IsPointLeftOfLine(Vector3 point)
            {
                return DetermineSideOfLine(point.x, point.y) > 0;
            }

            public float DistanceTo(Vector2 p)
            {
                float m = LineUtil.GetSlope(LineDirection);
                float b = LineUtil.GetXIntercept(LineDirection, P0);
                var distance = (-m * p.x) + p.y - b / Mathf.Sqrt((m * m) + 1);
                return distance;
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


            static Vector3[] GetArrowPoints(Vector3 from, Vector3 to, float arrowLength = 0.4f, float arrowWidth = 0.5f, float arrowOffset = 0.2f)
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