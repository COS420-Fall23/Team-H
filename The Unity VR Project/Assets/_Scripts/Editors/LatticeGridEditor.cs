#if UNITY_EDITOR
using LatticeLand;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LatticeGrid)), CanEditMultipleObjects]
public class LatticeGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LatticeGrid latticeGrid = (LatticeGrid)target;

        EditorGUILayout.Separator();

        if (GUILayout.Button("Generate Lattice Grid"))
        {
            latticeGrid.GenerateLatticeGrid();
        }

        if (GUILayout.Button("Delete Grid"))
        {
            latticeGrid.DeleteGrid();
        }

        EditorGUILayout.Separator();
    }
}
#endif