using System;
using NUnit.Framework;
using UnityEngine;
using static Assignment4.Quickhull;

public class SegmentTests
{
    
}


public class DistanceTests
{

    const float epsilon = 0.1f;
    
    [Test]
    public void CorrectDistanceWithVerticalSegment()
    {
        var p = new Vector2(3, 3);
        var q = new Vector2(3, 6);
        var c = new Vector2(4, 5);
        float expectedDistance = 1;
        LineSegment segment = new LineSegment(p, q);
        var distance = segment.DistanceTo(c);
        Assert.AreEqual(expectedDistance, distance, epsilon);
    }
    [Test]
    public void CorrectDistanceWithHorizontalSegment()
    {
        var p = new Vector2(5, 6);
        var q = new Vector2(3, 6);
        var c = new Vector2(4, 4);
        float expectedDistance = 2;
        LineSegment segment = new LineSegment(p, q);
        var distance = segment.DistanceTo(c);
        Assert.AreEqual(expectedDistance, distance, epsilon);
    }
    [Test]
    public void CorrectDistanceFromAboveSlope()
    {
        var p = new Vector2(4, 3);
        var q = new Vector2(3, 6);
        var c = new Vector2(4, 5);
        
        float expectedDistance = 1.26491106407f;
        
        LineSegment segment = new LineSegment(p, q);
        var distance = segment.DistanceTo(c);
        Assert.AreEqual(expectedDistance, distance, epsilon);
        
    }
    [Test]
    public void CorrectDistanceFromBelowSlope()
    {
        var p = new Vector2(4, 3);
        var q = new Vector2(3, 6);
        var c = new Vector2(3, 2);
        
        float expectedDistance = -1.8973665961f;
        
        LineSegment segment = new LineSegment(p, q);
        var distance = segment.DistanceTo(c);
        Assert.AreEqual(expectedDistance, distance, epsilon);
    }

    
}