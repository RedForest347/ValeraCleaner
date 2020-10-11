using RangerV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject DDD;
    public Transform SpawnPos;

    private void Start()
    {
        //CorutineManager.StartCorutine(enumerator());

        for (int i = 0; i < 50; i++)
        {
            Instantiate(DDD, SpawnPos.position, SpawnPos.rotation, transform);
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
            Instantiate(DDD, SpawnPos.position, SpawnPos.rotation, transform);
        }
    }
}
