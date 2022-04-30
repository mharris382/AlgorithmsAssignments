using System;
using UnityEngine;

namespace Assignment4
{
    public class LineTest : MonoBehaviour
    {
        public Transform l1;
        public Transform l2;


        private void OnDrawGizmos()
        {
            if (l1 == null || l2 == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(l1.position, l1.right * 100);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(l2.position, l2.right * 100);
            var intersection = LineUtil.GetIntersection(l1.right, l2.right, l1.position, l2.position);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(intersection, 0.5f);
        }
    }
}