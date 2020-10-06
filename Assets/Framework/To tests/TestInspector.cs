using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Test))]
public class TestInspector : Editor
{
    Test obj;

    private void OnEnable()
    {
        obj = (Test)target;
    }


    public void DDD()
    {
        // когда открыт префаб (выбран в файлах или открыта его сцена редактирования, buildIndex будет равен -1)
        EditorGUILayout.HelpBox("scene.buildIndex = " + Selection.activeGameObject.scene.buildIndex, 
            MessageType.Info);
    }
}
