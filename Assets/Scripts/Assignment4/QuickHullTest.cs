using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment4
{
    public class QuickHullTest : MonoBehaviour
    {
        public Transform pointsParent;

        public GameObject testPointPrefab;


        public GameObject pObject;
        public GameObject qObject;
        public LineRenderer lineObject;
        
        public Transform parentUnused;
        public  Transform parentTestPoints;
        
        bool IsValid() => pointsParent.childCount > 5;


        private Queue<GameObject> unusedTestPointObjects = new Queue<GameObject>();
        private List<GameObject> testPointObjects = new List<GameObject>();
        private Coroutine _test;

        private void OnEnable()
        {
            Quickhull.recursiveStepOccured += ShowQuickhullStep;
        }
              
        private void OnDisable()
        {
            Quickhull.recursiveStepOccured -= ShowQuickhullStep;
        }

        
        
        public void StartTest()
        {
            ClearTestPoints();
            CheckParents();
            if (_test != null)
            {
                Quickhull.recursiveStepOccured -= ShowQuickhullStep;
                StopCoroutine(_test);
            }

            var hull = new Hull();
            var testPoints = GetTestPoints();
            _test = StartCoroutine(Quickhull.AnimateQuickHull(testPoints, hull));
        }


        IEnumerator RunTest()
        {
            var hull = new Hull();
            var testPoints = GetTestPoints();
            Quickhull.recursiveStepOccured += ShowQuickhullStep;
            yield return Quickhull.AnimateQuickHull(testPoints, hull);
            Quickhull.recursiveStepOccured -= ShowQuickhullStep;
        }

        private Vector3[] GetTestPoints()
        {
            if (!IsValid())
                throw new MissingComponentException("Invalid Quick Hull Test");
            Vector3[] pnts = new Vector3[pointsParent.childCount];
            for (int i = 0; i < pointsParent.childCount; i++)
            {
                pnts[i] = pointsParent.GetChild(i).position;
            }
            return pnts;
        }

        private void ShowQuickhullStep(List<Vector3> pointset, Vector3 p, Vector3 q)
        {
            ShowPQSegment(p, q);
            ShowCurrentGroup(pointset);   
        }
        
        void ShowPQSegment(Vector3 p, Vector3 q)
        {
            lineObject.useWorldSpace = true;
            pObject.transform.position = p;
            qObject.transform.position = q;
            lineObject.positionCount = 2;
            lineObject.SetPosition(0, p);
            lineObject.SetPosition(1, q);
        }

        void ShowCurrentGroup(List<Vector3> points)
        {
            ClearTestPoints();
            if (points.Count == 0) return;
            foreach (var point in points)
            {
                var obj = GetTestPoint();
                obj.transform.position = point;
            }
        }

        void CheckParents()
        {
            if (parentUnused == null)
            {
                var go1 = new GameObject("Unused");
                parentUnused = go1.transform;
            }

            if (parentTestPoints == null)
            {
                var go2 = new GameObject("Test Group");
                parentUnused = go2.transform;
            }
        }

        GameObject GetTestPoint()
        {
            CheckParents();
            GameObject pnt;
            if (unusedTestPointObjects.Count == 0)
            {
                pnt = Instantiate(testPointPrefab, parentTestPoints);
            }
            else
            {
                pnt = unusedTestPointObjects.Dequeue();
                pnt.transform.SetParent(parentTestPoints);
            }
            testPointObjects.Add(pnt);
            return pnt;
        }
        void ClearTestPoints()
        {
            foreach (var pointObject in testPointObjects)
            {
                pointObject.transform.SetParent(parentUnused);
                unusedTestPointObjects.Enqueue(pointObject);
            }
            testPointObjects.Clear();
        }
    }
}