#if UNITY_EDITOR
using LatticeLand;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LineSegment)), CanEditMultipleObjects]
public class LineSegmentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LineSegment line = (LineSegment)target;
        DrawDefaultInspector();
        EditorGUILayout.Separator();
        if (GUILayout.Button("Regenerate Mesh"))
        {
            line.GenerateMeshCollider();
        }

        EditorGUILayout.Separator();
    }
}
#endif