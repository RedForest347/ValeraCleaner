using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Zchager : MonoBehaviour
{
    public float height;

    void Start()
    {
        
    }

    void Update()
    {
        //gameObject.transform.position.Set(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.y - height);
        gameObject.transform.SetPositionAndRotation(
            new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, (gameObject.transform.position.y - height) * 0.0001f),
            gameObject.transform.rotation);
    }
}
