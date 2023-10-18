using System;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;

public class LatticeLineMarker : MonoBehaviour
{
    #region Variables

    [SerializeField] private LineSegment _lineSegmentPrefab;
    [SerializeField] private float _markerTipWidth;
    
    [Header("Debugging Fields - No Touchy")]
    [SerializeField] private bool _isDrawing;
    [SerializeField] private LineSegment _curLineSegment;
    [SerializeField] private GridPoint _curHoveringPoint;
    [SerializeField] private GridPoint _curSelectedPoint;

    public InputAction eraseAttachedLinesAction;
    
    #endregion

    private void OnEnable()
    {
        eraseAttachedLinesAction.Enable();
    }

    private void Update()
    {
        if (eraseAttachedLinesAction.triggered && _curHoveringPoint)
        {
            _curHoveringPoint.ClearLineSegments();
        }
    }


    #region Functions

    /// <summary>
    /// || TABLE OF CONTENTS ||
    ///
    /// Region 00: Input Functions
    ///     0.0 PrimaryAction()
    ///         Contextual logic of pulling the controllers trigger.
    ///             Selecting grids to add lines, or deselected current selection.
    ///     0.1 SecondaryAction()
    ///         Contextual logic of pressing "A" on the controller.
    ///             Clears all lines connected to the _curHoveringPoint.
    /// 
    /// Region 01: Collision Logic
    ///     1.0 OnTriggerEnter() 
    ///         Conditionally sets the state of the point currently hovering over to:
    ///             Tag Check: Do nothing if tag "NoCollisionOnSame". 
    ///             Deselect - If the GridPoint your hovering over is the same as the one you've selected
    ///             Hover - Else
    ///     1.1 OnTriggerExit()
    ///         Tells the GridPoint you just left to attempt to go Idle, unless leaving selected object.
    ///             This logic is handled by GridPoint.SetStateConditionally(newState).
    /// 
    /// Region 02: Line Drawing Logic
    ///     2.0 StartLineDraw(GridPoint targetGridPoint)
    ///         Begins line drawing process by making targetGridPoint selected and 
    ///             instantiating a LineSegment connected to the _curSelectedPoint and the _markerTip.
    ///     2.1 EndLineDraw(GridPoint targetGridPoint)
    ///         Finishes the line drawing process by disconnecting the _curLineSegment from the LatticeLineMarker,
    ///             and connects it to the end point selected.
    ///     2.2 DeselectGridPoint()
    ///         Ends the line drawing process without drawing a line.
    ///         
    /// </summary>
    

    #region Region 00: Input Functions

    // 0.0
    public void PrimaryAction()
    {
        if (_curHoveringPoint)
        {
            if(_curHoveringPoint == _curSelectedPoint)
            {
                DeselectGridPoint();
            }
            else if (_isDrawing)
            {
                EndLineDraw(_curHoveringPoint);
            }
            else
            {
                StartLineDraw(_curHoveringPoint);
            }
        }
    }

    // 0.1
    public void SecondaryAction()
    {
        if (_curHoveringPoint && _curHoveringPoint.IsAttachedToALine())
        {
            _curHoveringPoint.ClearLineSegments();
        }
    }
    

    #endregion

    
    #region Region 01: Collision Logic

    // 1.0
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Marker OnTriggerEnter()");
        if (other.TryGetComponent(out _curHoveringPoint))
        {
            if (_curHoveringPoint == _curSelectedPoint)
            {
                _curHoveringPoint.SetStateConditionally(GridPointStates.DeselectHover);
            }
            else
            {
                _curHoveringPoint.SetStateConditionally(GridPointStates.Hover);
            }
        }
    }
    
    // 1.1
    private void OnTriggerExit(Collider other)
    {
        if (_curHoveringPoint)
        {
            if (_curSelectedPoint)
            {
                if (_curHoveringPoint == _curSelectedPoint)
                {
                    _curHoveringPoint.SetStateConditionally(GridPointStates.Selected);
                    _curHoveringPoint = null;
                }
                else
                {
                    _curHoveringPoint.SetStateConditionally(GridPointStates.Idle);
                    _curHoveringPoint = null;
                }
            }
            else
            {
                _curHoveringPoint.SetStateConditionally(GridPointStates.Idle);
                _curHoveringPoint = null;
            }
        }
    }

    #endregion

    
    #region Region 02: Line Drawing Logic

    // 2.0 - [| Conditions of Calling in 0.0 |]
    private void StartLineDraw(GridPoint targetGridPoint)
    {
        _isDrawing = true;
        _curSelectedPoint = targetGridPoint;
        _curSelectedPoint.SetStateConditionally(GridPointStates.Selected);
        _curLineSegment = Instantiate(_lineSegmentPrefab, _curSelectedPoint.transform);
        _curLineSegment.SetLineSegment_WithMarkerTip(
            _curSelectedPoint,
            transform,
            _curSelectedPoint.transform.localScale.x/2, 
            _markerTipWidth);
        targetGridPoint.AddLineSegment(_curLineSegment);
    }

    // 2.1 - [| Conditions of Calling in 0.0 |]
    private void EndLineDraw(GridPoint targetGridPoint)
    {
        _isDrawing = false;
        targetGridPoint.SetStateConditionally(GridPointStates.Attached);
        targetGridPoint.AddLineSegment(_curLineSegment);

        _curLineSegment.SetLineSegment_NoMarkerTip(
            _curSelectedPoint, 
            _curHoveringPoint, 
            _curSelectedPoint.transform.localScale.x/2, 
            _curSelectedPoint.transform.localScale.x/2);
        _curLineSegment = null;
        
        _curSelectedPoint.SetStateConditionally(GridPointStates.Attached);
        _curSelectedPoint = null;
    }

    // 2.2 - [| Conditions of Calling in 0.0 |]
    private void DeselectGridPoint()
    {
        // Conditions:
        //      Trigger pull, hovering over point, that point is the same as the already selected point.
        _isDrawing = false;
        
        _curSelectedPoint.RemoveTargetLineFromList(_curLineSegment);
        _curSelectedPoint = null;
        
        Destroy(_curLineSegment.gameObject);
        _curLineSegment = null;
    }

    #endregion
    

    #endregion
}

#region Debugging Custom Editor

/*
[CustomEditor(typeof(LatticeLineMarker))]
public class LatticeMarkerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LatticeLineMarker latticeLineMarker = (LatticeLineMarker)target;
        if (GUILayout.Button("Pull Trigger"))
        {
            latticeLineMarker.PrimaryAction();
        }

        if (GUILayout.Button("Press A"))
        {
            latticeLineMarker.SecondaryAction();
        }
    }
}
*/

#endregion
