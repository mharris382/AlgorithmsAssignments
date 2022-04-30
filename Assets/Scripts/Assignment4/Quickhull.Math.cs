using UnityEngine;

namespace Assignment4
{
    public static partial class Quickhull
    {
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
            bool hasLeft = (d1 < 0) || (d2 < 0) || (d3 < 0);
            bool hasRight = (d1 > 0) || (d2 > 0) || (d3 > 0);
            return !(hasLeft && hasRight);
        }
        
    }
}