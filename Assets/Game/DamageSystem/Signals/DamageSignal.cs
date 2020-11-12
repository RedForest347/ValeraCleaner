using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

/// <обрабатывается cref="TakeDamageProc"/>
public class DamageSignal : ISignal
{
    public Attack attackParameters;
    public EntityBase attaker;
    public EntityBase attackTarget;

    public DamageSignal(Attack attackParameters, EntityBase attackTarget, EntityBase attaker)
    {
        this.attackParameters = attackParameters;
        this.attackTarget = attackTarget;
        this.attaker = attaker;
    }
}