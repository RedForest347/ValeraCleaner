using UnityEngine;
using RangerV;
#if UNITY_EDITOR
using UnityEditor;
#endif


[System.Serializable]
public class HealthCmpAdd
{
    public float health_regen;
}


[Component("Main/Health")]
public class HealthCmp : ComponentBase
{
    public float health;
    public HealthCmpAdd health_add;

    public HealthCmp()
    {

    }
}



#if UNITY_EDITOR

//[CustomPropertyDrawer(typeof(HealthComponent))]
//public class HealthComponentDrawer : PropertyDrawer
//{
//    // Draw the property inside the given rect
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        // Using BeginProperty / EndProperty on the parent property means that
//        // prefab override logic works on the entire property.
//        EditorGUI.BeginProperty(position, label, property);

//        // Draw label
//        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

//        // Don't make child fields be indented
//        var indent = EditorGUI.indentLevel;
//        EditorGUI.indentLevel = 0;

//        // Calculate rects
//        //var amountRect = new Rect(position.x, position.y, 30, position.height);
//        //var unitRect = new Rect(position.x + 35, position.y, 50, position.height);
//        //var nameRect = new Rect(position.x + 90, position.y, position.width - 90, position.height);
//        var healthRect = new Rect(position.x, position.y, 30, position.height);

//        // Draw fields - passs GUIContent.none to each so they are drawn without labels
//        //EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("amount"), GUIContent.none);
//        //EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
//        //EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("name"), GUIContent.none);
//        EditorGUI.PropertyField(healthRect, property.FindPropertyRelative("health"), GUIContent.none);

//        // Set indent back to what it was
//        EditorGUI.indentLevel = indent;

//        EditorGUI.EndProperty();
//    }
//}


//[CustomEditor(typeof(HealthComponent))]
//public class HealthComponentEditor : Editor
//{
//    //public void OnEnable()
//    //{
//    //    myScript = (ENT)target;
//    //}

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();
//        EditorGUILayout.LabelField("Health:");
//        EditorGUILayout.PropertyField(serializedObject.FindProperty("health"));
//        serializedObject.ApplyModifiedProperties();
//    }
//}

#endif