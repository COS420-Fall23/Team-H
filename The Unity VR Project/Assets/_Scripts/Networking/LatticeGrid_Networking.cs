using System;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace LatticeLand
{
    public class LatticeGrid_Networking : NetworkBehaviour
    {
        #region | Variables |

        private LatticeGrid _latticeGrid;

        [Header("Debugging")] [SerializeField] private bool _enableDebugLogs = true;

        #endregion

        #region | All Methods |

        /* *********************
         * Table of Methods
         * *********************
         *
         *  | Unity Methods |
         *      Awake()
         *      OnEnable()
         *      OnDisable()
         *
         *  | Broadcast Methods |
         *      | Marker Broadcasts |
         *          OnServerSend()
         *          OnMarkerBroadcast_Client()
         *          ExecuteMarkerBroadcast()
         *
         *      | Interaction Command Broadcasts |
         *          OnClientRequestCommandBroadcast()
         *          OnServerSendCommandBroadcast()
         *          ExecuteInteractionCommand()
         *
         * **********************
         */

        #region | Unity Methods |

        private void Awake()
        {
            _latticeGrid = GetComponent<LatticeGrid>();
        }

        private void OnEnable()
        {
            // Interaction Command Reg
            InstanceFinder.ClientManager.RegisterBroadcast<InteractionCommandBroadcast>(
                OnServerSendCommandBroadcast);
            InstanceFinder.ServerManager.RegisterBroadcast<InteractionCommandBroadcast>(
                OnClientRequestCommandBroadcast);

            // Marker Reg
            InstanceFinder.ClientManager.RegisterBroadcast<MarkerBroadcast>(OnServerSendMarkerBroadcast);
            InstanceFinder.ServerManager.RegisterBroadcast<MarkerBroadcast>(OnClientRequestMarkerBroadcast);

            gameObject.name += OwnerId;
        }

        private void OnDisable()
        {
            // Grid Point Unregister
            InstanceFinder.ClientManager.UnregisterBroadcast<InteractionCommandBroadcast>(
                OnServerSendCommandBroadcast);
            InstanceFinder.ServerManager.UnregisterBroadcast<InteractionCommandBroadcast>(
                OnClientRequestCommandBroadcast);

            // marker Unreg
            InstanceFinder.ClientManager.UnregisterBroadcast<MarkerBroadcast>(OnServerSendMarkerBroadcast);
            InstanceFinder.ServerManager.UnregisterBroadcast<MarkerBroadcast>(OnClientRequestMarkerBroadcast);
        }

        #endregion

        #region | Broadcast Methods |

        #region | Marker Broadcasts |

        // Marker broadcast is to allow the LineSegment to follow marker tip while drawing.
        // Maybe this should be converted to an RPC structure.

        private void OnClientRequestMarkerBroadcast(NetworkConnection conn, MarkerBroadcast msg)
        {
            InstanceFinder.ServerManager.Broadcast(msg);
        }

        public void OnServerSendMarkerBroadcast(MarkerBroadcast msg)
        {
            ExecuteMarkerBroadcast(msg);
        }

        public void ExecuteMarkerBroadcast(MarkerBroadcast msg)
        {
            if (_enableDebugLogs)
            {
                Debug.Log("LG_Network: OnMarkerBroadcast called\n - IsDrawing: " + msg.isDrawing);
            }

            if (msg.isDrawing)
            {
                _latticeGrid.FollowMarkerTip(msg.markerTipPosition);
                // [[ Go To >> LatticeGrid.FollowMarkerTip
            }
        }

        #endregion

        #region | Interaction Command Broadcasts |

        private void OnClientRequestCommandBroadcast(NetworkConnection conn, InteractionCommandBroadcast msg)
        {
            InstanceFinder.ServerManager.Broadcast(msg);
        }

        public void OnServerSendCommandBroadcast(InteractionCommandBroadcast msg)
        {
            ExecuteInteractionCommand(msg);
        }

        public void ExecuteInteractionCommand(InteractionCommandBroadcast msg)
        {
            if (msg.InteractableGridObjectType == InteractableGridObjectType.GridPoint)
            {
                GridPoint targetGP = _latticeGrid.GetTargetGridPoint(msg.gridPointCoordinates);

                switch (msg.command)
                {
                    // ## GridPoint - Enter Hover
                    case LatticeGridCommand.EnterHover:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (GridPoint): Enter Hover Command Triggered");
                        }

                        targetGP.SetStateConditionally(InteractableObjectState.EnterHover);
                        break;

                    // ## GridPoint - Exit Hover
                    case LatticeGridCommand.ExitHover:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (GridPoint): Exit Hover Command Triggered");
                        }

                        targetGP.SetStateConditionally(InteractableObjectState.Idle);
                        break;

                    // ## GridPoint - Start Line
                    case LatticeGridCommand.StartLine:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (GridPoint): Start Line Command Triggered");
                        }

                        _latticeGrid.StartLineDraw(msg.lineSegmentName, targetGP);

                        // [[ Go To >> LatticeGrid.StartLineDraw
                        break;

                    // ## GridPoint - End Line 
                    case LatticeGridCommand.EndLine:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (GridPoint): End Line Command Triggered");
                        }

                        _latticeGrid.EndLineDraw(targetGP);
                        // [[ Go To >> LatticeGrid.EndLine
                        break;

                    // ## GridPoint - Deselect
                    case LatticeGridCommand.Deselect:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (GridPoint): Deselect Command Triggered");
                        }

                        _latticeGrid.DeselectGridPoint(targetGP);
                        break;

                    // ## GridPoint - Erase
                    case LatticeGridCommand.Erase:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (GridPoint): Erase Attached Lines Command Triggered");
                        }

                        foreach (var line in targetGP.GetAttachedLineSegments())
                        {
                            _latticeGrid.EraseTargetLine(line);
                        }

                        break;

                    // ## Deselect Hover
                    case LatticeGridCommand.DeselectHover:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (GridPoint): Deselect Hover Command Triggered");
                        }

                        targetGP.SetStateConditionally(InteractableObjectState.DeselectHover);
                        break;

                    // ## Apply Color
                    case LatticeGridCommand.ApplyColor:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (GridPoint): Apply Color Command Triggered");
                        }

                        _latticeGrid.ApplyColorMaterial(msg.colorMaterialIndex, targetGP);
                        targetGP.SetStateConditionally(InteractableObjectState.Idle);
                        break;

                    // ## Reset Color
                    case LatticeGridCommand.ResetColor:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (GridPoint): Reset Color Command Triggered");
                        }

                        targetGP.ResetColorMaterial();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            else if (msg.InteractableGridObjectType == InteractableGridObjectType.LineSegment)
            {
                LineSegment targetLineSegment = _latticeGrid.GetTargetLineSegment(msg.lineSegmentName);

                switch (msg.command)
                {
                    // ## LineSegment - Enter Hover
                    case LatticeGridCommand.EnterHover:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (LineSegment): Enter Hover Command Triggered");
                        }

                        targetLineSegment.SetStateConditionally(InteractableObjectState.EnterHover);
                        break;

                    // ## LineSegment - Exit Hover
                    case LatticeGridCommand.ExitHover:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (LineSegment): Exit Hover Command Triggered");
                        }

                        targetLineSegment.SetStateConditionally(InteractableObjectState.Idle);
                        break;

                    case LatticeGridCommand.Erase:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (LineSegment): Erase Command Triggered");
                        }

                        _latticeGrid.EraseTargetLine(targetLineSegment);
                        break;

                    // ## LineSegment - Apply Color
                    case LatticeGridCommand.ApplyColor:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (LineSegment): Apply Color Command Triggered");
                        }

                        _latticeGrid.ApplyColorMaterial(msg.colorMaterialIndex, targetLineSegment);
                        targetLineSegment.SetStateConditionally(InteractableObjectState.Idle);
                        break;

                    // ## LineSegment - Reset Color
                    case LatticeGridCommand.ResetColor:
                        if (_enableDebugLogs)
                        {
                            Debug.Log("Network (LineSegment): Reset Color Command Triggered");
                        }

                        targetLineSegment.ResetColorMaterial();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion

        #endregion

        #endregion
    }
}