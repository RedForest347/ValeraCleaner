using UnityEngine;
using RangerV;
using System;
using System.Collections.Generic;


public enum Collision2DActionType
{
    Enter = 1,
    Exit = 2,
    Stay = 3
}


[Component("Engine/Collision 2D Component", "BoxCollider2D Icon")]
public class Collision2DCmp : ComponentBase, ICustomAwake
{
    [HideInInspector, SerializeField]
    private int entity;

    public bool DestroyOnCollision;
    public List<Collider2D> IgnoreColliders;

    public Collision2DCmp()
    {
        /*Debug.Log("CollisionComponent " + entity + "\n" +
            "onCollisionEnterActionDictionary.Count = " + onCollisionEnterActionDictionary.Count +
            "onCollisionExitActionDictionary Count = " + onCollisionExitActionDictionary.Count +
            "onCollisionStayActionDictionary Count = " + onCollisionStayActionDictionary.Count
            );*/
    }

    public void OnAwake()
    {
        entity = gameObject.GetComponent<Entity>().entity;
    }



    #region LIGHT_VERSION

    //public delegate void collision2DAction(Collision collision, int entity);
    //event collision2DAction onCollisionEnterAction;
    //event collision2DAction onCollisionExitAction;
    //event collision2DAction onCollisionStayAction;
    //public void AddOncollision2DAction(collision2DAction action, Collision2DActionType collisionType)
    //{
    //    switch (collisionType)
    //    {
    //        case Collision2DActionType.Enter: onCollisionEnterAction += action; break;
    //        case Collision2DActionType.Exit: onCollisionExitAction += action; break;
    //        case Collision2DActionType.Stay: onCollisionStayAction += action; break;
    //    }
    //}
    //public void RemoveOncollision2DAction(collision2DAction action, Collision2DActionType collisionType)
    //{
    //    switch (collisionType)
    //    {
    //        case Collision2DActionType.Enter: onCollisionEnterAction -= action; break;
    //        case Collision2DActionType.Exit: onCollisionExitAction -= action; break;
    //        case Collision2DActionType.Stay: onCollisionStayAction -= action; break;
    //    }
    //}
    //private void OnCollisionEnter(Collision collision) { onCollisionEnterAction?.Invoke(collision, entity); }
    //private void OnCollisionExit(Collision collision) { onCollisionExitAction?.Invoke(collision, entity); }
    //private void OnCollisionStay(Collision collision) { onCollisionStayAction?.Invoke(collision, entity); }

    //public delegate void collision2DAction(Collider2D Collider2D, int entity);
    //event collision2DAction onTriggerEnterAction;
    //event collision2DAction onTriggerExitAction;
    //event collision2DAction onTriggerStayAction;
    //public void AddOncollision2DAction(collision2DAction action, Collision2DActionType collisionType)
    //{
    //    switch (collisionType)
    //    {
    //        case Collision2DActionType.Enter: onTriggerEnterAction += action; break;
    //        case Collision2DActionType.Exit: onTriggerExitAction += action; break;
    //        case Collision2DActionType.Stay: onTriggerStayAction += action; break;
    //    }
    //}
    //public void RemoveOncollision2DAction(collision2DAction action, Collision2DActionType collisionType)
    //{
    //    switch (collisionType)
    //    {
    //        case Collision2DActionType.Enter: onTriggerEnterAction -= action; break;
    //        case Collision2DActionType.Exit: onTriggerExitAction -= action; break;
    //        case Collision2DActionType.Stay: onTriggerStayAction -= action; break;
    //    }
    //}
    //private void OnTriggerEnter(Collider2D other) { onTriggerExitAction?.Invoke(other, entity); }
    //private void OnTriggerExit(Collider2D other) { onTriggerExitAction?.Invoke(other, entity); }
    //private void OnTriggerStay(Collider2D other) { onTriggerStayAction?.Invoke(other, entity); }

    #endregion


    #region HEAVY_VERSION

