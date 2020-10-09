using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AI;
using UnityEngine;
using UnityEngine.AI;

public class move : MonoBehaviour
{
    public Transform target;
    AIPath aiPath;
    Seeker seeker;
    //NavMeshAgent agent;


    void Start()
    {
        NavMeshVisualizationSettings.showNavigation = 2;
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();

    }

    void Update()
    {
        //aiPath.;
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("путь начат");
            seeker.StartPath(transform.position, target.position);
        }
    }
}
