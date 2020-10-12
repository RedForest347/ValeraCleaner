using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGround1Starter : Starter
{
    public override void StarterSetup()
    {
        //GlobalSystemStorage.Add<PlayerControllerProc>();DelayDestroyProc
        GlobalSystemStorage.Add<DelayDestroyProc>();
        GlobalSystemStorage.Add<ColisionDamageProc>(); 
        GlobalSystemStorage.Add<ColisionProc>();

        GlobalSystemStorage.Add<PlayerControllerProc2>();
        GlobalSystemStorage.Add<GunProc>();
        GlobalSystemStorage.Add<CameraFollowProc>();
        GlobalSystemStorage.Add<CorutineManager>();
    }
}
