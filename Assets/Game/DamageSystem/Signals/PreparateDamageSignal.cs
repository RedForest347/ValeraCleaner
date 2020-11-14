using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <обрабатывается cref="TakeDamageProc"/>
public class PreparateDamageSignal : ISignal
{
    public MeleeAttackCmp attackSender;

    public RaycastHit[] raycastHits;

    public PreparateDamageSignal(MeleeAttackCmp attackSender, RaycastHit[] raycastHits)
    {
        this.attackSender = attackSender;
        this.raycastHits = raycastHits;
    }
}
