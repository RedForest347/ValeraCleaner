using UnityEngine;
using RangerV;
using System;
using System.Collections.Generic;


public enum CollisionActionType
{
    Enter = 1,
    Exit = 2,
    Stay = 3
}


[Component("Engine/Collision Component", "BoxCollider Icon")]
public class CollisionCmp : ComponentBase
{
    public bool DestroyOnCollision;
    public List<Collider> IgnoreColliders;

    public CollisionCmp()
    {
        /*Debug.Log("CollisionComponent " + entity + "\n" +
            "onCollisionEnterActionDictionary.Count = " + onCollisionEnterActionDictionary.Count +
            "onCollisionExitActionDictionary Count = " + onCollisionExitActionDictionary.Count +
            "onCollisionStayActionDictionary Count = " + onCollisionStayActionDictionary.Count
            );*/
    }

    #region LIGHT_VERSION

    //public delegate void collisionAction(Collision collision, int entity);
    //event collisionAction onCollisionEnterAction;
    //event collisionAction onCollisionExitAction;
    //event collisionAction onCollisionStayAction;
    //public void AddOnCollisionAction(collisionAction action, CollisionActionType collisionType)
    //{
    //    switch (collisionType)
    //    {
    //        case CollisionActionType.Enter: onCollisionEnterAction += action; break;
    //        case CollisionActionType.Exit: onCollisionExitAction += action; break;
    //        case CollisionActionType.Stay: onCollisionStayAction += action; break;
    //    }
    //}
    //public void RemoveOnCollisionAction(collisionAction action, CollisionActionType collisionType)
    //{
    //    switch (collisionType)
    //    {
    //        case CollisionActionType.Enter: onCollisionEnterAction -= action; break;
    //        case CollisionActionType.Exit: onCollisionExitAction -= action; break;
    //        case CollisionActionType.Stay: onCollisionStayAction -= action; break;
    //    }
    //}
    //private void OnCollisionEnter(Collision collision) { onCollisionEnterAction?.Invoke(collision, entity); }
    //private void OnCollisionExit(Collision collision) { onCollisionExitAction?.Invoke(collision, entity); }
    //private void OnCollisionStay(Collision collision) { onCollisionStayAction?.Invoke(collision, entity); }

    //public delegate void collisionAction(Collider collider, int entity);
    //event collisionAction onTriggerEnterAction;
    //event collisionAction onTriggerExitAction;
    //event collisionAction onTriggerStayAction;
    //public void AddOncollisionAction(collisionAction action, CollisionActionType collisionType)
    //{
    //    switch (collisionType)
    //    {
    //        case CollisionActionType.Enter: onTriggerEnterAction += action; break;
    //        case CollisionActionType.Exit: onTriggerExitAction += action; break;
    //        case CollisionActionType.Stay: onTriggerStayAction += action; break;
    //    }
    //}
    //public void RemoveOncollisionAction(collisionAction action, CollisionActionType collisionType)
    //{
    //    switch (collisionType)
    //    {
    //        case CollisionActionType.Enter: onTriggerEnterAction -= action; break;
    //        case CollisionActionType.Exit: onTriggerExitAction -= action; break;
    //        case CollisionActionType.Stay: onTriggerStayAction -= action; break;
    //    }
    //}
    //private void OnTriggerEnter(Collider other) { onTriggerExitAction?.Invoke(other, entity); }
    //private void OnTriggerExit(Collider other) { onTriggerExitAction?.Invoke(other, entity); }
    //private void OnTriggerStay(Collider other) { onTriggerStayAction?.Invoke(other, entity); }

    #endregion


    #region HEAVY_VERSION

