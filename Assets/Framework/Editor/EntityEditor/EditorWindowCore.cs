#if DEBUG

using UnityEditor;
using UnityEngine;


namespace RangerV
{
    public class EditorWindowCore : EditorWindow
    {
        protected Vector2 scroll;
        protected CustomGUIeditorSettings GUIEditorSettings;
        protected string stylePath;

        protected void OnEnable()
        {
            GUIEditorSettings = (CustomGUIeditorSettings)Resources.Load("EntityGUIeditorSettings");
            stylePath = AssetDatabase.GetAssetPath(GUIEditorSettings);
        }
        protected void OnInspectorUpdate()
        {

        }
        protected void OnSelectionChange()
        {
            
        }
        protected void OnGUI()
        {
            
        }

    }
}

#endif