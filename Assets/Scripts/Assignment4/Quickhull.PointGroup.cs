using System;
using UnityEngine;

namespace Assignment4
{
    public static partial class Quickhull
    {
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
    }
}