    public delegate void collision2DAction(Collision2D Collider2D, int entity);
    [SerializeField, HideInInspector]
    private Dictionary<collision2DAction, Collider2D[]> onCollisionEnterActionDictionary = new Dictionary<collision2DAction, Collider2D[]>();
    [SerializeField, HideInInspector]
    private Dictionary<collision2DAction, Collider2D[]> onCollisionExitActionDictionary = new Dictionary<collision2DAction, Collider2D[]>();
    [SerializeField, HideInInspector]
    private Dictionary<collision2DAction, Collider2D[]> onCollisionStayActionDictionary = new Dictionary<collision2DAction, Collider2D[]>();
    private void OnCollision2DAction(Collision2D other, Dictionary<collision2DAction, Collider2D[]> triggerDictionary)
    {
        foreach (var action in triggerDictionary)
        {
            for (int Collider2D = 0; Collider2D < action.Value.Length; Collider2D++)
                if (other.collider == action.Value[Collider2D])
                    return;
            for (int Collider2D = 0; Collider2D < IgnoreColliders.Count; Collider2D++)
                if (other.collider == IgnoreColliders[Collider2D])
                    return;
            action.Key(other, entity);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) { OnCollision2DAction(collision, onCollisionEnterActionDictionary); }
    private void OnCollisionExit2D(Collision2D collision) { OnCollision2DAction(collision, onCollisionExitActionDictionary);   }
    private void OnCollisionStay2D(Collision2D collision) { OnCollision2DAction(collision, onCollisionStayActionDictionary);   }
    public bool AddOnCollision2DAction(collision2DAction action, Collision2DActionType collisionType, Collider2D[] ignorCollider2Ds)
    {
        switch (collisionType)
        {
            case Collision2DActionType.Enter:
                if (onCollisionEnterActionDictionary.ContainsKey(action))
                    return false;
                onCollisionEnterActionDictionary.Add(action, ignorCollider2Ds);
                return true;
            case Collision2DActionType.Exit:
                if (onCollisionExitActionDictionary.ContainsKey(action))
                    return false;
                onCollisionExitActionDictionary.Add(action, ignorCollider2Ds);
                return true;
            case Collision2DActionType.Stay:
                if (onCollisionStayActionDictionary.ContainsKey(action))
                    return false;
                onCollisionStayActionDictionary.Add(action, ignorCollider2Ds);
                return true;
            default:
                return false;
        }
    }
    public bool AddOnCollision2DAction(collision2DAction action, Collision2DActionType collisionType, Collider2D ignorCollider2D)
    {
        return AddOnCollision2DAction(action, collisionType, new Collider2D[] { ignorCollider2D });
    }
    public bool AddOnCollision2DAction(collision2DAction action, Collision2DActionType collisionType)
    {
        return AddOnCollision2DAction(action, collisionType, new Collider2D[0]);
    }
    public void RemoveOnCollision2DAction(collision2DAction action, Collision2DActionType collisionType)
    {
        switch (collisionType)
        {
            case Collision2DActionType.Enter:
                onCollisionEnterActionDictionary.Remove(action);
                break;
            case Collision2DActionType.Exit:
                onCollisionExitActionDictionary.Remove(action);
                break;
            case Collision2DActionType.Stay:
                onCollisionStayActionDictionary.Remove(action);
                break;
        }
    }
    public void RemoveAllOnCollision2DActions(collision2DAction action)
    {
        if (onCollisionEnterActionDictionary.ContainsKey(action)) onCollisionEnterActionDictionary.Remove(action);
        if (onCollisionExitActionDictionary.ContainsKey(action)) onCollisionExitActionDictionary.Remove(action);
        if (onCollisionStayActionDictionary.ContainsKey(action)) onCollisionStayActionDictionary.Remove(action);
    }





    public delegate void triggerAction(Collider2D Collider2D, int entity);
    [SerializeField, HideInInspector]
    private Dictionary<triggerAction, Collider2D[]> onTriggerEnterActionDictionary = new Dictionary<triggerAction, Collider2D[]>();
    [SerializeField, HideInInspector]
    private Dictionary<triggerAction, Collider2D[]> onTriggerExitActionDictionary = new Dictionary<triggerAction, Collider2D[]>();
    [SerializeField, HideInInspector]
    private Dictionary<triggerAction, Collider2D[]> onTriggerStayActionDictionary = new Dictionary<triggerAction, Collider2D[]>();
    private void OnTriggerAction(Collider2D other, Dictionary<triggerAction, Collider2D[]> triggerDictionary)
    {
        foreach (var action in triggerDictionary)
        {
            for (int Collider2D = 0; Collider2D < action.Value.Length; Collider2D++)
                if (other == action.Value[Collider2D])
                    return;
            for (int Collider2D = 0; Collider2D < IgnoreColliders.Count; Collider2D++)
                if (other == IgnoreColliders[Collider2D])
                    return;
            action.Key(other, entity);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) { OnTriggerAction(other, onTriggerEnterActionDictionary); }
    private void OnTriggerExit2D(Collider2D other) { OnTriggerAction(other, onTriggerExitActionDictionary); }
    private void OnTriggerStay2D(Collider2D other) { OnTriggerAction(other, onTriggerStayActionDictionary); }
    public bool AddOnTriggerAction(triggerAction action, Collision2DActionType collisionType, Collider2D[] ignorCollider2Ds)
    {
        switch (collisionType)
        {
            case Collision2DActionType.Enter:
                if (onTriggerEnterActionDictionary.ContainsKey(action))
                    return false;
                onTriggerEnterActionDictionary.Add(action, ignorCollider2Ds);
                return true;
            case Collision2DActionType.Exit:
                if (onTriggerExitActionDictionary.ContainsKey(action))
                    return false;
                onTriggerExitActionDictionary.Add(action, ignorCollider2Ds);
                return true;
            case Collision2DActionType.Stay:
                if (onTriggerStayActionDictionary.ContainsKey(action))
                    return false;
                onTriggerStayActionDictionary.Add(action, ignorCollider2Ds);
                return true;
            default:
                return false;
        }
    }
    public bool AddOnTriggerAction(triggerAction action, Collision2DActionType collisionType, Collider2D ignorCollider2D)
    {
        return AddOnTriggerAction(action, collisionType, new Collider2D[] { ignorCollider2D });
    }
    public bool AddOnTriggerAction(triggerAction action, Collision2DActionType collisionType)
    {
        return AddOnTriggerAction(action, collisionType, new Collider2D[0]);
    }
    public void RemoveOnTriggerAction(triggerAction action, Collision2DActionType collisionType)
    {
        switch (collisionType)
        {
            case Collision2DActionType.Enter:
                onTriggerEnterActionDictionary.Remove(action);
                break;
            case Collision2DActionType.Exit:
                onTriggerExitActionDictionary.Remove(action);
                break;
            case Collision2DActionType.Stay:
                onTriggerStayActionDictionary.Remove(action);
                break;
        }
    }
    public void RemoveAllOnTriggerActions(triggerAction action)
    {
        if (onTriggerEnterActionDictionary.ContainsKey(action)) onTriggerEnterActionDictionary.Remove(action);
        if (onTriggerExitActionDictionary.ContainsKey(action)) onTriggerExitActionDictionary.Remove(action);
        if (onTriggerStayActionDictionary.ContainsKey(action)) onTriggerStayActionDictionary.Remove(action);
    }

    #endregion


}