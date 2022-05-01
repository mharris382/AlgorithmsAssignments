using System;
using UnityEngine;

namespace Assignment4
{
    public class Distance : MonoBehaviour
    {
        public Transform p0;
        public Transform p1;
        public Transform p;


        public float distance;
        private void OnDrawGizmos()
        {
            if (p == null || p0 == null || p1 == null) return;
            var P0 = p0.position;
            var P1 = p1.position;
            var P = p.position;
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(P0, P1);

            Quickhull.LineSegment segment = new Quickhull.LineSegment(P0, P1);
            segment.DrawSegmentGizmos(Color.magenta);

            distance = segment.DistanceTo(P, true);
        }
        
        
    }
}