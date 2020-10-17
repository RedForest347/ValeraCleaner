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
    Rigidbody2D rb;
    AIPath aiPath;
    Seeker seeker;
    public AILerp aILerp;
    //NavMeshAgent agent;


    void Start()
    {
        aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        aILerp = GetComponent<AILerp>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //aiPath.;
        //Vector3 newPosition = target.transform.position;
        //aILerp.destination = newPosition;

        //AddForceUp();
        //StandartMoveToTarget();
        //AddForce();
        CustomMoveToTarget();

        if (Input.GetKeyDown(KeyCode.P))
        {
            Test();
        }
    }

    void StandartMoveToTarget()
    {
        Vector3 newPosition = target.transform.position;
        aILerp.destination = newPosition;
    }


    void AddForce()
    {
        rb.AddForce(new Vector2(0, 1));
    }



    int current_index = 0;
    int path_length;
    [SerializeField]
    float speed = 1;
    [SerializeField]
    float max_speed = 0.5f;
    float nearby_distance = 0.2f;

    void CustomMoveToTarget()
    {
        Vector3 newPosition = target.transform.position;
        aILerp.SearchNewPathCustom(newPosition);
        current_index = 0;


        if (aILerp.GetPathCustom() == null)
            return;

        List<GraphNode> path = aILerp.GetPathCustom().path;
        path_length = path.Count;


        if (path_length == 0 || (path_length == 1 && Nearby(transform.position, (Vector3)path[current_index].position, nearby_distance)))
        {
            Debug.Log("дошел");
            return;
        }

        if (path_length > 1)    current_index = 1;
        else                    current_index = 0;

        Vector3 target_pos = (Vector3)path[current_index].position;

        rb.AddForce((target_pos - transform.position).normalized * speed);

        if (rb.velocity.magnitude > max_speed) 
            rb.velocity = rb.velocity.normalized * max_speed;

        Debug.DrawLine(transform.position, target_pos, Color.red);

    }

    bool Nearby(Vector2 vector1, Vector2 vector2, float distance)
    {
        return (vector1 - vector2).magnitude < distance;
    }

    void Test()
    {
        Vector3 newPosition = target.transform.position;
        Debug.Log("путь начат");
        //ABPath path = aILerp.GetPath();


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


        newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //AIBase.destination
        aILerp.destination = newPosition;

        List<GraphNode> path = aILerp.GetPathCustom().path;

        for (int i = 0; i < path.Count; i++)
        {
            Int3 pos = path[i].position / 1000;
            Debug.Log(i + ") " + pos);
            Instantiate(Cube, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
        }


        /*for (int i = 0; i < path.path.Count; i++)
        {
            Int3 pos = path.path[i].position / 1000;
            Debug.Log(i + ") " + pos);
            //Instantiate(Cube, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);
        }*/


        //seeker.StartPath(transform.position, target.position);

    }

    void AddForceUp()
    {
        rb.AddForce(Vector2.up);
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
