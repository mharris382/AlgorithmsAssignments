using System;
using UnityEditor;
using UnityEngine;

namespace Assignment4
{
    public class TriangleTest2 : MonoBehaviour
    {
        public Transform[] trianglePoints;
        public Transform testPoint;


        public Quickhull.Triangle Triangle;
        public bool isTesting { get; set; }

        public bool IsValid()
        {
            isTesting = false;
            if (trianglePoints.Length != 3) return false;
            foreach (var trianglePoint in trianglePoints)
                if (trianglePoint == null) return false;
            if (testPoint == null) return false;
            return true;
        }
        private void OnDrawGizmos()
        {
            isTesting = false;
            if (trianglePoints.Length != 3) return;
            foreach (var trianglePoint in trianglePoints)
                if (trianglePoint == null) return;
            if (testPoint == null) return;
            isTesting = true;
            var triangle = new Quickhull.Triangle(trianglePoints[0].position, trianglePoints[1].position, trianglePoints[2].position);
            this.Triangle = triangle;
            var point = testPoint.position;
            triangle.DrawGizmos(Color.yellow);
            try
            {
                var group =Quickhull.DetermineGroup(triangle.P, triangle.Q, triangle.C, testPoint.position);
                switch (group)
                {
                    case Quickhull.PointGroup.In_Triangle:
                        Gizmos.color = Color.yellow;
                        break;
                    case Quickhull.PointGroup.Right_of_PC:
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawWireSphere(triangle.P, 0.25f);
                        Gizmos.DrawWireSphere(triangle.C, 0.25f);
                        var s1 = new Quickhull.LineSegment(triangle.P, triangle.C);
                        s1.DrawSegmentGizmos();
                        break;
                    case Quickhull.PointGroup.Right_of_CQ:
                        
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawWireSphere(triangle.C, 0.25f);
                        Gizmos.DrawWireSphere(triangle.Q, 0.25f);
                        var s2 = new Quickhull.LineSegment(triangle.C, triangle.Q);
                        s2.DrawSegmentGizmos();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                Gizmos.color = Color.red;
            }
            
            Gizmos.DrawSphere(point, 0.5f);
        }
    }


    [CustomEditor(typeof(TriangleTest2))]
    public class TriangleTest2Editor : Editor
    {
        private void OnSceneGUI()
        {
            var test = target as TriangleTest2;
            if (!test.isTesting) return;
            var triangle = test.Triangle;
            var P = triangle.P;
            var Q = triangle.Q;
            var C = triangle.C;
            Handles.color = Color.yellow;
            var centerPoint = (P + Q + C) / 3f;

            Vector3 GetLabelPos(Vector3 pos)
            {
                var offsetDir = pos - centerPoint;
                offsetDir.Normalize();
                return pos + (offsetDir * .5f);
            }
            
            Handles.Label(GetLabelPos(P), "P");
            Handles.Label(GetLabelPos(Q), "Q");
            Handles.Label(GetLabelPos(C), "C");

            var newP = Handles.FreeMoveHandle(P, Quaternion.identity, HandleUtility.GetHandleSize(P) * 0.03f, Vector3.zero, Handles.DotHandleCap);
            var newQ = Handles.FreeMoveHandle(Q, Quaternion.identity, HandleUtility.GetHandleSize(Q) * 0.03f, Vector3.zero, Handles.DotHandleCap);
            var newC = Handles.FreeMoveHandle(C, Quaternion.identity, HandleUtility.GetHandleSize(C) * 0.03f, Vector3.zero, Handles.DotHandleCap);
            newP.z = 0;
            newQ.z = 0;
            newC.z = 0;
            var pT = test.trianglePoints[0];
            var qT = test.trianglePoints[1];
            var cT = test.trianglePoints[2];
            if (newP != P)
            {
                Undo.RecordObject(pT, "Moved Point P");
                pT.position = newP;
            }
            else if (newQ != Q)
            {
                Undo.RecordObject(qT, "Moved Point Q");
                qT.position = newQ;
            }
            else if (newC != C)
            {
                Undo.RecordObject(cT, "Moved Point Q");
                cT.position = newC;
            }

            var testPointT = test.testPoint;
            Handles.color = Color.blue;
            Handles.Label(GetLabelPos(testPointT.position), "PNT");
            var newPoint = Handles.FreeMoveHandle(testPointT.position, Quaternion.identity, HandleUtility.GetHandleSize(testPointT.position) * 0.04f, Vector3.zero, Handles.DotHandleCap);
            if (newPoint != testPointT.position)
            {
                Undo.RecordObject(cT, "Moved Test Point");
                testPointT.position = newPoint;
            }
            
        }
    }
}