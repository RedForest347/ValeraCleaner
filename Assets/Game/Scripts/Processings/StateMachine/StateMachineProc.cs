using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineProc : ProcessingBase, ICustomStart, ICustomUpdate, ICustomFixedUpdate, ICustomLateUpdate
{
    Group SMGroup = Group.Create(new ComponentsList<StateMachineCmp>());


    public void OnStart()
    {

    }


    public void CustomUpdate()
    {
        foreach (int SM in SMGroup)
        {
            Storage.GetComponent<StateMachineCmp>(SM).StateUpdate();
        }
    }

    public void CustomLateUpdate()
    {
        foreach (int SM in SMGroup)
        {
            Storage.GetComponent<StateMachineCmp>(SM).StateLateUpdate();
        }
    }

    public void CustomFixedUpdate()
    {
        foreach (int SM in SMGroup)
        {
            Storage.GetComponent<StateMachineCmp>(SM).StateFixedUpdate();
        }
    }


}
