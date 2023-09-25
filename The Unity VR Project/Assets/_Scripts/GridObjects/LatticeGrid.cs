using System;
using System.Collections.Generic;
using UnityEngine;

namespace LatticeLand
{
    public class LatticeGrid : MonoBehaviour
    {
        #region | Variables |

        [Header("Grid Parameters: ")] [SerializeField]
        private GridPoint _gridPointPrefab;

        [SerializeField] private Vector3Int _gridDimensions;
        [SerializeField] private float _gapBetweenGridPoints;
        [SerializeField] private float _gridPointScale;

        [Header("Grid State Variables")] [SerializeField]
        private LineSegment _lineSegmentPrefab;

        [SerializeField] public Polygon _polyPrefab;
        [SerializeField] private Material[] _coloringMaterials;
        private GridPoint[,,] _gridPoints;

        private Dictionary<string, LineSegment> _drawnLineSegments;
        private LineSegment _curLineSeg; // Used to store line in between StartLine and EndLine.


        [Header("Debugging")] [SerializeField] private bool _autoGenerateGrid;
        [SerializeField] private bool _enableDebugLogs = true;

        #endregion

        #region | All Methods |

        /* *********************
         * Table of Methods
         * *********************
         *
         *  | Unity Methods |
         *      OnEnable()
         * 
         *  | Grid Instance Manipulation Methods |
         *      GenerateLatticeGrid()
         *      DeleteGrid()
         *
         *  | Line Drawing Methods |
         *      StartLineDraw()
         *      EndLineDraw()
         *      DeselectGridPoint()
         *      FollowMarkerTip()
         *      ApplyColorMaterial()
         *      EraseTargetLine()
         *
         *  | Getters & Setters |
         *      GetTargetGridPoint()
         *      GetTargetLineSegment()
         *      GetDimensions()
         *      GetLineDictionaryCount()
         *
         * **********************
         */

        #region | Unity Methods |

        private void OnEnable()
        {
            _drawnLineSegments = new Dictionary<string, LineSegment>();
        }

        private void Start()
        {
            if (_autoGenerateGrid)
            {
                GenerateLatticeGrid();
            }
        }

        #endregion

        #region | Grid Instance Manipulation Methods |

        public void GenerateLatticeGrid()
        {
            if (_enableDebugLogs)
            {
                Debug.Log("Generating Grid...");
            }

            _gridPoints = new GridPoint[_gridDimensions.x, _gridDimensions.y, _gridDimensions.z];

            for (int x = 0; x < _gridDimensions.x; x++)
            {
                for (int y = 0; y < _gridDimensions.y; y++)
                {
                    for (int z = 0; z < _gridDimensions.z; z++)
                    {
                        Vector3 calcSpawnPoint = new Vector3(x * _gapBetweenGridPoints, y * _gapBetweenGridPoints,
                            z * _gapBetweenGridPoints);
                        GridPoint newGridPoint = Instantiate(_gridPointPrefab, gameObject.transform);
                        newGridPoint.transform.localPosition = calcSpawnPoint;
                        GridPoint gridPointComponent = newGridPoint.GetComponent<GridPoint>();
                        gridPointComponent.SetGridCoordinates(x, y, z);
                        gridPointComponent.SetIdleStateScale(_gridPointScale);
                        newGridPoint.transform.localScale =
                            new Vector3(_gridPointScale, _gridPointScale, _gridPointScale);
                        _gridPoints[x, y, z] = gridPointComponent;
                    }
                }
            }
        }

        public void DeleteGrid()
        {
            if (_enableDebugLogs)
            {
                Debug.Log("Deleting Grid...");
            }

            if (GetComponentInChildren<GridPoint>())
            {
                while (transform.childCount > 0)
                {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }
            }
        }

        #endregion

        #region | Line Drawing Methods |

        public void StartLineDraw(string keyName, GridPoint targetGP)
        {
            if (_enableDebugLogs) Debug.Log("Grid - Start Line Draw");

            targetGP.SetStateConditionally(InteractableObjectState.Selected);

            if (_enableDebugLogs) Debug.Log("Grid - TargetGP Coordinates: " + targetGP.GetGridCoordinates());


            _curLineSeg = Instantiate(_lineSegmentPrefab, Vector3.zero, Quaternion.identity);
            _curLineSeg.StartLineSegment(keyName, targetGP,
                targetGP.transform.localScale.x / 2, .002f);
            // Go To >> LineSegment.StartLineSegment
        }


        public void EndLineDraw(GridPoint targetGP)
        {
            if (_enableDebugLogs)
            {
                Debug.Log("Grid: End Line Draw");
            }

            _curLineSeg.EndLineSegment(targetGP);

            _drawnLineSegments.Add(_curLineSeg.name, _curLineSeg);
            _curLineSeg = null;
            // Go To >> LineSegment.EndLineSegment
        }

        public void GeneratePolygon(int[] pointRefs, Vector3[] positions)
        {
            Polygon _newPoly = Instantiate(_polyPrefab, Vector3.zero, Quaternion.identity);
            _newPoly.positions = positions;
            _newPoly.refs = pointRefs;

            string s = pointRefs[0].ToString();
            for (int j = 1; j < pointRefs.Length; j++)
            {
                s += "==>" + pointRefs[j];
            }

            _newPoly.gameObject.name = "f." + s;
        }


        public void DeselectGridPoint(GridPoint targetGP)
        {
            if (_enableDebugLogs)
            {
                Debug.Log("Grid: Deselect Grid Point");
            }

            targetGP.RemoveTargetLineFromList(_curLineSeg);
            targetGP.SetStateConditionally(InteractableObjectState.Idle);
            Destroy(_curLineSeg.gameObject);
            _curLineSeg = null;
            targetGP.SetStateConditionally(InteractableObjectState.Idle);
            // Go To >> GridPoint.SetStateConditionally
        }


        public void FollowMarkerTip(Vector3 endPosition)
        {
            if (_enableDebugLogs)
            {
                Debug.Log("Grid: Follow Marker Tip");
            }

            _curLineSeg.FollowMarkerTip(endPosition);
            // [[ Got To >> NewLineSegment.FollowMarkerTip
        }

        public void ApplyColorMaterial(int colorMaterialIndex, ColorableObject targetObj)
        {
            targetObj.ApplyColorMaterial(_coloringMaterials[colorMaterialIndex]);
        }

        public void EraseTargetLine(LineSegment targetLine)
        {
            _drawnLineSegments.Remove(targetLine.name);
            targetLine.Erase();
        }

        #endregion

        #region | Getters & Setters |

        public GridPoint GetTargetGridPoint(Vector3Int targetGridPointCoordinates)
        {
            return _gridPoints[targetGridPointCoordinates.x, targetGridPointCoordinates.y,
                targetGridPointCoordinates.z];
        }

        public LineSegment GetTargetLineSegment(string targetLineSegmentName)
        {
            if (_drawnLineSegments.ContainsKey(targetLineSegmentName))
            {
                return _drawnLineSegments[targetLineSegmentName];
            }

            throw new ArgumentException();
        }

        public Vector3Int GetGridDimensions()
        {
            return _gridDimensions;
        }

        public int GetLineDictionaryCount()
        {
            return _drawnLineSegments.Count;
        }

        #endregion

        #endregion
    }
}