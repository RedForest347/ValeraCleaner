using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class DamageSignal : ISignal
{
    public DamageGiverCmp damageGiver;
    public EntityBase damageTaker;

    public DamageSignal(DamageGiverCmp damageGiver, EntityBase damageTaker)
    {
        this.damageGiver = damageGiver;
        this.damageTaker = damageTaker;
    }
}