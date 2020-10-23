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
                        interactiveCmp.OnSelect?.Invoke(interactiveCmp.entity);
                        interactiveCmp.SelectUE.Invoke();
                    }

                    interactiveCmp.select_in_current_frame = true;
                }
                else
                {
                    interactiveCmp.select_in_current_frame = false;
                }

                CheckInteractiveCmpStates(interactiveCmp);
            }
        }
    }

    void CheckInteractiveCmpStates(InteractiveCmp interactiveCmp)
    {
        if (interactiveCmp.select_in_current_frame)
        {
            if (interactiveCmp.was_select_in_previous_frame)
            {
                interactiveCmp.OnZoneStay?.Invoke(interactiveCmp.entity);
                interactiveCmp.StayUE.Invoke();
            }
            else
            {
                interactiveCmp.OnZoneEnter?.Invoke(interactiveCmp.entity);
                interactiveCmp.EnterUE.Invoke();
            }
        }
        else if (interactiveCmp.was_select_in_previous_frame)
        {
            interactiveCmp.OnZoneExit?.Invoke(interactiveCmp.entity);
            interactiveCmp.ExitUE.Invoke();
        }

        interactiveCmp.was_select_in_previous_frame = interactiveCmp.select_in_current_frame;
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
