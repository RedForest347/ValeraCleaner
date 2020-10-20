using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseChestProc : ProcessingBase, ICustomStart, ICustomDisable
{
    Group ChestGroup = Group.Create(new ComponentsList<OpenCloseChestCmp, InteractiveCmp>());



    public void OnStart()
    {
        ChestGroup.InitEvents(OnAdd, OnRemove);
    }

    void OnAdd(int chest)
    {
        Storage.GetComponent<InteractiveCmp>(chest).OnSelected += OnSelectChest;
    }

    void OnRemove(int chest)
    {
        Storage.GetComponent<InteractiveCmp>(chest).OnSelected -= OnSelectChest;
    }

    void OnSelectChest(int chest)
    {
        OpenCloseChestCmp chestCmp = Storage.GetComponent<OpenCloseChestCmp>(chest);

        if (chestCmp.CloseChestSprite.enabled)
        {
            chestCmp.CloseChestSprite.enabled = false;
            chestCmp.OpenChestSprite.enabled = true;
        }
        else
        {
            chestCmp.CloseChestSprite.enabled = true;
            chestCmp.OpenChestSprite.enabled = false;
        }

    }


    public void OnCustomDisable()
    {
        ChestGroup.DeinitEvents(OnAdd, OnRemove);
    }
}
