using System;
using System.Collections;
using System.Collections.Generic;
using LatticeLand;
using UnityEngine;

public class HashTest : MonoBehaviour
{
    public LineSegment _LineSegment;
    private Dictionary<int, LineSegment> _lineDic;


    private void Start()
    {
        _lineDic = new Dictionary<int, LineSegment>();
        LineSegment newLine = Instantiate(_LineSegment);
        LineSegment newLine1 = Instantiate(_LineSegment);
        LineSegment newLine2 = Instantiate(_LineSegment);

        Debug.Log("Line Name: " + newLine.name);
        Debug.Log("Line Name: " + newLine1.name);
        Debug.Log("Line Name: " + newLine2.name);
        
        _lineDic.Add(newLine.GetHashCode(), newLine);
        _lineDic.Add(newLine1.GetHashCode(), newLine1);
        _lineDic.Add(newLine2.GetHashCode(), newLine2);


    }
}
