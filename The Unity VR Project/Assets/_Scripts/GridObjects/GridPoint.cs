using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace LatticeLand
{
    public class GridPoint : ColorableObject
    {
        #region | Variables |

        #region | State Variables |

        [Header("State Scale Ratios")] [SerializeField]
        private InteractableObjectState curState = InteractableObjectState.Idle;

        [SerializeField] private float _hoverStateScaleChangeRatio = 1.2f;
        [SerializeField] private float _selectedStateScaleChangeRatio = 1.25f;
        [SerializeField] private float _attachedStateScaleChangeRatio = 1.15f;

        [Tooltip("Idle state scale is set on instantiation by the Lattice Grid.")] [SerializeField]
        private float _idleStateScale;

        [Header("State Materials")] [SerializeField]
        private MeshRenderer _meshRenderer;

        [SerializeField] private Material _idleStateMaterial;
        [SerializeField] private Material _defaultIdleStateMaterial;
        [SerializeField] private Material _hoverStateMaterial;
        [SerializeField] private Material _selectedStateMaterial;
        [SerializeField] private Material _attachedStateMaterial;
        [SerializeField] private Material _deselectMaterial;

        #endregion

        [Header("Debugging")] [SerializeField] private bool _enableDebugLogs = true;
        [SerializeField] private Vector3Int _gridCoordinates;
        [SerializeField] private List<LineSegment> _attachedLineSegments;

        [SerializeField]
        public int referenceInt {
            get
            {
                return (int)(1000000 + _gridCoordinates.z + _gridCoordinates.y * 100 + _gridCoordinates.x * 10000);
            }
        }

        #endregion

        #region | All Methods |

        /* *********************
         * Table of Methods
         * *********************
         *
         *  | Unity Methods |
         *      OnEnable()
         * 
         *  | Custom Methods |
         *      AddLineSegment()
         *      ClearLineSegments()
         *      RemoveTargetLineFromList()
         *
         *  | Inherited Methods |
         *      SetStateConditionally()
         *      GetObjectType()
         *      ApplyColor()
         *      ResetColor()
         *
         *  | Getters & Setters |
         *      SetScaleByState()
         *      GetGridCoordinates()
         *      SetGridCoordinates()
         *      GetCurrentState()
         *      GetGridWorldPosition()
         *      GetAttachedLineSegments()
         *      IsAttachedToALine()
         *      IsAttachedLineFollowingMarker()
         *      SetIdleStateScale()
         *
         * **********************
         */

        #region | Unity Methods |

        private void OnEnable()
        {
            _objectType = InteractableGridObjectType.GridPoint;
            _defaultIdleStateMaterial = _idleStateMaterial;
        }

        #endregion

        #region | Custom Methods |

        public void Start()
        {
            //implicitly dimensions must be < 100
            gameObject.name = "p." + referenceInt;
        }
        public void AddLineSegment(LineSegment lineSegment)
        {
            _attachedLineSegments.Add(lineSegment);
        }

        public void ClearLineSegments()
        {
            if (_attachedLineSegments.Count > 0)
            {
                foreach (var line in _attachedLineSegments)
                {
                    line.Erase();
                }
            }
        }

        public void RemoveTargetLineFromList(LineSegment targetLine)
        {
            if (_attachedLineSegments.Contains(targetLine))
            {
                _attachedLineSegments.Remove(targetLine);
                _attachedLineSegments.TrimExcess();
                SetStateConditionally(InteractableObjectState.Idle);
            }
        }

        #endregion

        #region | Inherited Methods |

        public override void SetStateConditionally(InteractableObjectState newState)
        {
            // [ From >> NewLatticeGrid.ToggleHoverState >> GridPointState.Hover

            if (_enableDebugLogs)
            {
                Debug.Log("GridPoint.SetStateConditionally - NewState: " + newState);
            }

            switch (newState)
            {
                case InteractableObjectState.Idle:
                    if (_enableDebugLogs)
                    {
                        Debug.Log("GridPoint: Set State Idle");
                    }

                    if (IsAttachedLineFollowingMarker())
                    {
                        SetStateConditionally(InteractableObjectState.Selected);
                        break;
                    }

                    if (IsAttachedToALine())
                    {
                        SetStateConditionally(InteractableObjectState.Attached);
                        break;
                    }

                    curState = InteractableObjectState.Idle;
                    SetScaleByState(InteractableObjectState.Idle);
                    _meshRenderer.material = _idleStateMaterial;
                    break;

                case InteractableObjectState.EnterHover:
                    if (_enableDebugLogs)
                    {
                        Debug.Log("GridPoint: Set State Enter Hover");
                    }

                    if (curState == InteractableObjectState.Selected)
                    {
                        if (_enableDebugLogs)
                        {
                            Debug.Log("GridPoint: Trying To Set Enter Hover to Deselect Hover");
                        }

                        SetStateConditionally(InteractableObjectState.DeselectHover);
                        break;
                    }

                    SetStateConditionally(InteractableObjectState.Hover);
                    break;

                case InteractableObjectState.Hover:
                    if (_enableDebugLogs)
                    {
                        Debug.Log("GridPoint: Set State Hover");
                    }

                    curState = InteractableObjectState.Hover;
                    SetScaleByState(InteractableObjectState.Hover);
                    _meshRenderer.material = _hoverStateMaterial;
                    break;

                case InteractableObjectState.Selected:
                    if (_enableDebugLogs)
                    {
                        Debug.Log("GridPoint: Set State Selected");
                    }

                    curState = InteractableObjectState.Selected;
                    SetScaleByState(InteractableObjectState.Selected);
                    _meshRenderer.material = _selectedStateMaterial;
                    break;

                case InteractableObjectState.Attached:
                    if (_enableDebugLogs)
                    {
                        Debug.Log("GridPoint: Set State Attached");
                    }

                    curState = InteractableObjectState.Attached;
                    SetScaleByState(InteractableObjectState.Attached);
                    _meshRenderer.material = _attachedStateMaterial;
                    break;

                case InteractableObjectState.DeselectHover:
                    if (_enableDebugLogs)
                    {
                        Debug.Log("GridPoint: Set State Deselect Hover");
                    }

                    curState = InteractableObjectState.DeselectHover;
                    SetScaleByState(InteractableObjectState.DeselectHover);
                    _meshRenderer.material = _deselectMaterial;
                    break;
            }
        }

        public override void ApplyColorMaterial(Material newColor)
        {
            _idleStateMaterial = newColor;
            _attachedStateMaterial = newColor;
        }

        public override void ResetColorMaterial()
        {
            _idleStateMaterial = _defaultIdleStateMaterial;
        }

        #endregion

        #region | Getters & Setters |

        private void SetScaleByState(InteractableObjectState state)
        {
            switch (state)
            {
                case InteractableObjectState.Idle:
                    gameObject.transform.localScale = new Vector3(
                        _idleStateScale,
                        _idleStateScale,
                        _idleStateScale
                    );
                    break;
                case InteractableObjectState.Hover:
                    gameObject.transform.localScale = new Vector3(
                        _idleStateScale * _hoverStateScaleChangeRatio,
                        _idleStateScale * _hoverStateScaleChangeRatio,
                        _idleStateScale * _hoverStateScaleChangeRatio);
                    break;
                case InteractableObjectState.DeselectHover:
                    gameObject.transform.localScale = new Vector3(
                        _idleStateScale * _hoverStateScaleChangeRatio,
                        _idleStateScale * _hoverStateScaleChangeRatio,
                        _idleStateScale * _hoverStateScaleChangeRatio
                    );
                    break;
                case InteractableObjectState.Attached:
                    gameObject.transform.localScale = new Vector3(
                        _idleStateScale * _attachedStateScaleChangeRatio,
                        _idleStateScale * _attachedStateScaleChangeRatio,
                        _idleStateScale * _attachedStateScaleChangeRatio
                    );
                    break;
                case InteractableObjectState.Selected:
                    gameObject.transform.localScale = new Vector3(
                        _idleStateScale * _selectedStateScaleChangeRatio,
                        _idleStateScale * _selectedStateScaleChangeRatio,
                        _idleStateScale * _selectedStateScaleChangeRatio
                    );
                    break;
            }
        }

        public Vector3Int GetGridCoordinates()
        {
            return _gridCoordinates;
        }

        public void SetGridCoordinates(int x, int y, int z)
        {
            _gridCoordinates[0] = x;
            _gridCoordinates[1] = y;
            _gridCoordinates[2] = z;
        }

        public InteractableObjectState GetCurrentState()
        {
            return curState;
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

        private bool IsAttachedLineFollowingMarker()
        {
            if (_enableDebugLogs)
            {
                Debug.Log("GridPoint: IsAttachedLineFollowingMarker");
            }

            foreach (var line in _attachedLineSegments)
            {
                if (_enableDebugLogs)
                {
                    Debug.Log("Current Line: " + line + " IsFollowingMarker: " + line.GetIsFollowingMarker());
                }

                if (line.GetIsFollowingMarker())
                {
                    return true;
                }
            }

            return false;
        }

        public void SetIdleStateScale(float idleStateScale)
        {
            _idleStateScale = idleStateScale;
        }
        
        uint GetVector3IntHash(Vector3Int i)
        {
            int3 _newInt3 = new int3(i.x, i.y, i.z);
            return math.hash(_newInt3);
        }

        #endregion

        #endregion
    }
}