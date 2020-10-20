using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class InteractiveStarter : Starter
{
    public override void StarterSetup()
    {
        GlobalSystemStorage.Add<PlayerControllerProc>();
        GlobalSystemStorage.Add<InteractiveProc>();
        GlobalSystemStorage.Add<MoverProc>();
        GlobalSystemStorage.Add<OpenCloseChestProc>();
    }
}
