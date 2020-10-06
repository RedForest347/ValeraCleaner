//using UnityEngine;
//using UnityEditor;

//using System.Collections;
//using System.Collections.Generic;
//using System;

//[CustomEditor(typeof(GUISkin))]
//public class GUISkinEditor : Editor
//{
//    GUISkin _skin;

//    public void OnEnable()
//    {
//        _skin = (GUISkin)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        EditorGUILayout.BeginVertical("box");
//        EditorGUILayout.LabelField("Name: " + _skin.name);
//        EditorGUILayout.BeginHorizontal();
//        if (GUILayout.Button("Reset to 'Dark GUI Style'", GUILayout.Width(200f)))
//        {
//            _skin.box = GUI.skin.box;
//            _skin.button = GUI.skin.button;
//            _skin.toggle = GUI.skin.toggle;
//            _skin.label = GUI.skin.label;
//            _skin.textField = GUI.skin.textField;
//            _skin.textArea = GUI.skin.textArea;
//            _skin.window = GUI.skin.window;

//            _skin.horizontalSlider = GUI.skin.horizontalSlider;
//            _skin.horizontalSliderThumb = GUI.skin.horizontalSliderThumb;
//            _skin.verticalSlider = GUI.skin.verticalSlider;
//            _skin.verticalSliderThumb = GUI.skin.verticalSliderThumb;

//            _skin.horizontalScrollbar = GUI.skin.horizontalScrollbar;
//            _skin.horizontalScrollbarThumb = GUI.skin.horizontalScrollbarThumb;
//            _skin.horizontalScrollbarLeftButton = GUI.skin.horizontalScrollbarLeftButton;
//            _skin.horizontalScrollbarRightButton = GUI.skin.horizontalScrollbarRightButton;

//            _skin.verticalScrollbar = GUI.skin.verticalScrollbar;
//            _skin.verticalScrollbarThumb = GUI.skin.verticalScrollbarThumb;
//            _skin.verticalScrollbarUpButton = GUI.skin.verticalScrollbarUpButton;
//            _skin.verticalScrollbarDownButton = GUI.skin.verticalScrollbarDownButton;

//            _skin.scrollView = GUI.skin.scrollView;
//            _skin.customStyles = GUI.skin.customStyles;

//            Debug.Log("'Dark GUI Style' set");
//        }
//        if (GUILayout.Button("Add box", GUILayout.Width(100f)))
//        {
//            GUIStyle[] styles = _skin.customStyles;
//            Array.Resize<GUIStyle>(ref styles, _skin.customStyles.Length + 1);
//            _skin.customStyles = styles;

//            _skin.customStyles[_skin.customStyles.Length - 1] = GUI.skin.box;
//        }
//        EditorGUILayout.EndHorizontal();
//        EditorGUILayout.Separator();
//        EditorGUILayout.EndVertical();

//        DrawDefaultInspector();
//    }
//}





