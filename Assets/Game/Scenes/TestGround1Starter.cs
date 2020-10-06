using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGround1Starter : Starter
{
    public override void StarterSetup()
    {
        GlobalSystemStorage.Add<MoveProc>();
    }
}
