using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

/// <обрабатывается cref="TakeDamageProc"/>
public class DamageSignal : ISignal
{
    public AttackInfo attackInfo;
    public EntityBase attaker;
    public EntityBase attackTarget;

    public DamageSignal(AttackInfo attackInfo, EntityBase attackTarget, EntityBase attaker)
    {
        this.attackInfo = attackInfo;
        this.attackTarget = attackTarget;
        this.attaker = attaker;
    }
}