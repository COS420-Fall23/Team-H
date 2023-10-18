using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private GridPoint[] _gridPoints = new GridPoint[2];
    private Transform _targetMarkerTip = null;
    
    private void Update()
    {
        if (_targetMarkerTip)
        {
            _lineRenderer.SetPosition(0, _gridPoints[0].transform.position);
            _lineRenderer.SetPosition(1, _targetMarkerTip.position);
        }
        else{
            for (int i = 0; i < _gridPoints.Length; i++)
            {
                _lineRenderer.SetPosition(i, _gridPoints[i].transform.position);
            }
        }
    }
    
    public void SetLineSegment_NoMarkerTip(GridPoint startPoint, GridPoint endPoint, float startLineWidth, float endLineWidth)
    {
        
        _lineRenderer.startWidth = startLineWidth;
        _lineRenderer.endWidth = endLineWidth;
        _gridPoints[0] = startPoint;
        _gridPoints[1] = endPoint;
        _targetMarkerTip = null;

    }
    public void SetLineSegment_WithMarkerTip(GridPoint startPoint, Transform markerTip, float startLineWidth, float endLineWidth)
    {
        
        _lineRenderer.startWidth = startLineWidth;
        _lineRenderer.endWidth = endLineWidth;
        _gridPoints[0] = startPoint;
        _targetMarkerTip = markerTip;

    }

    public GridPoint[] GetEndPoints()
    {
        return _gridPoints;
    }
    
}
