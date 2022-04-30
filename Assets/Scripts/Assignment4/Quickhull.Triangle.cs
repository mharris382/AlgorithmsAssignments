using UnityEngine;

namespace Assignment4
{
    public static partial class Quickhull
    {
        public struct Triangle
        {
            public Vector3 P0, P1, P2;

            public Vector3 P => P0;
            public Vector3 Q => P1;
            public Vector3 C => P2;

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
    }
}