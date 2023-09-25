using FishNet;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LatticeLand
{
    public class ColoringTool : MonoBehaviour, ILatticeTool
    {
        #region | Variables |

        private InteractionCommandBroadcast _interactionCommandMsg;
        private bool _isServer;

        [Header("Input Action Events")] 
        public InputAction chooseColor;
        public InputAction applyColor;
        public InputAction switchTool;
        [SerializeField] private LineMarker _lineTool;

        [Header("Colors")] 
        [SerializeField] private Material[] _colorMaterial = new Material[6];
        [SerializeField] private MeshRenderer _tipMesh;
        private MeshRenderer _meshRenderer;

        [Header("Debugging")] 
        [SerializeField] private bool _enableDebugLogs;
        [SerializeField] private int _curColorIndex;
        [SerializeField] private ColorableObject _curHoveringColorableObject;

        #endregion

        #region | All Methods |

        /* *********************
         * Table of Methods
         * *********************
         *
         *  | Unity Methods |
         *      Awake()
         *      OnEnable()
         *      Update()
         *
         *  | Input Methods |
         *      CycleColor()
         *      ApplyColor()
         *      SwitchTool()
         *
         *  | Inherited Methods |
         *      TriggerEnter()
         *      TriggerExit()
         *
         * **********************
         */

        #region | Unity Methods |

        private void Awake()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _curColorIndex = 0;
        }

        private void OnEnable()
        {

            if (InstanceFinder.IsServer)
            {
                _isServer = true;
            }
            if (!_isServer)
            {
                applyColor.Enable();
                chooseColor.Enable();
                switchTool.Enable();
            }

            _tipMesh.material = _colorMaterial[_curColorIndex];
        }

        private void Update()
        {
            if (chooseColor.triggered)
            {
                CycleColor();
            }

            if (applyColor.triggered)
            {
                ApplyColor();
            }

            if (switchTool.triggered)
            {
                if (_enableDebugLogs)
                {
                    Debug.Log("ColoringTool: SwitchTool.Triggered");
                }

                SwitchTool();
            }
        }

        #endregion

        #region | Input Methods |

        public void CycleColor()
        {
            if (_enableDebugLogs)
            {
                Debug.Log("ColoringTool: Cycling Color");
            }

            if (_curColorIndex < _colorMaterial.Length - 1)
            {
                _curColorIndex++;
            }
            else
            {
                _curColorIndex = 0;
            }

            _tipMesh.material = _colorMaterial[_curColorIndex];
        }

        public void ApplyColor()
        {
            if (_enableDebugLogs)
            {
                Debug.Log("ColoringTool: Applying Color");
            }

            if (_curHoveringColorableObject)
            {
                _interactionCommandMsg.command = LatticeGridCommand.ApplyColor;
                _interactionCommandMsg.colorMaterialIndex = _curColorIndex;

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
            if (!_lineTool.gameObject.activeInHierarchy)
            {
                if (_enableDebugLogs)
                {
                    Debug.Log("ColoringTool: Switch Tool");
                }

                _lineTool.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        #endregion

        #region | Inherited Methods |

        public void TriggerEnter(Collider other)
        {
            if (_enableDebugLogs)
            {
                Debug.Log("ColoringTool: TriggerEnter Called");
            }

            if (!_curHoveringColorableObject && other.TryGetComponent(out _curHoveringColorableObject))
            {
                _interactionCommandMsg.command = LatticeGridCommand.EnterHover;


                if (_curHoveringColorableObject.GetObjectType() == InteractableGridObjectType.GridPoint)
                {
                    // GridPoint Hover
                    _interactionCommandMsg.InteractableGridObjectType = InteractableGridObjectType.GridPoint;
                    _interactionCommandMsg.gridPointCoordinates =
                        _curHoveringColorableObject.GetComponent<GridPoint>().GetGridCoordinates();

                    // Null Other
                    _interactionCommandMsg.lineSegmentName = null;
                }

                else if (_curHoveringColorableObject.GetObjectType() == InteractableGridObjectType.LineSegment)
                {
                    // LineSegment Hover
                    _interactionCommandMsg.InteractableGridObjectType = InteractableGridObjectType.LineSegment;
                    _interactionCommandMsg.lineSegmentName = _curHoveringColorableObject.name;
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
            if (other.TryGetComponent(out _curHoveringColorableObject))
            {
                if (_enableDebugLogs)
                {
                    Debug.Log("ColoringTool: TriggerExit Called");
                }

                _interactionCommandMsg.command = LatticeGridCommand.ExitHover;

                if (_curHoveringColorableObject.GetObjectType() == InteractableGridObjectType.GridPoint)
                {
                    // GridPoint Hover
                    _interactionCommandMsg.InteractableGridObjectType = InteractableGridObjectType.GridPoint;
                    _interactionCommandMsg.gridPointCoordinates =
                        _curHoveringColorableObject.GetComponent<GridPoint>().GetGridCoordinates();

                    // Null Other
                    _interactionCommandMsg.lineSegmentName = null;
                }

                else if (_curHoveringColorableObject.GetObjectType() == InteractableGridObjectType.LineSegment)
                {
                    // LineSegment Hover
                    _interactionCommandMsg.InteractableGridObjectType = InteractableGridObjectType.LineSegment;
                    _interactionCommandMsg.lineSegmentName = _curHoveringColorableObject.name;
                }

                if (_isServer)
                {
                    InstanceFinder.ServerManager.Broadcast(_interactionCommandMsg);
                }
                else
                {
                    InstanceFinder.ClientManager.Broadcast(_interactionCommandMsg);
                }

                _curHoveringColorableObject = null;
            }
        }

        #endregion

        #endregion
    }
}