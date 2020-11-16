using UnityEngine;


[System.Serializable]
public struct ExplosionAttackInfo
{
    public string attackName;
    [Min(0)]
    public float radius;
    public ValueSpread damageSpread;
    public ValueSpread pushForseSpread;
    public LayerMask layerMask;

    public float attackDelay; //?
    public float timeBetweenAttacks; //?
    public EffectBase[] effects;
    public AttackZone attackZone;

    [HideInInspector]
    public float timeAfterLastAttack;

    [HideInInspector]
    public float calculatedDamage;
    [HideInInspector]
    public float calculatedPushForce;
}

[System.Serializable]
public struct ValueSpread
{
    public float min;
    public float max;
}