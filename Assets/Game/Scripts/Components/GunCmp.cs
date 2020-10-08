using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCmp : ComponentBase
{
    [HideInInspector]
    public bool shooting;

    public GameObject Bullet;
    public float rateOfFire;

    [HideInInspector]
    public float timer = 0;
}
