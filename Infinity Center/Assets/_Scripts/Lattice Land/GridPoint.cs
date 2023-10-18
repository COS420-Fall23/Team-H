using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEditor;

public enum GridPointStates
{
    Idle,
    Hover,
    Selected,
    Attached,
    DeselectHover
};

public class GridPoint : MonoBehaviour
{
    
    private int3 _gridPos;

    #region State Variables

    [SerializeField] private List<LineSegment> _attachedLineSegments;
    
    [Header("State Scale Ratios")]
    [SerializeField] private GridPointStates _curGridPointState = GridPointStates.Idle;
    [SerializeField] private float _hoverStateScaleChangeRatio = 1.2f;
    [SerializeField] private float _selectedStateScaleChangeRatio = 1.25f;
    [SerializeField] private float _attachedStateScaleChangeRatio = 1.15f;
    [Tooltip("Idle state scale is set on instantiation by the Lattice Grid.")]
    [SerializeField] private float _idleStateScale;
    
    [Header("State Materials")] 
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _idleStateMaterial;
    [SerializeField] private Material _hoverStateMaterial;
    [SerializeField] private Material _selectedStateMaterial;
    [SerializeField] private Material _attachedStateMaterial;
    [SerializeField] private Material _deselectMaterial;

    #endregion


    #region Functions

    public void AddLineSegment(LineSegment newLineSegment)
    {
        _attachedLineSegments.Add(newLineSegment);
    }

    public void ClearLineSegments()
    {
        if (_attachedLineSegments.Count > 0)
        {
            for (int i = _attachedLineSegments.Count -1; i >= 0; i--)
            {
                GridPoint[] targetLineSegmentPoints = _attachedLineSegments[i].GetEndPoints(); 
                if (targetLineSegmentPoints[0] != this)
                {
                    targetLineSegmentPoints[0].RemoveTargetLineFromList(_attachedLineSegments[i]);
                }
                if (targetLineSegmentPoints[1]!= this)
                {
                    targetLineSegmentPoints[1].RemoveTargetLineFromList(_attachedLineSegments[i]);   
                }
                LineSegment targetLine = _attachedLineSegments[i];
                _attachedLineSegments.RemoveAt(i);
                Destroy(targetLine.gameObject);
            }
        }
    }

    public void RemoveTargetLineFromList(LineSegment targetLine)
    {
        if (_attachedLineSegments.Contains(targetLine)) 
        {
            _attachedLineSegments.Remove(targetLine);
        }
        SetStateConditionally(GridPointStates.Idle);
    }
    
    #endregion
    
    #region Getters & Setters
    
    public void SetStateConditionally(GridPointStates newState)
    {
        // This traffics the logic of states, and then sets relevant variables respectably.
        switch (newState)
        {
            case GridPointStates.Idle:
                if (IsAttachedToALine())
                {
                    SetStateConditionally(GridPointStates.Attached);
                    break;
                }
                _curGridPointState = GridPointStates.Idle;
                SetScaleByState(GridPointStates.Idle);
                _meshRenderer.material = _idleStateMaterial;
                break;
                
            case GridPointStates.Hover:
                _curGridPointState = GridPointStates.Hover;
                SetScaleByState(GridPointStates.Hover);
                _meshRenderer.material = _hoverStateMaterial;
                break;
            case GridPointStates.Selected:
                _curGridPointState = GridPointStates.Selected;
                SetScaleByState(GridPointStates.Selected);
                _meshRenderer.material = _selectedStateMaterial;
                break;
            case GridPointStates.Attached:
                _curGridPointState = GridPointStates.Attached;
                SetScaleByState(GridPointStates.Attached);
                _meshRenderer.material = _attachedStateMaterial;
                break;
            case GridPointStates.DeselectHover:
                _curGridPointState = GridPointStates.DeselectHover;
                SetScaleByState(GridPointStates.DeselectHover);
                _meshRenderer.material = _deselectMaterial;
                break;
        }
    }
    
    private void SetScaleByState(GridPointStates state)
    {
        // This changes the scale of the point in ratio to the set idle scale based on its current state.
        // Idle state scale is set by the Lattice Grid.
        switch (state)
        {
            case GridPointStates.Idle:
                gameObject.transform.localScale = new Vector3(
                    _idleStateScale,
                    _idleStateScale,
                    _idleStateScale
                );
                break;
            case GridPointStates.Hover:
                gameObject.transform.localScale = new Vector3(
                    _idleStateScale * _hoverStateScaleChangeRatio,
                    _idleStateScale * _hoverStateScaleChangeRatio,
                    _idleStateScale * _hoverStateScaleChangeRatio);
                break;
            case GridPointStates.DeselectHover:
                gameObject.transform.localScale = new Vector3(
                    _idleStateScale * _hoverStateScaleChangeRatio,
                    _idleStateScale * _hoverStateScaleChangeRatio,
                    _idleStateScale * _hoverStateScaleChangeRatio
                );
                break;
            case GridPointStates.Attached:
                gameObject.transform.localScale = new Vector3(
                    _idleStateScale * _attachedStateScaleChangeRatio,
                    _idleStateScale * _attachedStateScaleChangeRatio,
                    _idleStateScale * _attachedStateScaleChangeRatio
                );
                break;
            case GridPointStates.Selected:
                gameObject.transform.localScale = new Vector3(
                    _idleStateScale * _selectedStateScaleChangeRatio,
                    _idleStateScale * _selectedStateScaleChangeRatio, 
                    _idleStateScale * _selectedStateScaleChangeRatio
                );
                break;
        }
    }
    
    public void SetGridPos(int x, int y, int z)
    {
        _gridPos.x = x;
        _gridPos.y = y;
        _gridPos.z = z;
    }

    public GridPointStates GetPointState()
    {
        return _curGridPointState;
    }

    public Vector3 GetGridWorldPosition()
    {
        return gameObject.transform.localToWorldMatrix.GetPosition();
    }

    public List<LineSegment> GetAttachedLineSegments()
    {
        return _attachedLineSegments;
    }
    public bool IsAttachedToALine()
    {
        if (_attachedLineSegments.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void SetIdleStateScale(float idleStateScale)
    {
        _idleStateScale = idleStateScale;
    }

    #endregion

    #region Debugging Custom Inspector
    
    /*
    [CustomEditor(typeof(GridPoint))]
    public class GridPointEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawHeader();
            
            GridPoint gridPoint = (GridPoint)target;
            if (GUILayout.Button("Set State to Idle"))
            {
                gridPoint.SetStateConditionally(GridPointStates.Idle);
            }
            if (GUILayout.Button("Set State to Hover"))
            {
                gridPoint.SetStateConditionally(GridPointStates.Hover);
            }
            if (GUILayout.Button("Set State to Selected"))
            {
                gridPoint.SetStateConditionally(GridPointStates.Selected);
            }

            if (GUILayout.Button("Set State to DeselectHover"))
            {
                gridPoint.SetStateConditionally(GridPointStates.DeselectHover);
            }
        }
    }
    */

    #endregion
}


