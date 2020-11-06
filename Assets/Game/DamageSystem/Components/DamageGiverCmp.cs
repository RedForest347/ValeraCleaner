using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class DamageGiverCmp : ComponentBase
{
    public float damage;

    [Tooltip("in seconds"), Min(0)]
    public float time_between_attacks;
    [HideInInspector]
    public float time_after_last_attack;
    public bool readyToAttack { get => time_after_last_attack >= time_between_attacks; }
}
