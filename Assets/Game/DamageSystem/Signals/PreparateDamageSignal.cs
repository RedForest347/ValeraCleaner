using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <обрабатывается cref="TakeDamageProc"/>
public class PreparateDamageSignal : ISignal
{
    public AttackCmp attackSender;

    public RaycastHit[] raycastHits;

    public PreparateDamageSignal(AttackCmp attackSender, RaycastHit[] raycastHits)
    {
        this.attackSender = attackSender;
        this.raycastHits = raycastHits;
    }
}
