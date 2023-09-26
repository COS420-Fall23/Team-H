using FishNet;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LatticeLand
{
    public class LineMarker : MonoBehaviour, ILatticeTool
    {
        #region | Variables |

        // Networking
        private bool _isServer;
        private InteractionCommandBroadcast _interactionCommandMsg;
        private MarkerBroadcast _markerMsg;
        private LineSegment _curLineSegment;

        [Header("Marker Tip Info")] 
        [SerializeField] private Transform _markerTip;

        [Header("Input Action Events")] 
        public InputAction eraseAction;
        public InputAction drawAction;
        public InputAction switchTool;
        [SerializeField] private ColoringTool _coloringTool;

        [Header("Debugging")] 
        [SerializeField] private bool _enableDebugLogs = true;
        [SerializeField] private ColorableObject _curHoveringObject;
        [SerializeField] private GridPoint _curSelectedPoint;

        #endregion

        #region | All Methods |

        /* *********************
         * Table of Methods
         * *********************
         *
         *  | Unity Methods |
         *      OnEnable()
         *      Update()
         *
         *  | Input Methods |
         *      DrawAction()
         *      EraseAction()
         *      SwitchTool()
         *
         *  | Inherited Methods |
         *      TriggerEnter()
         *      TriggerExit()
         *
         * **********************
         */

        #region | Unity Methods |

        private void OnEnable()
        {
            if (InstanceFinder.IsServer)
            {
                _isServer = true;
            }

            if (!_isServer)
            {
                eraseAction.Enable();
                drawAction.Enable();
                switchTool.Enable();
            }
            _interactionCommandMsg.InteractableGridObjectType = InteractableGridObjectType.GridPoint;

        }

        private void Update()
        {
            if (eraseAction.triggered)
            {
                if (_enableDebugLogs)
                {
                    Debug.Log("LineMarker: EraseAction.Triggered");
                }

                EraseAction();
            }

            if (drawAction.triggered)
            {
                if (_enableDebugLogs)
                {
                    Debug.Log("LineMarker: DrawAction.Triggered");
                }

                DrawAction();
            }

            if (switchTool.triggered)
            {
                if (_enableDebugLogs)
                {
                    Debug.Log("LineMarker: SwitchTool.Triggered");
                }

                SwitchTool();
            }

            if (_markerMsg.isDrawing)
            {
                _markerMsg.markerTipPosition = _markerTip.position;


                if (_isServer)
                {
                    InstanceFinder.ServerManager.Broadcast(_markerMsg);
                }
                else
                {
                    InstanceFinder.ClientManager.Broadcast(_markerMsg);
                }
            }
        }

        #endregion

        #region | Input Methods |

        public void DrawAction()
        {
            if (_enableDebugLogs)
            {
                Debug.Log("Marker: Trigger Pulled");
            }

            if (_curHoveringObject)
            {
                if (_curHoveringObject.GetObjectType() == InteractableGridObjectType.GridPoint)
                {
                    // Deselect Condition
                    if (_curHoveringObject == _curSelectedPoint)
                    {
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Marker: Deselect Condition");
                        }

                        _interactionCommandMsg.command = LatticeGridCommand.Deselect;
                        _markerMsg.isDrawing = false;
                        _curSelectedPoint = null;
                    }

                    // End Line Command
                    else if (_markerMsg.isDrawing)
                    {
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Marker: End Line");
                        }

                        _markerMsg.isDrawing = false;
                        _interactionCommandMsg.command = LatticeGridCommand.EndLine;
                        _curSelectedPoint = null;
                    }

                    // Start Line Command
                    else
                    {
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Marker: Start Line");
                        }

                        _markerMsg.isDrawing = true;

                        _curSelectedPoint = _curHoveringObject.GetComponent<GridPoint>();

                        // vv Added

                        //_curLineSegment = Instantiate(_linePrefab, Vector3.zero, quaternion.identity);

                        //_curLineSegment.StartLineSegment("LineSegment(" + _curLineSegment.GetHashCode() + ")", _curSelectedPoint,
                        //     _curSelectedPoint.transform.localScale.x / 2, .002f);
                        _interactionCommandMsg.lineSegmentName = new string($"LineSegment({Time.time.GetHashCode()})");
                        // ^^

                        _interactionCommandMsg.command = LatticeGridCommand.StartLine;
                        _interactionCommandMsg.gridPointCoordinates = _curSelectedPoint.GetGridCoordinates();
                    }
                }
                else if (_curHoveringObject.GetObjectType() == InteractableGridObjectType.LineSegment)
                {
                    // Can implement behavior here: Trigger pull while hovering over a LineSegment
                    // - Redraw? (Detach from closest endpoint) 
                }


                if (_isServer)
                {
                    InstanceFinder.ServerManager.Broadcast(_interactionCommandMsg);
                }
                else
                {
                    InstanceFinder.ClientManager.Broadcast(_interactionCommandMsg);
                }
            }
        }

        public void EraseAction()
        {
            if (_curHoveringObject && !_curSelectedPoint)
            {
                _interactionCommandMsg.command = LatticeGridCommand.Erase;

                if (_curHoveringObject.GetObjectType() == InteractableGridObjectType.GridPoint)
                {
                    _interactionCommandMsg.InteractableGridObjectType = InteractableGridObjectType.GridPoint;
                    // _interactionCommandMsg.gridPointCoordinates = _curHoveringObject.GetComponent<GridPoint>().GetGridCoordinates();
                }

                if (_curHoveringObject.GetObjectType() == InteractableGridObjectType.LineSegment)
                {
                    _interactionCommandMsg.InteractableGridObjectType = InteractableGridObjectType.LineSegment;
                    //_interactionCommandMsg.lineSegmentName = _curHoveringObject.name;
                }

                if (_isServer)
                {
                    InstanceFinder.ServerManager.Broadcast(_interactionCommandMsg);
                }
                else
                {
                    InstanceFinder.ClientManager.Broadcast(_interactionCommandMsg);
                }
            }
        }

        public void SwitchTool()
        {
            if (!_coloringTool.gameObject.activeInHierarchy)
            {
                if (_enableDebugLogs)
                {
                    Debug.Log("LineMarker: Switch Tool");
                }

                _coloringTool.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        #endregion

        #region | Inherited Methods |

        public void TriggerEnter(Collider other)
        {
            if (!_curHoveringObject && other.TryGetComponent(out _curHoveringObject)) // Might have to nest conditional
            {
                _interactionCommandMsg.command = LatticeGridCommand.EnterHover;

                if (_curHoveringObject.GetObjectType() == InteractableGridObjectType.GridPoint)
                {
                    _interactionCommandMsg.InteractableGridObjectType = InteractableGridObjectType.GridPoint;
                    _interactionCommandMsg.gridPointCoordinates =
                        _curHoveringObject.GetComponent<GridPoint>().GetGridCoordinates();
                }
                else if (_curHoveringObject.GetObjectType() == InteractableGridObjectType.LineSegment)
                {
                    _interactionCommandMsg.InteractableGridObjectType = InteractableGridObjectType.LineSegment;
                    _interactionCommandMsg.lineSegmentName = _curHoveringObject.name;
                }

                if (_isServer)
                {
                    InstanceFinder.ServerManager.Broadcast(_interactionCommandMsg);
                }
                else
                {
                    InstanceFinder.ClientManager.Broadcast(_interactionCommandMsg);
                }
            }
        }

        public void TriggerExit(Collider other)
        {
            ColorableObject tempObj;

            if (other.TryGetComponent(out tempObj) && tempObj == _curHoveringObject)
            {
                _interactionCommandMsg.command = LatticeGridCommand.ExitHover;

                if (_curHoveringObject.GetObjectType() == InteractableGridObjectType.GridPoint)
                {
                    _interactionCommandMsg.InteractableGridObjectType = InteractableGridObjectType.GridPoint;
                    _interactionCommandMsg.gridPointCoordinates =
                        _curHoveringObject.GetComponent<GridPoint>().GetGridCoordinates();
                }

                else if (_curHoveringObject.GetObjectType() == InteractableGridObjectType.LineSegment)
                {
                    _interactionCommandMsg.InteractableGridObjectType = InteractableGridObjectType.LineSegment;
                    _interactionCommandMsg.lineSegmentName = _curHoveringObject.name;
                }

                if (_enableDebugLogs)
                {
                    Debug.Log("Marker TriggerExit: " + _interactionCommandMsg.command);
                }

                if (_isServer)
                {
                    InstanceFinder.ServerManager.Broadcast(_interactionCommandMsg);
                }
                else
                {
                    InstanceFinder.ClientManager.Broadcast(_interactionCommandMsg);
                }

                _curHoveringObject = null;
            }
        }

        #endregion

        #endregion
    }
}