using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCmp : ComponentBase
{
    private void Update()
    {
        gameObject.transform.Translate(Vector3.right * Time.deltaTime * 10);
    }
}
