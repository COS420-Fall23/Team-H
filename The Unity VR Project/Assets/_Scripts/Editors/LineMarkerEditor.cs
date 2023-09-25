#if UNITY_EDITOR
using LatticeLand;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LineMarker)), CanEditMultipleObjects]
public class LineMarkerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        LineMarker lineMarker = (LineMarker)target;
        
        EditorGUILayout.HelpBox("InputAction Test Buttons: ", MessageType.None);
        
        if (GUILayout.Button("Draw Action ('Trigger Pressed')"))
        {
            lineMarker.DrawAction();
        }
        
        if (GUILayout.Button("Erase Action ('Primary Button')"))
        {
            lineMarker.EraseAction();
        }
        
        if (GUILayout.Button("Switch Tool ('Secondary Button')"))
        {
            lineMarker.SwitchTool();
        }
    }
}

#endif