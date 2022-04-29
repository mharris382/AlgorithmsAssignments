using System;
using UnityEngine;

namespace Assignment4
{
    public class SegmentTest : MonoBehaviour
    {

        public Transform p0;
        public Transform p1;


        public Transform testPoint;

        private void OnDrawGizmos()
        {
            if (p0 == null || p1 == null || testPoint==null) return;
            var segment = new Quickhull.LineSegment(p0.position, p1.position);
            
            segment.DrawSegmentGizmos(Color.yellow);
            
            var sign = Quickhull.SignOfPoint(segment, testPoint.position);

            bool isLeft = Quickhull.IsPointLeftOfOrientedLine(segment, testPoint.position);
            
            Gizmos.color = isLeft ? Color.green : Color.red;
            Gizmos.DrawSphere(testPoint.position, 0.5f);
        }


        static Vector3[] GetArrowPoints(Vector3 from, Vector3 to, float arrowLength = 0.4f, float arrowWidth = 0.5f, float arrowOffset = 0.2f)
        {
            var pnts = new Vector3[7];
            pnts[0] = from;
            pnts[1] = pnts[5] = to;
            Vector2 direction = (to - from);
            var dN = direction.normalized;
            var perp = Vector2.Perpendicular(dN);
            var distance = direction.magnitude;
            var arrowStart = (Vector2)to - (dN * (arrowLength));
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