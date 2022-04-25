using System;
using System.Collections.Generic;
using Assignment1;
using NUnit.Framework;
using UnityEngine;

public class PriorityQueueTests
{
    [Test]
    public void CreateNewPriorityQueue()
    {
        var pq = new PriorityQueue<Char>();
        pq.InitCapacity(1);
        Assert.IsTrue(pq.Enqueue('A', 4));
        Assert.AreEqual('A',pq.ExtractMin());
    }

    [Test]
    public void CannotAddToQueueWhenFull()
    {
        var pq = new PriorityQueue<Char>();
        pq.InitCapacity(1);
        Assert.IsTrue(pq.Enqueue('A', 4));
        Assert.IsFalse(pq.Enqueue('A', 4));
    }

    [Test]
    public void ReturnsDefaultIfEmpty()
    {
        var pq = new PriorityQueue<Char>();
        pq.InitCapacity(1);
        Assert.IsTrue(pq.Enqueue('A', 4));
        Assert.AreEqual('A',pq.ExtractMin());
        Assert.AreEqual((char)0, pq.ExtractMin());
    }

    [Test]
    public void ExtractCorrectMin()
    {
        var pq = new PriorityQueue<Char>();
        pq.InitCapacity(6);
        Assert.IsTrue(pq.Enqueue('B', 5));
        Debug.Log(pq.ToString());
        Assert.IsTrue(pq.Enqueue('A', 4));
        Debug.Log(pq.ToString());
        Assert.IsTrue(pq.Enqueue('C', 6));
        Debug.Log(pq.ToString());
        Assert.IsTrue(pq.Enqueue('D', 7));
        Debug.Log(pq.ToString());
        Assert.IsTrue(pq.Enqueue('E', 8));
        Debug.Log(pq.ToString());
        Assert.AreEqual('A', pq.ExtractMin());
        Debug.Log(pq.ToString());
        Assert.AreEqual('B', pq.ExtractMin());
        Debug.Log(pq.ToString());
        Assert.AreEqual('C', pq.ExtractMin());
        Debug.Log(pq.ToString());
        Assert.AreEqual('D', pq.ExtractMin());
        Debug.Log(pq.ToString());
        Assert.AreEqual('E', pq.ExtractMin());
    }
    
    
    [Test]
    public void DeletesElementsCorrectly()
    {
        var pq = new PriorityQueue<Char>();
        pq.InitCapacity(3);
        Assert.IsTrue(pq.Enqueue('B', 5));
        Debug.Log(pq.ToString());
        Assert.IsTrue(pq.Enqueue('A', 4));
        Debug.Log(pq.ToString());
        Assert.IsTrue(pq.Enqueue('C', 6));
        Debug.Log(pq.ToString());
        Assert.AreEqual(3, pq.Count);
        Debug.Log(pq.ToString());
        pq.Remove('B');
        Debug.Log(pq.ToString());
        Assert.AreEqual(2, pq.Count);
        Assert.AreEqual('A', pq.ExtractMin());
        Debug.Log(pq.ToString());
        Assert.AreEqual(1, pq.Count);
        Assert.AreEqual('C', pq.ExtractMin());
        Assert.AreEqual(0, pq.Count);
    }
    [Test]
    public void TestPriorityQueueOrder()
    {
        var pq = new PriorityQueue<Char>();
        List<(char, int)> testValues = new List<(char, int)>();
        char[] testInputOrder = new char[] {'B', 'D', 'C', 'E', 'A'};
        char[] testOutputOrder = new char[] {'A', 'B', 'C', 'D', 'E'};
        
        pq.InitCapacity(testInputOrder.Length);
        
        for (int i = 0; i < testInputOrder.Length; i++) 
            Assert.IsTrue(pq.Enqueue(testInputOrder[i], (int) testInputOrder[i]), "Failed to Insert all test elements");

        for (int i = 0; i < testOutputOrder.Length; i++) 
            Assert.AreEqual(testOutputOrder[i], pq.ExtractMin());
    }

    int GetIndexOfExpectedMin(List<(char, int)> values)
    {
        int minIndex = -1;
        int min =int.MaxValue;
        for (int i = 0; i < values.Count; i++)
        {
            var tuple = values[i];
            if (tuple.Item2 < min)
            {
                min = tuple.Item2;
                minIndex = i;
            }
        }
        return minIndex;
    }
    
    (char, int) GetLetterKey(char letter) => (letter, (int) letter);
}