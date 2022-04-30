using System;
using UnityEngine;

namespace Assignment4
{
    public static class LineUtil
    {
        public static float GetXIntercept(Vector2 line, Vector2 referencePoint)
        {
            var slope = GetSlope(line);

            var y = referencePoint.y;
            var x = referencePoint.x;
            var b = y - (slope * x);
            return b;
        }

        public static float GetSlope(Vector2 line)
        {
            return line.y == 0 ? float.PositiveInfinity : line.x / line.y;
        }

        public static Vector3 GetIntersection(Vector3 l1, Vector3 l2, Vector3 l1Position, Vector3 l2Position)
        {
            var m1 = LineUtil.GetSlope(l1);
            var m2 = LineUtil.GetSlope(l2);
            if (m1 == m2)
            {
                throw new Exception("Lines are parallel");
            }

            var c1 = LineUtil.GetXIntercept(l1, l1Position);
            var c2 = LineUtil.GetXIntercept(l2, l2Position);
            float b1 = l1.y, b2 = l2.y, a1 = l1.x, a2 = l2.x;
            var dy = (c1 * a2) - (c2 * a1);
            var dx = (b1 * c2) - (b2 * c1);
            var n = (a1 * b2) - (a2 * b1);
            return new Vector3(dx / n, dy / n);
        }
    }
}