using UnityEngine;

[System.Serializable]
public struct MeleeAttackInfo
{
    public string attackName;
    public float damage;
    public float pushForce;
    public LayerMask layerMask;

    public float attackDelay; //?
    public float timeBetweenAttacks; //?
    public PatyManager[] effects;
    public AttackZone attackZone;

    [HideInInspector]
    public float timeAfterLastAttack;
}