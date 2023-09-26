#if UNITY_EDITOR
using LatticeLand;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShapeDrawer)), CanEditMultipleObjects]
public class ShapeDrawerEditor : Editor
{
    #region | SerializedProperties |

    // Shape Selection Properties
    SerializedProperty shapeDimensionSelection;
    SerializedProperty shapeSelection2D;
    SerializedProperty shapeSelection3D;

    // Relativity Properties

    // Shape Information
    SerializedProperty startingGridPoint;

    SerializedProperty lineLength;
    SerializedProperty lineDirection;

    SerializedProperty tipGridPos;
    SerializedProperty chronologicallyConnectedPoints;


    private string _drawButtonLabel;

    #endregion

    private void OnEnable()
    {
        shapeDimensionSelection = serializedObject.FindProperty("_shapeDimensionSelection");

        shapeSelection2D = serializedObject.FindProperty("_shapeSelection2D");
        shapeSelection3D = serializedObject.FindProperty("_shapeSelection3D");

        startingGridPoint = serializedObject.FindProperty("_startingGridPoint");
        lineDirection = serializedObject.FindProperty("_lineDirection");
        lineLength = serializedObject.FindProperty("_lineLength");

        tipGridPos = serializedObject.FindProperty("_tipGridPos");
        chronologicallyConnectedPoints = serializedObject.FindProperty("_chronologicallyConnectedPoints");
    }

    public override void OnInspectorGUI()
    {
        ShapeDrawer shapeDrawer = (ShapeDrawer)target;

        serializedObject.Update();

        EditorGUILayout.HelpBox("  NOTE: To minimize issues, enter *PLAY MODE* before modifying shape information.",
            MessageType.None);
        EditorGUILayout.Space(10);

        EditorGUILayout.PropertyField(shapeDimensionSelection);

        // Manually Draw Shape With Complete Array Of GridPoints():
        if (shapeDrawer.GetShapeDimensionSelection() == ShapeDimensionSelection.Manual_Complete)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Manually Draw With Complete Array Of GridPoints:");
            EditorGUILayout.HelpBox(
                "No need to enter the first point again at the end. This automatically finishes the shape by connecting the last point to the first.",
                MessageType.None);
            EditorGUILayout.PropertyField(chronologicallyConnectedPoints);

            if (GUILayout.Button("Draw Shape"))
            {
                shapeDrawer.DrawShape(shapeDrawer.GetChronologicallyConnectedPoints());
            }
        }
        // Manually Draw Shape, One Line At A Time
        else if (shapeDrawer.GetShapeDimensionSelection() == ShapeDimensionSelection.Manual_Iteratively)
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Manually Draw One Point At A Time:");
            EditorGUILayout.PropertyField(startingGridPoint);

            EditorGUILayout.PropertyField(lineDirection, new GUIContent("Direction to Draw Line (Global)"));
            EditorGUILayout.PropertyField(lineLength);

            EditorGUILayout.HelpBox(
                "Draw a line in the selected direction with the selected length. The starting point will automatically be redefined as the previous line's end point.",
                MessageType.None);
            if (GUILayout.Button("Draw Line"))
            {
                shapeDrawer.DrawLine();
            }
        }
        else
        {
            // 2D Shape Draw
            if (shapeDrawer.GetShapeDimensionSelection() == ShapeDimensionSelection.TwoDimensional)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Select Shape & Set Parameters:");
                EditorGUILayout.PropertyField(shapeSelection2D);
                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("Shape Properties:");
                EditorGUILayout.PropertyField(startingGridPoint);
                EditorGUILayout.HelpBox("(0,0,0) is Left(x), Bottom(y), Front(z)", MessageType.None);
                EditorGUILayout.PropertyField(lineDirection, new GUIContent("Direction to draw base along: "));
                EditorGUILayout.PropertyField(lineLength, new GUIContent("Base Line Length: "));
                if (shapeDrawer.GetShapeSelection2D() == ShapeSelection2D.Triangle)
                {
                    _drawButtonLabel = "Draw Triangle";
                    EditorGUILayout.PropertyField(tipGridPos);
                }

                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("Relativity Handling:");

                if (shapeDrawer.GetShapeSelection2D() == ShapeSelection2D.Square)
                {
                    _drawButtonLabel = "Draw Square";
                }

                EditorGUILayout.Separator();

                if (GUILayout.Button(_drawButtonLabel))
                {
                    switch (shapeDrawer.GetShapeSelection2D())
                    {
                        case ShapeSelection2D.Triangle:
                            shapeDrawer.DrawTriangle();
                            break;
                        case ShapeSelection2D.Square:
                            shapeDrawer.DrawSquare();
                            break;
                    }
                }
            }

            // 3D Shape Draw
            if (shapeDrawer.GetShapeDimensionSelection() == ShapeDimensionSelection.ThreeDimensional)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Select Shape & Set Parameters:");
                EditorGUILayout.PropertyField(shapeSelection3D);

                EditorGUILayout.Separator();
                EditorGUILayout.PropertyField(startingGridPoint);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif