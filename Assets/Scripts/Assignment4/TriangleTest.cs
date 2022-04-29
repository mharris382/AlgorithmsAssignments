using System;
using UnityEngine;

namespace Assignment4
{
    public class TriangleTest : MonoBehaviour
    {
        public Transform[] trianglePoints;
        public Transform testPoint;


        private void OnDrawGizmos()
        {
            if (trianglePoints.Length != 3) return;
            foreach (var trianglePoint in trianglePoints)
                    if (trianglePoint == null) return;
            if (testPoint == null) return;
            var triangle = new Quickhull.Triangle()
            {
                P0 =trianglePoints[0].position, P1= trianglePoints[1].position, P2=trianglePoints[2].position
            };
            var point = testPoint.position;
            bool inside = Quickhull.IsPointInsideTriangle(triangle, point);
            var color = inside ? Color.green : Color.red;
            Gizmos.color = color;
            Gizmos.DrawSphere(point, 0.5f);
            triangle.DrawGizmos(Color.yellow);
        }
    }
}