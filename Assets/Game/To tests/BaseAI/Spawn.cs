using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject SpawnObj;
    public Transform SpawnPos;
    public bool should_spawn;
    public int num_f_spawn;

    private void Start()
    {

        if (should_spawn)
        {
            //CorutineManager.StartCorutine(enumerator());

            for (int i = 0; i < num_f_spawn; i++)
            {

                Vector3 random = new Vector3(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));

                Instantiate(SpawnObj, SpawnPos.position + random, SpawnPos.rotation, transform);
            }

        }
    }


    void Update()
    {

    }

    IEnumerator enumerator()
    {
        Debug.Log("Start");
        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < 200; j++)
            {
                yield return null;
            }
            Instantiate(SpawnObj, SpawnPos.position, SpawnPos.rotation, transform);
        }
    }
}
