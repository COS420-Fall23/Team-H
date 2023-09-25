#if UNITY_EDITOR
using LatticeLand;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColoringTool)), CanEditMultipleObjects]
public class ColoringToolEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ColoringTool coloringTool = (ColoringTool)target;

        EditorGUILayout.HelpBox("InputAction Test Buttons: ", MessageType.None);
        if (GUILayout.Button("Apply Color"))
        {
            coloringTool.ApplyColor();
        }

        if (GUILayout.Button("Cycle Color"))
        {
            coloringTool.CycleColor();
        }

        if (GUILayout.Button("Switch Tool"))
        {
            coloringTool.SwitchTool();
        }
    }
}
#endif