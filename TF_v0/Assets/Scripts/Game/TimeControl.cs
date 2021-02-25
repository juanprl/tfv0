using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    public float time;

    void Start()
    {
        Time.timeScale = time;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 0;
        }

        //Meter pausa del juego aquí, tipo tiempo 0 o no
    }

}
