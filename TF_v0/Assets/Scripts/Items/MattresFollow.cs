using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MattresFollow : MonoBehaviour
{
    public bool a;
    public float speedRotation = 1;
    Transform target;
    SpeedForce speedForce;

    void Start()
    {
        speedForce = GetComponent<SpeedForce>();
    }

    void Update()
    {
        if (a)
        {
         
           // transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, Time.deltaTime * 70);

          //  transform.position = Vector3.Lerp(transform.position, target.position, 100);
        }
    }

    private void OnTriggerEnter(Collider other)
    {      
        if (Time.timeScale == 1 && !a)
        {
            if (other.gameObject.name == "Colchon"  && speedForce.itMoved) 
            {
                target = other.GetComponent<Transform>();

                a = true;

                //Reducir la fuerza actual a la mitad? 
                //
                GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x/2, GetComponent<Rigidbody>().velocity.y / 2, GetComponent<Rigidbody>().velocity.z / 2); 

                //Rev
               // transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, Time.deltaTime * speedRotation);

            }
        }
    }
}
