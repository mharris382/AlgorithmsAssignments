using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment4
{
    public class Hull
    {
        private List<Vector3> points = new List<Vector3>();

        public event Action<Vector3> onPointAdded;
        
        public void AddPoint(Vector3 point)
        {
            points.Add(point);
            onPointAdded?.Invoke(point);
        }

        public Vector3[] HullPoints => points.ToArray();


        public void DrawGizmos(Color c)
        {
            var prevColor = Gizmos.color;
            Gizmos.color = c;

            if (points.Count < 1)
            {
                Debug.LogError($"Cannot draw hull, does not have enough points({points.Count})!");
                return;
            }

            Vector3 prevPoint = Vector3.zero;
            for (int i = 1; i < points.Count; i++)
            {
                var p0 = points[i-1];
                var p1 =prevPoint = points[i];
                Gizmos.DrawLine(p0, p1);
            }
            Gizmos.DrawLine(prevPoint, points[0]);
            Gizmos.color = prevColor;
        }
    }
}