    public delegate void collisionAction(Collision collider, int entity);
    [SerializeField, HideInInspector]
    private Dictionary<collisionAction, Collider[]> onCollisionEnterActionDictionary = new Dictionary<collisionAction, Collider[]>();
    [SerializeField, HideInInspector]
    private Dictionary<collisionAction, Collider[]> onCollisionExitActionDictionary = new Dictionary<collisionAction, Collider[]>();
    [SerializeField, HideInInspector]
    private Dictionary<collisionAction, Collider[]> onCollisionStayActionDictionary = new Dictionary<collisionAction, Collider[]>();
    private void OnCollisionAction(Collision other, Dictionary<collisionAction, Collider[]> triggerDictionary)
    {
        //Debug.Log("OnCollisionAction");
        foreach (var action in triggerDictionary)
        {
            for (int collider = 0; collider < action.Value.Length; collider++)
                if (other.collider == action.Value[collider])
                    return;
            for (int collider = 0; collider < IgnoreColliders.Count; collider++)
                if (other.collider == IgnoreColliders[collider])
                    return;
            action.Key(other, entity);
        }
    }
    private void OnCollisionEnter(Collision collision) { OnCollisionAction(collision, onCollisionEnterActionDictionary); }
    private void OnCollisionExit(Collision collision) { OnCollisionAction(collision, onCollisionExitActionDictionary);   }
    private void OnCollisionStay(Collision collision) { OnCollisionAction(collision, onCollisionStayActionDictionary);   }
    public bool AddOnCollisionAction(collisionAction action, CollisionActionType collisionType, Collider[] ignorColliders)
    {
        switch (collisionType)
        {
            case CollisionActionType.Enter:
                if (onCollisionEnterActionDictionary.ContainsKey(action))
                    return false;
                onCollisionEnterActionDictionary.Add(action, ignorColliders);
                return true;
            case CollisionActionType.Exit:
                if (onCollisionExitActionDictionary.ContainsKey(action))
                    return false;
                onCollisionExitActionDictionary.Add(action, ignorColliders);
                return true;
            case CollisionActionType.Stay:
                if (onCollisionStayActionDictionary.ContainsKey(action))
                    return false;
                onCollisionStayActionDictionary.Add(action, ignorColliders);
                return true;
            default:
                return false;
        }
    }
    public bool AddOnCollisionAction(collisionAction action, CollisionActionType collisionType, Collider ignorCollider)
    {
        return AddOnCollisionAction(action, collisionType, new Collider[] { ignorCollider });
    }
    public bool AddOnCollisionAction(collisionAction action, CollisionActionType collisionType)
    {
        return AddOnCollisionAction(action, collisionType, new Collider[0]);
    }
    public void RemoveOnCollisionAction(collisionAction action, CollisionActionType collisionType)
    {
        switch (collisionType)
        {
            case CollisionActionType.Enter:
                onCollisionEnterActionDictionary.Remove(action);
                break;
            case CollisionActionType.Exit:
                onCollisionExitActionDictionary.Remove(action);
                break;
            case CollisionActionType.Stay:
                onCollisionStayActionDictionary.Remove(action);
                break;
        }
    }
    public void RemoveAllOnCollisionActions(collisionAction action)
    {
        if (onCollisionEnterActionDictionary.ContainsKey(action)) onCollisionEnterActionDictionary.Remove(action);
        if (onCollisionExitActionDictionary.ContainsKey(action)) onCollisionExitActionDictionary.Remove(action);
        if (onCollisionStayActionDictionary.ContainsKey(action)) onCollisionStayActionDictionary.Remove(action);
    }


    public delegate void triggerAction(Collider collider, int entity);
    [SerializeField, HideInInspector]
    private Dictionary<triggerAction, Collider[]> onTriggerEnterActionDictionary = new Dictionary<triggerAction, Collider[]>();
    [SerializeField, HideInInspector]
    private Dictionary<triggerAction, Collider[]> onTriggerExitActionDictionary = new Dictionary<triggerAction, Collider[]>();
    [SerializeField, HideInInspector]
    private Dictionary<triggerAction, Collider[]> onTriggerStayActionDictionary = new Dictionary<triggerAction, Collider[]>();
    private void OnTriggerAction(Collider other, Dictionary<triggerAction, Collider[]> triggerDictionary)
    {
        
        foreach (var action in triggerDictionary)
        {
            for (int collider = 0; collider < action.Value.Length; collider++)
                if (other == action.Value[collider])
                    return;
            for (int collider = 0; collider < IgnoreColliders.Count; collider++)
                if (other == IgnoreColliders[collider])
                    return;

            //Debug.Log("OnTriggerAction");
            action.Key(other, entity);
        }
    }
    private void OnTriggerEnter(Collider other) { OnTriggerAction(other, onTriggerEnterActionDictionary); }
    private void OnTriggerExit(Collider other) { OnTriggerAction(other, onTriggerExitActionDictionary); }
    private void OnTriggerStay(Collider other) { OnTriggerAction(other, onTriggerStayActionDictionary); }
    public bool AddOnTriggerAction(triggerAction action, CollisionActionType collisionType, Collider[] ignorColliders)
    {
        switch (collisionType)
        {
            case CollisionActionType.Enter:
                if (onTriggerEnterActionDictionary.ContainsKey(action))
                    return false;
                onTriggerEnterActionDictionary.Add(action, ignorColliders);
                return true;
            case CollisionActionType.Exit:
                if (onTriggerExitActionDictionary.ContainsKey(action))
                    return false;
                onTriggerExitActionDictionary.Add(action, ignorColliders);
                return true;
            case CollisionActionType.Stay:
                if (onTriggerStayActionDictionary.ContainsKey(action))
                    return false;
                onTriggerStayActionDictionary.Add(action, ignorColliders);
                return true;
            default:
                return false;
        }
    }

    public bool AddOnTriggerAction(triggerAction action, CollisionActionType collisionType, Collider ignorCollider)
    {
        return AddOnTriggerAction(action, collisionType, new Collider[] { ignorCollider });
    }

    public bool AddOnTriggerAction(triggerAction action, CollisionActionType collisionType)
    {
        return AddOnTriggerAction(action, collisionType, new Collider[0]);
    }

    public void RemoveOnTriggerAction(triggerAction action, CollisionActionType collisionType)
    {
        switch (collisionType)
        {
            case CollisionActionType.Enter:
                onTriggerEnterActionDictionary.Remove(action);
                break;
            case CollisionActionType.Exit:
                onTriggerExitActionDictionary.Remove(action);
                break;
            case CollisionActionType.Stay:
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