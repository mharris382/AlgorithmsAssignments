using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Assignment1;

public class BinaryHeapTests
{
    public void VerifyTestArray()
    {
        var actualArray = GetTestArray(5);
        var expectedArray = new (char, int)[5]
        {
            ('A',65),
            ('B', 66),
            ('C', 67),
            ('D', 68),
            ('E', 69)
        };
        for (int i = 0; i < 5; i++)
        {
            var actual = actualArray[i];
            var expect = expectedArray[i];
            Assert.AreEqual(expect.Item1, actual.Item1 );
            Assert.AreEqual(expect.Item2, actual.Item2 );
        }
    }
    
    public void TestStartHeap()
    {
        var testArray = GetTestArray(5);
        var bheap = new BinaryHeap<string>();
        bheap.StartHeap(5);
    }

    public void TestHeapEmpty()
    {
        
    }

    public void TestGetMin()
    {
        
    }

    public void TestHeapInsert()
    {
        
    }

    public void TestHeapDelete()
    {
        
    }

    (char,int)[] GetTestArray(int size)
    {
        var letters = size;
        var letter = 'A';
        var alphabet = new (char, int)[letters];
        for (int i = 0; i < letters; i++)
        {
            var key = (int) letter;
            alphabet[i] = (letter, key);
            letter = (char) (key + 1);
        }
        return alphabet;
    }

    (char, int) GetLetterKey(char letter) => (letter, (int) letter);
}
