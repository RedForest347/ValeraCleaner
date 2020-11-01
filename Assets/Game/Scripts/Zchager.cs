using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Zchager : MonoBehaviour
{
    public float height;
    public bool OnlyEditMode;

    float previous_heigth;

    private void Start()
    {
        /**Transform[] Objs = FindObjectsOfType<Transform>();

        for (int i = 0; i < Objs.Length; i++)
        {
            if (Objs[i].transform.position.z != 0)
            {
                if (Objs[i].GetComponent<Zchager>() != null && 
                    (Objs[i].GetComponentInChildren<SpriteRenderer>() != null || Objs[i].GetComponent<SpriteRenderer>() != null))
                {
                    Debug.Log("объект " + Objs[i].name + " в координатах " + Objs[i].transform.position + " имеет ненулевую высоту");
                }
            }
        }*/
    }

    private void Update()
    {
        //if (Application.isEditor)
        //Debug.Log("Application.isEditor");
        if (previous_heigth != height || Application.isEditor)
        {
            previous_heigth = height;
            ZChange();
        }
    }

    void ZChange()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, -height * 0.0001f);
    }
}
