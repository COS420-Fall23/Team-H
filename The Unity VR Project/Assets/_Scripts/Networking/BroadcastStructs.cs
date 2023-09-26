using FishNet.Broadcast;
using UnityEngine;

namespace LatticeLand
{
    // I would like to make these structs readonly, and create a seperate broadcast for GridPoint commands and LineSegment commands.

    public struct InteractionCommandBroadcast : IBroadcast
    {
        public InteractableGridObjectType InteractableGridObjectType;
        public LatticeGridCommand command;

        // Color Command
        public int colorMaterialIndex;

        // GridPoint Params
        public Vector3Int gridPointCoordinates;

        // LineSegment Params
        public string lineSegmentName;
    }

    public struct MarkerBroadcast : IBroadcast
    {
        public bool isDrawing;
        public Vector3 markerTipPosition;
    }
}