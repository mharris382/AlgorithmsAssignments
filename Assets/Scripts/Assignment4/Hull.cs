using System.Collections.Generic;
using UnityEngine;

namespace Assignment4
{
    public class Hull
    {
        private List<Vector3> points = new List<Vector3>();
        public void AddPoint(Vector3 point)
        {
            points.Add(point);
        }

        public Vector3[] HullPoints => points.ToArray();
    }
}