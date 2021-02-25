using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpeedForce1 : MonoBehaviour
{
    //BackUp
    
    Rigidbody rb;
    Vector3 oldPosition;// Tenemos que conservar la orientación,
    Vector3 newPosition;// La nueva realmente ya no importa.

    //¿Cómo aplicamos el boost? Y si tenemos en cuenta la orientación final? Tipo el objeto tiene la fuerza hacia la orientación del objeto final. Cara arriba/cara/atrás

    public float time = 0.1f;//
    float acelerationX, acelerationY, acelerationZ;
    float fuerzaX, fuerzaY, fuerzaZ;
    float distanceX, distanceY, distanceZ;
    float mass;

    // float velAngX, velAngY, velAngZ;
    // float velX, velY, velZ;

    //ME recomiendan Lerp... ni idea de como aplicarlo

    //No sigue las físicas convencionales, todos los objetos al ser movidos tienen un boost de velocidad y fuerza
    //Excusa lore: La energía supervelocista se engancha al objeto, y por eso salen disparado.
    //¿cuánto mayor sea su fuerza, más lento salen o más rápido?

    //fijarse en Zelda breath of the wild para ver los objetos que salen volando al ser golpeados.
    public bool a;
    public bool itMoved = false;

    //
    public Text textTemp;

    void Start()
    {
        //oldPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
      
        //print(oldPosition +"SSSSSSSSS");
    }

    void Update()
    {
        //print(Time.time);

        //que la fuerza con la que salen lanzados sea proporcional al peso porque sino un 50kg se va 5m pero si pesa 1kg se 1000m,



        //Los objetos aunque no se interactuen tiene un boost, solución: bool a, si interactuasa con el objeto cambia a true/false.

        //Mientras !a irá cogiendo su posicón, una vez se interactue con ellos, el bool será true y se tendrá en cuenta que la última
        //posición que tuvo antes de comenzar es la inicial
        //Por ahora esta planteado solo para usarse una vez.
        if (!a)
        {
            oldPosition = transform.position;
        }

        //Suponemos que el botón para coger objetos es este, y al pulsarlo lo coges.
        if (Input.GetKeyDown(KeyCode.E))
        {
            a = true;
            itMoved = true;

            //
            textTemp.enabled = false;
        }

        if (/*Time.timeScale == 1*/ Input.GetKeyDown(KeyCode.Q) && itMoved == true) //Esto debería ir en FixedUpdate 
        {
            newPosition = transform.position;

            // print(newPosition.transform.position);           
            //print(oldPosition.transform.position + "  ·" + newPosition.transform.position);

            distanceX = oldPosition.x - newPosition.x;
            distanceY = oldPosition.y - newPosition.y;
            distanceZ = oldPosition.z - newPosition.z;

            //distancia = oldPosition.position.x - newPosition.position.x;

            acelerationX = distanceX / time;
            acelerationY = distanceY / time;
            acelerationZ = distanceZ / time;

            fuerzaX = mass * acelerationX;
            fuerzaY = mass * acelerationY;
            fuerzaZ = mass * acelerationZ;

            //

           // rb.AddForce(fuerzaX, fuerzaY, fuerzaZ);

            rb.velocity += new Vector3(acelerationX, acelerationY, acelerationZ);

            //print(Vector3.Distance(oldPosition.localPosition, newPosition.localPosition));
            //print(distancia);
        }
        /*Proceso:
         * Coges objeto con E.
         * Cuando pulsas Q el juego se reanuda.
         * Go Brooom xD */
    }

    /*
     * 10fuerza x mass o 8.55 * mass
     * 20 fuerza - mass = 1 mola
     * 60 - 5kg // 50 - 5kg
     * 
     */
}
