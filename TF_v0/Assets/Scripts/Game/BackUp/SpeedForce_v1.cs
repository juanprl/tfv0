using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpeedForce_v1 : MonoBehaviour
{
    Rigidbody rb;
    Vector3 oldPosition;// Tenemos que conservar la orientación,
    Vector3 newPosition;// La nueva realmente ya no importa.

    //¿Cómo aplicamos el boost? Y si tenemos en cuenta la orientación final? Tipo el objeto tiene la fuerza hacia la orientación del objeto final. Cara arriba/cara/atrás

    float mass;
    public float explosionForce = 12f; //Hacerlo que lo saque de una variable global para poder actualizar todos los objetos simultaneamente.
    public float explosionRadius = 12f; //Hacerlo que lo saque de una variable global para poder actualizar todos los objetos simultaneamente.


    //ME recomiendan Lerp... ni idea de como aplicarlo
    //No sigue las físicas convencionales, todos los objetos al ser movidos tienen un boost de velocidad y fuerza
    //Excusa lore: La energía supervelocista se engancha al objeto, y por eso salen disparado.
    //¿cuánto mayor sea su fuerza, más lento salen o más rápido?
    //fijarse en Zelda breath of the wild para ver los objetos que salen volando al ser golpeados.

    public bool a;
    public bool itMoved = false;
    bool moveReference;
    Transform transReferenceTemp;

    //
    GameObject exploxionDirection;

    public GameObject paredFrontal;
    public GameObject paredTrasera;
    public GameObject paredDerecha;
    public GameObject paredIzquierda;
    public GameObject paredArriba;
    public GameObject paredBajo;

    void Start()
    {
        //oldPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;

        //Cuando estan activos, tiene una flecha o indicativo para indicarte la dirección de donde va a salir la fuerza.
        paredFrontal.SetActive(false);
        paredArriba.SetActive(false);
        paredBajo.SetActive(false); 
        paredTrasera.SetActive(false);
        paredIzquierda.SetActive(false); 
        paredDerecha.SetActive(false); 



        //Un objeto puede estar formado por multiples piezas, por ejemplo, una mesa, es la plataforma y cuatro patas,
        //en el juego habrá objetos que se podrán dividir en otros más queños, como la mesa o un coche y sus puertas (pueden ser arrancadas)
                                                //Meter en futuro
        //Mientras que el objeto sea un conjunto, los mini- objetos deben actuar como uno solo, es decir, su speedforce esta desactivada, para eso debemos comprobar:
        //Pos: si seleccionas "arrancar", se activa su speedforce y posibilidad de seleccionarlo.
        
        
    }

    void Update()
    {
        //print(Time.time);

        //que la fuerza con la que salen lanzados sea proporcional al peso porque sino un 50kg se va 5m pero si pesa 1kg se 1000m,

        //si interactuasa con el objeto cambia a true/false y tienen un boost.

        //¿si se queda en el mismo sitio, lo consideremos qué no se a movido, aunque se haya tocado? Sí.
        
        //Por ahora esta planteado solo para usarse una vez.
        if (!a)
        {
            oldPosition = transform.position;
        }

        //La orientación de la fuerza dependerá de la colocación final del usuario. Teclas V-B, y Flechas -> <-
        if (Input.GetKeyDown(KeyCode.Q) && itMoved == true &&  Time.timeScale != 1 /*Si no se pone es realmente gracioso, porque cada vez que pulsas q vuelven a recibir el impacto*/) //Esto debería ir en FixedUpdate (poner este if en ua función aparte, y que se llame en FixedUpdate)
        {
            if (rb != null)
            {
                rb.AddExplosionForce(rb.GetComponent<Rigidbody>().mass * explosionForce, exploxionDirection.transform.position, explosionRadius, 1f, ForceMode.Impulse);
                print(name + " ha usado SpeedForce");

                ChooseForceExploxionDirection(99);
            }
        }
    }

    /*Proceso:
         * Coges objeto.
         * Cuando pulsas Q el juego se reanuda.
         * Go Brooom xD 
    */

    /* 10fuerza x mass o 8.55 * mass
     * 20 fuerza - mass = 1 mola
     * 60 - 5kg // 50 - 5kg
     */

    public void MoveObject()
    {
        a = true;
        itMoved = true;

        //Temp

        

        //Cuando un objeto puede ser Movido que cambie su color o añadir algun efecto.
        //Por ahora Flecha, pero podríamos meter rayos o algo. 
    }

    public void ChooseForceExploxionDirection(int x)
    {        
        //Mejora, en vez de tener que añadir un "cubo"/6 paredes para cada objeto, podríamos hacer que esas direcciones sean calculadas
        //Tipo: paredFrontal = transform(x,y,z); etc
        //La flechas serían un único o 6 objetos situados fuera de cámara y luego serían actualizados sus posición.


        if (x == 0)
        {
            exploxionDirection = paredFrontal;

            paredFrontal.SetActive(true);
            paredArriba.SetActive(false);
            paredBajo.SetActive(false); 
            paredTrasera.SetActive(false);
            paredIzquierda.SetActive(false);
            paredDerecha.SetActive(false); 
        }
        if (x == 1)
        {
            exploxionDirection = paredTrasera;

            paredFrontal.SetActive(false);
            paredArriba.SetActive(false);
            paredBajo.SetActive(false);
            paredTrasera.SetActive(true);
            paredIzquierda.SetActive(false);
            paredDerecha.SetActive(false);
        }
        if (x == 2)
        {
            exploxionDirection = paredBajo;

            paredFrontal.SetActive(false);
            paredArriba.SetActive(false);
            paredBajo.SetActive(true);
            paredTrasera.SetActive(false);
            paredIzquierda.SetActive(false);
            paredDerecha.SetActive(false);
        }
        if (x == 3)
        {
            exploxionDirection = paredArriba;

            paredFrontal.SetActive(false);
            paredArriba.SetActive(true);
            paredBajo.SetActive(false);
            paredTrasera.SetActive(false);
            paredIzquierda.SetActive(false);
            paredDerecha.SetActive(false);
        }
        if (x == 4)
        {
            exploxionDirection = paredIzquierda;

            paredFrontal.SetActive(false);
            paredArriba.SetActive(false);
            paredBajo.SetActive(false);
            paredTrasera.SetActive(false);
            paredIzquierda.SetActive(true);
            paredDerecha.SetActive(false);
        }
        if (x == 5)
        {
            exploxionDirection = paredDerecha;

            paredFrontal.SetActive(false);
            paredArriba.SetActive(false);
            paredBajo.SetActive(false);
            paredTrasera.SetActive(false);
            paredIzquierda.SetActive(false);
            paredDerecha.SetActive(true);
        }

        if (x == 99)
        {
            paredFrontal.SetActive(false);
            paredArriba.SetActive(false);
            paredBajo.SetActive(false); 
            paredTrasera.SetActive(false);
            paredIzquierda.SetActive(false);
            paredDerecha.SetActive(false); 
        }
    }
}
