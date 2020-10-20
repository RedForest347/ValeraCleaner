﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Zchager : MonoBehaviour
{
    public float height;
    public bool OnlyEditMode;

    void Update()
    {
        bool Zexecute = true;

        if (OnlyEditMode)
            Zexecute = (Application.isEditor && !Application.isPlaying);

        if (Zexecute)
            ZChange();
    }

    void ZChange()
    {
        //gameObject.transform.position.Set(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.y - height);
        gameObject.transform.SetPositionAndRotation(
        new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.y - height),
        gameObject.transform.rotation);
    }
    
}
