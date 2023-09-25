using System.Collections.Generic;
using UnityEngine;

namespace LatticeLand.Utils
{
    public class ShapeDetection : MonoBehaviour
    {
        private List<GridPoint> _gridPoints;
        private List<LineSegment> _lineSegments;

        public float GetAngle(GridPoint intersectionPoint, LineSegment initialLine, LineSegment terminalLine)
        {
            return Vector3.Angle(terminalLine.GetDirection(intersectionPoint),
                initialLine.GetDirection(intersectionPoint));
        }

        public int GetCountOfGridPoints()
        {
            return _gridPoints.Count;
        }

        public int GetCountOfLineSegments()
        {
            return _lineSegments.Count;
        }
    }
}