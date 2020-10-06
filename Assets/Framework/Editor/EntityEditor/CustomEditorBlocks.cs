#if DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RangerV
{
    public static class CustomEditorBlocks
    {
        public static void ShowScriptableObject(SerializedProperty property, ScriptableObject scriptableObject, GUIStyle boxStyle)
        {
            EditorGUILayout.PropertyField(property);
            if (scriptableObject != null)
            {
                EditorGUILayout.BeginVertical(boxStyle);
                {
                    var editor = Editor.CreateEditor(scriptableObject);
                    editor.OnInspectorGUI();
                }
                EditorGUILayout.EndVertical();
            }

        }

    }
}


#endif