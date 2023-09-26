#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LatticeLand;

[CustomEditor(typeof(Matchmaker))]
public class MatchmakerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Matchmaker matchmaker = (Matchmaker)target;
        
        EditorGUILayout.Separator();

        if (GUILayout.Button("Join Random"))
        {
            matchmaker.JoinRandom();
        }
    }
}
#endif
