using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreparateDamageSignal : ISignal
{
    public DamageSignal damageSignal;

    public RaycastHit[] raycastHits;

    public PreparateDamageSignal(DamageSignal damageSignal, RaycastHit[] raycastHits)
    {
        this.damageSignal = damageSignal;
        this.raycastHits = raycastHits;
    }
}
