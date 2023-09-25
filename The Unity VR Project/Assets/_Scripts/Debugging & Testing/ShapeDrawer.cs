using UnityEngine;

namespace LatticeLand
{
    #region | Enums |

    public enum ShapeDimensionSelection
    {
        Manual_Complete,
        Manual_Iteratively,
        TwoDimensional,
        ThreeDimensional
    }

    public enum ShapeSelection2D
    {
        Triangle,
        Square
    }

    public enum ShapeSelection3D
    {
        Pyramid,
        Cube
    }

    public enum GlobalGridDirection
    {
        Right_PosX,
        Left_NegX,
        Up_PosY,
        Down_NegY,
        Forward_PosZ,
        Backward_NegZ
    }

    #endregion

    public class ShapeDrawer : MonoBehaviour
    {
        #region | Core Variables |

        private LatticeGrid _latticeGrid;
        private LatticeGrid_Networking _gridNetworking;

        [SerializeField] private ShapeDimensionSelection _shapeDimensionSelection;

        [SerializeField] private Vector3Int[] _chronologicallyConnectedPoints;
        [SerializeField] private ShapeSelection2D _shapeSelection2D;
        [SerializeField] private ShapeSelection3D _shapeSelection3D;

        [SerializeField] private Vector3Int _startingGridPoint;
        [SerializeField] private GlobalGridDirection _lineDirection;
        [SerializeField] private int _lineLength;


        [SerializeField] private Vector3Int _tipGridPos;


        // Local Direction properties

        // Global Direction Properties

        #endregion

        #region | Unity Methods |

        private void OnEnable()
        {
            _latticeGrid = GetComponent<LatticeGrid>();
            _gridNetworking = FindObjectOfType<LatticeGrid_Networking>();
        }

        #endregion

        #region | Universal Methods |

        public void DrawShape(Vector3Int[] chronologicallyConnectedPoints)
        {
            Debug.Log("Drawing Shape");
            for (int i = 0; i < chronologicallyConnectedPoints.Length; i++)
            {
                if (i == chronologicallyConnectedPoints.Length - 1)
                {
                    Debug.Log("Finishing Shape");
                    _latticeGrid.StartLineDraw($"Line: {UnityEngine.Random.value.GetHashCode()}",
                        _latticeGrid.GetTargetGridPoint(chronologicallyConnectedPoints[i]));
                    _latticeGrid.EndLineDraw(_latticeGrid.GetTargetGridPoint(chronologicallyConnectedPoints[0]));
                    return;
                }

                _latticeGrid.StartLineDraw($"Line: {UnityEngine.Random.value.GetHashCode()}",
                    _latticeGrid.GetTargetGridPoint(chronologicallyConnectedPoints[i]));
                _latticeGrid.EndLineDraw(_latticeGrid.GetTargetGridPoint(chronologicallyConnectedPoints[i + 1]));
            }
        }

        public void DrawLine()
        {
            Vector3Int endPoint = DetermineLineEndPoint();
            _latticeGrid.StartLineDraw($"Line: {UnityEngine.Random.value.GetHashCode()}",
                _latticeGrid.GetTargetGridPoint(_startingGridPoint));
            _latticeGrid.EndLineDraw(_latticeGrid.GetTargetGridPoint(endPoint));
            _startingGridPoint = endPoint;
        }

        #endregion

        #region | Two-Dimentional Methods |

        public void DrawTriangle()
        {
            Vector3Int[] gridPoints = new Vector3Int[3];
            gridPoints[0] = _startingGridPoint;
            gridPoints[1] = DetermineLineEndPoint();
            gridPoints[2] = _tipGridPos;
            DrawShape(gridPoints);
        }

        public void DrawSquare()
        {
            Debug.Log("Will Draw Square Soon");
        }

        private Vector3Int DetermineLineEndPoint()
        {
            switch (_lineDirection)
            {
                case GlobalGridDirection.Right_PosX:
                    return new Vector3Int(_startingGridPoint.x + _lineLength, _startingGridPoint.y,
                        _startingGridPoint.z);
                case GlobalGridDirection.Left_NegX:
                    return new Vector3Int(_startingGridPoint.x - _lineLength, _startingGridPoint.y,
                        _startingGridPoint.z);
                case GlobalGridDirection.Up_PosY:
                    return new Vector3Int(_startingGridPoint.x, _startingGridPoint.y + _lineLength,
                        _startingGridPoint.z);
                case GlobalGridDirection.Down_NegY:
                    return new Vector3Int(_startingGridPoint.x, _startingGridPoint.y - _lineLength,
                        _startingGridPoint.z);
                case GlobalGridDirection.Forward_PosZ:
                    return new Vector3Int(_startingGridPoint.x, _startingGridPoint.y,
                        _startingGridPoint.z + _lineLength);
                case GlobalGridDirection.Backward_NegZ:
                    return new Vector3Int(_startingGridPoint.x, _startingGridPoint.y,
                        _startingGridPoint.z - _lineLength);
            }

            return Vector3Int.zero;
        }

        #endregion

        #region | Three-Dimentional Methods |

        /*

public void Draw3DShape(ShapeSelection3D shapeSelection)
{
    switch (shapeSelection)
    {
        case ShapeSelection3D.Pyramid:
            DrawPyramid();
            break;
        case ShapeSelection3D.Cube:
            DrawCube();
            break;
    }
}

private void DrawPyramid()
{
    Debug.Log("Draw Pyramid");
}

private void DrawCube()
{
    Debug.Log("Draw Cube");
}
*/

        #endregion

        #region | Getters & Setters |

        public ShapeDimensionSelection GetShapeDimensionSelection()
        {
            return _shapeDimensionSelection;
        }

        public ShapeSelection2D GetShapeSelection2D()
        {
            return _shapeSelection2D;
        }

        public Vector3Int[] GetChronologicallyConnectedPoints()
        {
            return _chronologicallyConnectedPoints;
        }

        public Vector3Int GetGridDimentions()
        {
            return _latticeGrid.GetGridDimensions();
        }

        #endregion
    }
}