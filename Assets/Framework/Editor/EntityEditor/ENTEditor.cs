using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;
using UnityEditor;

namespace RangerV
{
    #region CUSTOM_ENT_EDITOR_WINDOW_V3
    public class ENTEditor : EditorWindowSelected<Entity>
    {
        string proc_mes;
        bool should_show;

        protected override void Enable()
        {
            if (selected_object != null)
            {
                proc_mes = ProcMesHelper.CreateSupportProcInfo(selected_object);
            }
        }

        protected override void SelectionChange()
        {
            if (selected_object != null)
            {
                proc_mes = ProcMesHelper.CreateSupportProcInfo(selected_object);
            }
        }

        [MenuItem("Window/Entity editor")]
        static void GetWindow()
        {
            GetWindow<ENTEditor>("Entity editor");
        }

        protected override void GUIDraw()
        {
            EditorGUILayout.BeginVertical(GUIEditorSettings.box_1_0);
            {
                EditorGUILayout.BeginVertical(GUIEditorSettings.box_1_1);
                EditorGUILayout.LabelField("ENTITY:    " + selected_object.entity.ToString() + "    '" + selected_object.gameObject.name + "'", GUIEditorSettings.headerLabel);
                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();

                CheckCmpsOnCorrect();
                

                #region SHOW_COMPONENTS
                EditorGUILayout.BeginHorizontal(GUIEditorSettings.box_1_1);

                

                EditorGUILayout.LabelField("Components count:    " + selected_object.Components.Count, EditorStyles.boldLabel);

                if (GUILayout.Button("Component manager", GUIEditorSettings.button_0, GUILayout.Width(140f), GUILayout.Height(20f)))
                {
                    AddComponent();
                }

                EditorGUILayout.EndHorizontal();

                if (selected_object.Components.Count > 0)
                {
                    int compBases_count = selected_object.Components.Count;
                    for (int component_index = 0; component_index < compBases_count; component_index++)
                    {
                        #region SHOW_COMPONENT

                        ComponentBase component = selected_object.Components[component_index];
                        EditorGUILayout.BeginVertical();
                        {
                            if (component != null)
                            {
                                EditorGUILayout.BeginVertical();
                                EditorGUILayout.BeginHorizontal(GUIEditorSettings.box_0_1);
                                {
                                    EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

                                    if (component.GetType().GetCustomAttribute<ComponentAttribute>() != null)
                                        EditorGUILayout.LabelField(new GUIContent(component.GetType().GetCustomAttribute<ComponentAttribute>().GetIcon()), GUILayout.Width(20));
                                    else
                                        EditorGUILayout.LabelField(new GUIContent(EditorGUIUtility.IconContent("dll Script Icon").image), GUILayout.Width(20));

                                    selected_object.show_comp[component_index] = EditorGUILayout.Foldout(selected_object.show_comp[component_index], component.GetType().ToString(), true);
                                    EditorGUILayout.EndHorizontal();

                                    if (component_index != 0 && GUILayout.Button("↑", GUIEditorSettings.button_0, GUILayout.Width(20f)))
                                    {
                                        selected_object.Components[component_index] = selected_object.Components[component_index - 1];
                                        selected_object.Components[component_index - 1] = component;

                                        bool show_comp_temp = selected_object.show_comp[component_index];
                                        selected_object.show_comp[component_index] = selected_object.show_comp[component_index - 1];
                                        selected_object.show_comp[component_index - 1] = show_comp_temp;
                                    }

                                    if (component_index != compBases_count - 1)
                                    {
                                        if (GUILayout.Button("↓", GUIEditorSettings.button_0, GUILayout.Width(20f)))
                                        {
                                            selected_object.Components[component_index] = selected_object.Components[component_index + 1];
                                            selected_object.Components[component_index + 1] = component;

                                            bool show_comp_temp = selected_object.show_comp[component_index];
                                            selected_object.show_comp[component_index] = selected_object.show_comp[component_index + 1];
                                            selected_object.show_comp[component_index + 1] = show_comp_temp;
                                        }
                                    }
                                    else
                                        EditorGUILayout.LabelField("", GUILayout.Width(20));

                                    if (GUILayout.Button("Remove", GUIEditorSettings.button_0, GUILayout.Width(70f)))
                                    {
                                        RemoveItem(component_index);
                                        break;
                                    }
                                }
                                EditorGUILayout.EndHorizontal();
                                EditorGUILayout.EndVertical();
                                ShowComponentFields(component, component_index);
                            }
                            else
                            {
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.HelpBox("Component missing", MessageType.Warning);
                                if (GUILayout.Button("Remove", GUIEditorSettings.button_0, GUILayout.Width(70f)))
                                {
                                    RemoveItem(component_index);
                                    break;
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                        }

                        

                        EditorGUILayout.EndVertical();
                        #endregion
                    }

                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.BeginHorizontal(GUIEditorSettings.box_0_1);
                        {
                            should_show = EditorGUILayout.Foldout(should_show, "should show proc helper", true);
                            if (should_show)
                            {
                                GUILayout.Box("\n" + proc_mes);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                }

                
                
                #endregion
            }
            EditorGUILayout.EndHorizontal();


        }




        void AddComponent()
        {
            GenericMenu dropdownMenu = new GenericMenu();
            Type componentType = typeof(ComponentBase);
            List<Type> add_component_list = Assembly.GetAssembly(componentType)
                                     .GetTypes()
                                     .Where(type =>
                                     {
                                         return type.IsSubclassOf(componentType)
                                         && type.GetCustomAttribute<HideComponent>() == null;
                                     })
                                     .ToList();

            add_component_list.Sort((type1, type2) =>
            {
                return string.Compare(type1.GetCustomAttribute<ComponentAttribute>()?.GetPath() ?? type1.Name,
                                      type2.GetCustomAttribute<ComponentAttribute>()?.GetPath() ?? type2.Name);
            });


            AddItems(true);
            dropdownMenu.AddSeparator("");
            AddItems(false);
            dropdownMenu.ShowAsContext();

            void AddItems(bool with_component_attribute)
            {
                for (int i = 0; i < add_component_list.Count; i++)
                {
                    bool contains_attribute = add_component_list[i].GetCustomAttribute<ComponentAttribute>() != null;
                    if (with_component_attribute ? contains_attribute : (!contains_attribute))
                    {
                        string menuPath;

                        if (with_component_attribute)
                            menuPath = add_component_list[i].GetCustomAttribute<ComponentAttribute>().GetPath();
                        else
                            menuPath = add_component_list[i].Name;

                        bool on = selected_object.Components.Where(componentBase => componentBase.GetType() == add_component_list[i]).ToList().Count != 0;

                        if (on) 
                            dropdownMenu.AddItem(new GUIContent(menuPath), on, RemoveItem, add_component_list[i]);
                        else 
                            dropdownMenu.AddItem(new GUIContent(menuPath), on, AddItem, add_component_list[i]);
                    }
                }
            }

        }

        void AddItem(object componentType)
        {
            if (componentType != null)
            {
                selected_object.AddCmp((Type)componentType);
                SetEntityDirty();
            }
        }


        void RemoveItem(int index)
        {
            selected_object.RemoveCmp(selected_object.Components[index].GetType());
            SetEntityDirty();
        }
        void RemoveItem(object type)
        {
            for (int index = 0; index < selected_object.Components.Count; index++)
                if (selected_object.Components[index].GetType() == (Type)type)
                    RemoveItem(index);

            SetEntityDirty();
        }

        void SetEntityDirty()
        {
            EditorUtility.SetDirty(selected_object);
            Undo.RecordObject(selected_object, "Changed selected_object");

            if (selected_object != null)
            {
                proc_mes = ProcMesHelper.CreateSupportProcInfo(selected_object);
            }
        }

        void ShowComponentFields(ComponentBase component, int index)
        {
            if (selected_object.show_comp[index])
            {
                GUILayout.Space(3);
                EditorGUILayout.BeginVertical(GUIEditorSettings.box_0_0);
                {
                    EditorGUILayout.Separator();
                    Editor editor = Editor.CreateEditor(component);
                    editor.OnInspectorGUI();
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(20);
            }
        }

        void CheckCmpsOnCorrect()
        {
            ComponentBase[] components = selected_gameObject.GetComponents<ComponentBase>();
            List<ComponentBase> componentBases = selected_object.GetAllComponents();

            List<ComponentBase> NeedAdd = components.Where((cmp) => !componentBases.Contains(cmp)).ToList();
            List<Type> Dublicates = components.GroupBy(x => x?.GetType()).Where(g => g.Count() > 1).Select(y => y.Key).ToList();


            bool some_components_is_null = false;


            for (int i = 0; i < selected_object.Components.Count; i++)
                if (selected_object.Components[i] == null)
                    some_components_is_null = true;


            if (NeedAdd.Count > 0 || Dublicates.Count > 0 || some_components_is_null)
            {
                string mes = "";

                if (NeedAdd.Count > 0)
                    mes += "на GameObject присутствуют компоненты, которые не включены в Entity \n";
                if (Dublicates.Count > 0)
                    mes += "на GameObject присутствуют повторяющиеся ComponentBase (фиксите сами)\n";
                if (selected_object.Components.Contains(null))
                    mes += "на GameObject присутствуют missing компоненты\n";

                EditorGUILayout.BeginVertical(GUIEditorSettings.box_1_1);

                

                EditorGUILayout.HelpBox(mes, MessageType.Warning);

                if (GUILayout.Button("Fix it"))
                {
                    FixErrors();
                    SetEntityDirty();
                    Debug.Log("Испавлено");
                }

                EditorGUILayout.EndVertical();
            }


            void FixErrors()
            {
                for (int i = 0; i < NeedAdd.Count; i++)
                {
                    selected_object.Components.Add(NeedAdd[i]);
                    selected_object.show_comp.Add(false);
                }

                for (int i = 0; i < selected_object.Components.Count; i++)
                {
                    if (selected_object.Components[i] == null)
                    {
                        selected_object.Components.RemoveAt(i);
                    }
                }
            }
        }
    }

    #endregion CUSTOM_ENT_EDITOR_WINDOW_V3
}
