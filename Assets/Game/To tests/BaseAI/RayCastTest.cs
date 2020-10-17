using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;
using System.Threading.Tasks;
using System;

public class RayCastTest : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Ray2D ray = Physics2D.Raycast(new Vector2(), new Vector2()).point

            int num_of_pass = 100000;
            Vector2 point;
            Vector2 vector2 = Vector2.zero;


            Stopwatch time = Stopwatch.StartNew();
            for (int i = 0; i < num_of_pass; i++)
                Physics2D.Raycast(vector2, vector2);

            Debug.Log("simple on " + time.ElapsedMilliseconds);

        }
    }
}
