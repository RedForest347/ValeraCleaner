using UnityEngine;
using RangerV;
using System;

public class InteractiveProc : ProcessingBase, ICustomUpdate
{
    Group InteractiveGroup = Group.Create(new ComponentsList<InteractiveCmp>());
    Group PlayerGroup = Group.Create(new ComponentsList<PlayerControllerCmp>());

    public void CustomUpdate()
    {
        foreach (int interactive in InteractiveGroup)
        {
            foreach (int player in PlayerGroup)
            {
                InteractiveCmp interactiveCmp = Storage.GetComponent<InteractiveCmp>(interactive);

                if (InZone(player, interactiveCmp))
                {
                    if (Input.GetKeyDown(interactiveCmp.select_key))
                    {
                        interactiveCmp.Select();
                    }
                }
            }
        }
    }


    bool InZone(int player_ent, InteractiveCmp interactiveObj)
    {
        Vector3 PlayerPos = EntityBase.GetEntity(player_ent).transform.position;

        Vector2 distance_betveen = interactiveObj.transform.position - PlayerPos;

        if (distance_betveen.magnitude <= interactiveObj.active_distance)
        {
            Vector2 from = ((Vector2)interactiveObj.transform.up).Rotate(interactiveObj.angle_offset);
            Vector2 to = PlayerPos - interactiveObj.transform.position;
            float angle = Math.Abs(Vector2.SignedAngle(from, to));

            if (angle <= interactiveObj.active_angle / 2) 
                return true;
        }
        return false;
    }
}
