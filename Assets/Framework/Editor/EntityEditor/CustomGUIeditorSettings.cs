#if DEBUG

using UnityEditor;
using UnityEngine;

namespace RangerV
{
    [CreateAssetMenu(fileName = "Custom GUI editor settings", menuName = "Custom editor/Custom GUI editor settings")]
    public class CustomGUIeditorSettings : ScriptableObject
    {
        public bool useBox;
        public bool useButtons;

        [Space]
        public Color mainUIColor;
        public Color mainContentUIColor;
        public Color mainBackgroundUIColor;



        
        [HideInInspector] public GUIStyle headerLabel;

        [HideInInspector] public GUIStyle box_0_0;
        [HideInInspector] public GUIStyle box_0_1;
        [HideInInspector] public GUIStyle box_1_0;
        [HideInInspector] public GUIStyle box_1_1;

        [HideInInspector] public GUIStyle button_0;
        //-------------------------------------------------------//
        [Space]
        [SerializeField] private GUIStyle HeaderLabel;

        [SerializeField] private GUIStyle Box1;

        [SerializeField] private GUIStyle Button1;




        public void SkinInitialize()
        {
            headerLabel = new GUIStyle(EditorStyles.boldLabel);
            headerLabel.alignment = TextAnchor.MiddleCenter;

            box_0_1 = new GUIStyle(useBox ? Box1 : GUI.skin.box);

            box_0_0 = new GUIStyle(useBox ? Box1 : GUI.skin.box);
            box_0_0.normal.background = null;

            box_1_1 = new GUIStyle(useBox ? Box1 : GUI.skin.box);
            box_1_1.padding.left = 10;
            box_1_1.padding.right = 8;
            box_1_1.padding.bottom = 8;
            box_1_1.padding.top = 8;

            box_1_0 = new GUIStyle(box_1_1);
            box_1_0.normal.background = null;

            button_0 = new GUIStyle(useButtons ? Button1 : GUI.skin.button);

        }


    }
}

#endif
