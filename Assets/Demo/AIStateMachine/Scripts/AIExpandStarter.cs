using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class AIExpandStarter : Starter
{
    public override void StarterSetup()
    {
        
        GlobalSystemStorage.Add<MoverProc>();
        GlobalSystemStorage.Add<GunProc>();
        GlobalSystemStorage.Add<PlayerControllerProc>();
        GlobalSystemStorage.Add<CameraFollowProc>();


        //GlobalSystemStorage.Add<AIFindTargetProc>();
        GlobalSystemStorage.Add<AIMoveProc>();
        GlobalSystemStorage.Add<StateMachineProc>();

    }
}
