using System;
using Unity.Mathematics;
using UnityEngine;

namespace LatticeLand
{
    public class LineSegment : ColorableObject
    {
        #region | Variables |

        private LineRenderer _lineRenderer;

        [Header("State Variables:")] [SerializeField]
        private Material _idleStateMaterial;

        [SerializeField] private Material _hoverStateMaterial;
        [SerializeField] private Material _defaultIdleStateMaterial;
        [SerializeField] private float _idleLineWidth;
        [SerializeField] private float _hoverLineWidthRatio = 1.05f;

        [Header("Debugging")] [SerializeField] private bool _enableDebugLogs = true;
        [SerializeField] private bool _isFollowingMarker;
        [SerializeField] private GridPoint[] _gridPoints = new GridPoint[2];
        [SerializeField] public int PointAReference;
        [SerializeField] public int PointBReference;
        public Vector3 PosA;
        public Vector3 PosB;
        private MeshCollider _meshCollider;

        #endregion


        #region | Constructor |

        public LineSegment(string keyName, GridPoint startPoint, float startLineWidth, float endLineWidth)
        {
            name = keyName;
            _idleLineWidth = startLineWidth;
            _isFollowingMarker = true;
            _lineRenderer.startWidth = startLineWidth;
            _lineRenderer.endWidth = endLineWidth;
            _gridPoints[0] = startPoint;
            PointAReference = startPoint.referenceInt;
            PosA = startPoint.transform.position;
            _lineRenderer.SetPosition(0, startPoint.transform.position);
            _lineRenderer.SetPosition(1, startPoint.transform.position);
            startPoint.AddLineSegment(this);
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
         *      StartLineSegment()
         *      EndLineSegment()
         *      GenerateMeshCollider()
         *      FollowMarkerTip()
         *      Erase()
         *
         *  | Inherited Methods |
         *      SetStateConditionally()
         *      ApplyColorMaterial()
         *      ResetColorMaterial()
         *
         *  | Getters & Setters |
         *      GetEndPoints()
         *      GetIsFollowingMarker()
         *      GetLength()
         *      GetDirection()
         *
         * **********************
         */

        
        
        #region | Unity Methods |

        private void OnEnable()
        {
            _objectType = InteractableGridObjectType.LineSegment;
            _lineRenderer = GetComponent<LineRenderer>();
            _meshCollider = GetComponent<MeshCollider>();
            _defaultIdleStateMaterial = _idleStateMaterial;
        }

        #endregion

        #region | Custom Methods |

        // Would like to make this into a constructor, but this needs to be done on a GameObject level for prefabs.
        public void StartLineSegment(string keyName, GridPoint startPoint, float startLineWidth, float endLineWidth)
        {
            name = keyName;
            _idleLineWidth = startLineWidth;
            _isFollowingMarker = true;
            _lineRenderer.startWidth = startLineWidth;
            _lineRenderer.endWidth = endLineWidth;
            _gridPoints[0] = startPoint;
            PointAReference = startPoint.referenceInt;
            PosA = startPoint.transform.position;
            _lineRenderer.SetPosition(0, startPoint.transform.position);
            _lineRenderer.SetPosition(1, startPoint.transform.position);
            startPoint.AddLineSegment(this);
        }

        public void EndLineSegment(GridPoint endPoint)
        {
            _isFollowingMarker = false;
            _gridPoints[1] = endPoint;
            PointBReference = endPoint.referenceInt;
            //name = new int2(PointAReference, PointBReference);
            //gameObject.name = "e." + name.x + " <==> " + name.y;
            _gridPoints[1].SetStateConditionally(InteractableObjectState.Attached);
            _gridPoints[0].SetStateConditionally(InteractableObjectState.Attached);
            _lineRenderer.SetPosition(1, endPoint.transform.position);
            endPoint.AddLineSegment(this);
            SetStateConditionally(InteractableObjectState.Idle);
        }

        public void GenerateMeshCollider()
        {
            Mesh mesh = new Mesh();
            _lineRenderer.BakeMesh(mesh);
            _meshCollider.sharedMesh = mesh;
        }

        // [[ From >> NewLatticeGrid.FollowMarkerTip 
        public void FollowMarkerTip(Vector3 markerTip)
        {
            if (_enableDebugLogs)
            {
                Debug.Log("LineSegment - FollowMarkerTip");
            }

            _lineRenderer.SetPosition(1, markerTip);
        }

        public void Erase()
        {
            _gridPoints[0].RemoveTargetLineFromList(this);
            _gridPoints[1].RemoveTargetLineFromList(this);
            Destroy(gameObject);
        }

        #endregion

        #region | Inherited Methods |

        public override void SetStateConditionally(InteractableObjectState newState)
        {
            switch (newState)
            {
                case InteractableObjectState.Idle:
                    if (_enableDebugLogs)
                    {
                        Debug.Log("LineSegment: Idle State");
                    }

                    if (!_isFollowingMarker)
                    {
                        _lineRenderer.material = _idleStateMaterial;
                        _lineRenderer.startWidth = _idleLineWidth;
                        _lineRenderer.endWidth = _idleLineWidth;
                        GenerateMeshCollider();
                    }

                    break;
                case InteractableObjectState.EnterHover:
                    if (_enableDebugLogs)
                    {
                        Debug.Log("LineSegment: Enter Hover State");
                    }

                    _lineRenderer.material = _hoverStateMaterial;
                    _lineRenderer.startWidth = _idleLineWidth * _hoverLineWidthRatio;
                    _lineRenderer.endWidth = _idleLineWidth * _hoverLineWidthRatio;
                    break;
                default:
                    Debug.Log("LineSegmentL ArgumentOutOfRange");
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void ApplyColorMaterial(Material newColor)
        {
            _idleStateMaterial = newColor;
        }

        public override void ResetColorMaterial()
        {
            _idleStateMaterial = _defaultIdleStateMaterial;
        }

        #endregion

        #region | Getters & Setters |

        public GridPoint[] GetEndPoints()
        {
            return _gridPoints;
        }

        uint GetVector3IntHash(Vector3Int i)
        {
            int3 _newInt3 = new int3(i.x, i.y, i.z);
            return math.hash(_newInt3);
        }

        public bool GetIsFollowingMarker()
        {
            return _isFollowingMarker;
        }

        public float GetLength()
        {
            if (_gridPoints[0] && _gridPoints[1])
            {
                if (_enableDebugLogs)
                {
                    Debug.Log("LineSegment Length: " + Vector3.Distance(_gridPoints[0].transform.position,
                        _gridPoints[1].transform.position));
                }

                return Vector3.Distance(_gridPoints[0].transform.position, _gridPoints[1].transform.position);
            }

            return 0;
        }

        public Vector3 GetDirection(GridPoint startPoint)
        {
            if (_gridPoints[0] == startPoint)
            {
                if (_enableDebugLogs)
                {
                    Debug.Log("LineSegment Direction: " +
                              (startPoint.GetGridWorldPosition() - _gridPoints[1].GetGridWorldPosition()));
                }

                return startPoint.GetGridWorldPosition() - _gridPoints[1].GetGridWorldPosition();
            }

            if (_gridPoints[1] == startPoint)
            {
                if (_enableDebugLogs)
                {
                    Debug.Log("LineSegment Direction: " +
                              (startPoint.GetGridWorldPosition() - _gridPoints[0].GetGridWorldPosition()));
                }

                return startPoint.GetGridWorldPosition() - _gridPoints[0].GetGridWorldPosition();
            }

            Debug.LogError("LineSegment.GetDirection: Target startPoint Not Found");
            return Vector3.zero;
        }

        #endregion

        #endregion
    }
}