using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RangerV;

public class OpenCloseChestCmp : ComponentBase, ICustomAwake
{
    public SpriteRenderer OpenChestSprite;
    public SpriteRenderer CloseChestSprite;

    public void OnAwake()
    {
        OpenChestSprite.enabled = false;
        CloseChestSprite.enabled = true;
    }
}
