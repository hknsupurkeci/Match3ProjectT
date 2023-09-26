using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Shape))]
public class ShapeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Shape shape = (Shape)target;

        shape.width = EditorGUILayout.IntField("Width", shape.width);
        shape.height = EditorGUILayout.IntField("Height", shape.height);

        SerializedProperty arrayProp = serializedObject.FindProperty("shapeArray");
        serializedObject.Update();

        int newLength = shape.width * shape.height;
        if (newLength != shape.shapeArray.Length)
        {
            System.Array.Resize(ref shape.shapeArray, newLength);
        }

        for (int y = 0; y < shape.height; y++)
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < shape.width; x++)
            {
                int index = y * shape.width + x;
                shape.shapeArray[index] = EditorGUILayout.IntField(shape.shapeArray[index]);
            }
            EditorGUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
