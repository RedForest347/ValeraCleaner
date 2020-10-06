using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using RangerV;
using System.Linq;
using System;


namespace RangerV
{

    [CustomPropertyDrawer(typeof(MethodHolder))]
    public class MethodHolderEditor : PropertyDrawer
    {
        ComponentBase componentBase;
        MethodHolder methodHolder;
        SerializedProperty property;

        public override void OnGUI(Rect position, SerializedProperty _property, GUIContent label)
        {
            property = _property;

            EditorGUI.BeginProperty(position, label, _property);

            GUISkin skin = GUI.skin;

            componentBase = (ComponentBase)(_property.serializedObject.targetObject);
            methodHolder = (MethodHolder)componentBase.GetType().GetField(_property.name).GetValue(_property.serializedObject.targetObject);

            string dropdown_button_name = methodHolder.method_name;


            GUIStyle style = skin.GetStyle("Button");
            GUIContent content = new GUIContent(dropdown_button_name == "" ? "no function selected" : dropdown_button_name);

            style.fixedWidth = Math.Max(new GUIStyle().CalcSize(content).x + 20, 100);
            style.fixedHeight = Math.Max(new GUIStyle().CalcSize(content).y + 5, 20);



            if (EditorGUI.DropdownButton(position, content, FocusType.Passive, style))
            {
                CreateDropDownMenu(methodHolder, componentBase.gameObject);
            }

            EditorGUI.EndProperty();

            void CreateDropDownMenu(MethodHolder someT, GameObject FieldHolder)
            {
                GenericMenu dropdownMenu = new GenericMenu();

                Component[] CmpList = FieldHolder.GetComponents<Component>();

                List<MethodInfo> methodInfos = new List<MethodInfo>(100);

                for (int i = 0; i < CmpList.Length; i++)
                {
                    BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.NonPublic;

                    List<MethodInfo> Methods = CmpList[i].GetType().GetMethods(bindingFlags)
                        .Where((p) => p.GetParameters().Length == 0)
                        .ToList();

                    for (int k = 0; k < Methods.Count; k++)
                        methodInfos.Add(Methods[k]);

                }

                methodInfos.Sort((i, j) => string.Compare(i.DeclaringType + "/" + i.Name, j.DeclaringType + "/" + j.Name));

                for (int i = 0; i < methodInfos.Count; i++)
                {
                    string path = methodInfos[i].DeclaringType + "/" + methodInfos[i].Name + " (" + 
                        methodInfos[i].ReturnType.Name + ") " + (methodInfos[i].IsPublic ? "" : "(private)");

                    bool on = methodInfos[i].Name == someT.method_name;

                    dropdownMenu.AddItem(new GUIContent(path), on, SetMethodData, methodInfos[i]);
                }

                dropdownMenu.ShowAsContext();
            }
        }

        void SetMethodData(object _methodInfo)
        {
            MethodInfo methodInfo = (MethodInfo)_methodInfo;

            methodHolder.method_name = methodInfo.Name;
            methodHolder.type_name = methodInfo.DeclaringType.FullName;
            methodHolder.assembly_name = methodInfo.DeclaringType.Assembly.FullName;
            methodHolder.component = componentBase.GetComponent(methodInfo.DeclaringType);

            Type[] types = methodInfo.DeclaringType.Assembly.GetTypes();

            EditorUtility.SetDirty(property.serializedObject.targetObject);
            Undo.RecordObject(property.serializedObject.targetObject, "Changed Sequence");

            methodHolder.DataCheckOnCorrect();
        }
    }
}
