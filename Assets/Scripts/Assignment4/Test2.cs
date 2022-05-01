using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment4
{
    public class Test2 : MonoBehaviour
    {
        public Vector3[] points;

        public bool testStarted;
        [Header("Generate Points")]
        public float r = 10;
        public int cnt = 20;


        [ContextMenu(("Generate Points"))]
        public void GenerateNewSet()
        {
            testStarted = false;
            points = new Vector3[cnt];
            for (int i = 0; i < cnt; i++)
            {
                var center = transform.position;
                var rx = UnityEngine.Random.Range(-r, r);
                var ry = UnityEngine.Random.Range(-r, r);
                points[i] = new Vector3(rx, ry);
            }
        }

        private bool next = false;
        [ContextMenu("Next Step")]
        public void MoveToNextStep()
        {
            if(testStarted)
                next = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                testStarted = false;
                StopAllCoroutines();
            }
            if (Input.GetKeyDown(KeyCode.S) && !testStarted)
            {
                StartTest();
            }
            if (Input.GetKeyDown(KeyCode.Space) && testStarted)
            {
                next = true;
            }
        }

        [ContextMenu("Start Test")]
        public void StartTest()
        {
            testStarted = true;
            StartCoroutine(ShowSteps());
        }
        
        QuickHullSteps.Step step = null, prev = null;
        IEnumerator ShowSteps()
        {
            var pnts = new List<Vector3>(points);
            var result = QuickHullSteps.GetSteps(pnts);
            var first = result.Item1;
            var recursive = result.Item2;
            Debug.Log($"Solution took <b>{recursive.Count}</b> recursive steps to solve.");
            Queue<QuickHullSteps.Step> steps = new Queue<QuickHullSteps.Step>();
            
            foreach (var recursiveStep in recursive)
            {
                steps.Enqueue(recursiveStep);
            }

            step = first;
            int stepNumber = 0;
            while (steps.Count > 0)
            {
                Debug.Log($"Showing Step #{stepNumber}");
                
                if (next)
                {
                    next = false;
                    prev = step;
                    stepNumber++;
                    step = steps.Dequeue();
                }

                yield return null;

            }
            step.DrawSegment(Color.blue, Color.red, 0.5f, withArrow:false);
        }

        private void OnDrawGizmos()
        {
            if (testStarted)
            {
                var c1 = Color.blue;
                var c2 = Color.red;
                var c3 = Color.yellow;
                var c4 = Color.green;
                var c5 = Color.magenta;
                if (step != null)
                {
                    step.DrawSegment(c1, c2, 0.5f, withArrow:false);
                    if(step is QuickHullSteps.RecursiveStep rStep)
                        rStep.DrawFurthestPoint(c3);
                    step.DrawS1(c4);
                    step.DrawS2(c5);
                }
                if (prev != null)
                {
                    c1.a = c2.a = c3.a = c4.a = c5.a = 0.25f;
                    step.DrawSegment(c1, c2, 0.45f, false);
                    if (prev is QuickHullSteps.RecursiveStep rStep2) rStep2.DrawFurthestPoint(c3);
                    step.DrawS1(c4); step.DrawS2(c5);
                }
            }
            else
            {
                foreach (var point in points)
                {
                    Gizmos.color = Color.Lerp(Color.white, Color.red, 0.35f);
                    Gizmos.DrawWireSphere( point,  0.5f);
                }
            }
        }
    }
}