using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTemp : MonoBehaviour
{
    //Alpha //Temp
    public Transform firePoint;
    public GameObject bullet;
    bool a;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            if (!a)
            {
                Shoot();
                a = true;
            }
        }
    }

    void Shoot()
    {
        Instantiate(bullet, firePoint.transform.position, firePoint.transform.rotation);
    }
}
