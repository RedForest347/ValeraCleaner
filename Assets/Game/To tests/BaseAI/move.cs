using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.AI;
using UnityEngine;
using UnityEngine.AI;
using Stopwatch = System.Diagnostics.Stopwatch;

public class move : MonoBehaviour
{
    public Transform target;
    public GameObject Cube;
    public AstarPath astarPath;
    AIPath aiPath;
    Seeker seeker;
    AILerp aILerp;
    //NavMeshAgent agent;


    void Start()
    {
        NavMeshVisualizationSettings.showNavigation = 2;
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        aILerp = GetComponent<AILerp>();

    }

    void Update()
    {
        //aiPath.;
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("путь начат");
            ABPath path = aILerp.GetPath();


            int pass_num = 1;
            Stopwatch time = Stopwatch.StartNew();

            time = Stopwatch.StartNew();
            Debug.Log("Start");
            for (int i = 0; i < pass_num; i++)
            {
                foreach (var item in astarPath.ScanAsyncCustom())
                {
                    Debug.Log(item);
                }
            }
            time.Stop();
            Debug.Log("ScanAsyncCustom custom on " + time.ElapsedMilliseconds + " ms " + pass_num + " pass");


            //Task task = new Task(ReScan);
            //task.Start();


            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //AIBase.destination
            aILerp.destination = newPosition;
            /*for (int i = 0; i < path.path.Count; i++)
            {
                Int3 pos = path.path[i].position / 1000;
                Debug.Log(i + ") " + pos);
                //Instantiate(Cube, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
            }*/
            

            //seeker.StartPath(transform.position, target.position);
        }
    }

    void ReScan()
    {
        int pass_num = 1;
        Stopwatch time = Stopwatch.StartNew();

        /*for (int i = 0; i < pass_num; i++)
        {
            foreach (var item in astarPath.ScanAsync()) ;
        }
        Debug.Log("ScanAsync on " + time.ElapsedMilliseconds + " ms " + pass_num + " pass");
        time.Stop();*/
        time = Stopwatch.StartNew();
        Debug.Log("Start");
        for (int i = 0; i < pass_num; i++)
        {
            foreach (var item in astarPath.ScanAsyncCustom())
            {
                Debug.Log(item);
            }
        }
        time.Stop();
        Debug.Log("ScanAsyncCustom custom on " + time.ElapsedMilliseconds + " ms " + pass_num + " pass");
    }
}
