using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpeedForce : MonoBehaviour
{
    /* Para un juego con una supervelocidad más realista sería realentizar todo el juegob y que se i nteractue como en este juego, pero
     * aplicando los calculos de acelaración y fuerza originalmente planteados. Un problema que puede ocurrir es si realentizamos el juego entero
     * pueda ser que los script con update no se ejecuten bien, ya que, no ocurrirán las suficientes veces. Pos solucion: el juego esta en tiempo normal, pero tanto posiciones,
     * animaciones, particulas y otros, están frenados via script)
     */
    
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

    //Temp
    Transform pared;
    Vector3 paredV3;

    //
    public BoxCollider boxCollider;
    Vector3 sizeBoxCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;

        //Un objeto puede estar formado por multiples piezas, por ejemplo, una mesa, es la plataforma y cuatro patas,
        //en el juego habrá objetos que se podrán dividir en otros más queños, como la mesa o un coche y sus puertas (pueden ser arrancadas)
        //Meter en futuro
        //Mientras que el objeto sea un conjunto, los mini- objetos deben actuar como uno solo, es decir, su speedforce esta desactivada, para eso debemos comprobar:
        //Pos: si seleccionas "arrancar", se activa su speedforce y posibilidad de seleccionarlo.

        //El collider del que sacamos las medidas, debería estar en trigger para que no interfiera. //Lo hacemos en el inicio por si se nos olvida.       
        boxCollider.isTrigger = true;

    }

    void Update()
    {               
       // print(Time.time);

        //que la fuerza con la que salen lanzados sea proporcional al peso porque sino un 50kg se va 5m pero si pesa 1kg se 1000m,

        //si interactuasa con el objeto cambia a true/false y tienen un boost.
        //¿si se queda en el mismo sitio, lo consideremos qué no se a movido, aunque se haya tocado? Sí.
        
        if (!a)//en deshuso
        {
            oldPosition = transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Q) && itMoved == true &&  Time.timeScale != 1 /*Si no se pone es realmente gracioso, porque cada vez que pulsas q vuelven a recibir el impacto*/) //Esto debería ir en FixedUpdate (poner este if en ua función aparte, y que se llame en FixedUpdate)
        {
           if (rb != null)
           {
                rb.AddExplosionForce(rb.GetComponent<Rigidbody>().mass * explosionForce, /*transform.Position*/ transform.localPosition + paredV3, explosionRadius, 1f, ForceMode.Impulse);

                //ChooseForceExploxionDirection(99);

                print(name + " ha usado SpeedForce");
           }
        }

     
        //transform.Translate(Vector3.forward * Time.deltaTime);
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
        //
        if (!a)
        {
            Contador.contSpeedForce += 1;
        }
        Contador.nameObj = name;


        a = true;
        itMoved = true;

        //Cuando un objeto puede ser Movido que cambie su color o añadir algun efecto.
        //Por ahora Flecha, pero podríamos meter rayos o algo. 

    }

    public void ChooseForceExploxionDirection(int x)  //Teclas V-B, y Flechas -> <-. La orientación de la fuerza dependerá de la colocación final del usuario.  

    {
        //Mejora, en vez de tener que añadir un "cubo"/6 paredes para cada objeto, podríamos hacer que esas direcciones sean calculadas
        //Tipo: paredFrontal = transform(x,y,z); etc //No tiene que haber paredes y que sean calculadas 
        //La flechas serían un único o 6 objetos situados (prefab) fuera de cámara y luego serían actualizados sus posición local, no global.
        //Problemas: 1 No seguirían al objeto -Pos Solucion: Meter un update de actualizar posición
        //2 los objetos no miden lo mismo,un coche es mucho más alargado que ancho. ¿usar su posición + scala x/y/z? 

        //Para que esto funcione en todos, la porientación local de todos los objetos debe ser la misma, y se debe trabajar con local treansform 

        //Pos v2: hay un cubo en un gameObject al igual que la flecha, al activarlo se situa en el centro del objeto cpn Speedf y tiene su otientación, al usar u/v selecciona la localización de sus caras, 
        //la cosa es que tiene que adaptarse a su escala, hay que averiguar el tamaño real de los mesh

        //No está terminado, necesita que la distancia entre la explosion y eel objeto sea proporcional a su tamaño o algo.

        //En caso de no poder calcular su tamaño adecuadamente, será un valor que meteremos manualmente, es un rollo, pero menos que meter 6 paredes?
        //Mejora: Meter en un subgrupo un boxcoliider con el tamaño adecuado del mesh, y sacarle las medidas.

        //Calc 3
        /*Cada mesh tiene un collider que nos da su medida real,
         * situandonos en la posición local del mesh, le sumaremos/restaremos sus medidas -x/x.-y/y, z/-z
         * para obtener así la ubicación de la explosión basada en su tamaño*/
        //Una cosa a tener en cuenta es que si los GameObject han sido reescalados, no darán una medida real.
        //Con este método creo que no será necesario usar el reescaldo para la explosion pero si para el efecto.

        //Ajustar el efecto de la posición según el tamaño del mismo.


        if (x == 0)
        {
                // exploxionDirection = paredFrontal;
            //paredV3 = new Vector3(-1,0,0); //Algo le pasa a esta direccion 
            paredV3 = new Vector3(-boxCollider.size.x, 0, 0);
            print(" paredFrontal ");
            Contador.objDirecction = "_Fr_";
        }
        if (x == 1)
        {
                // exploxionDirection = paredTrasera;
                paredV3 = new Vector3 (boxCollider.size.x, 0, 0);
                print(" paredTrasera ");
            Contador.objDirecction = "_Tr_";
        }
        if (x == 2)
        {
                // exploxionDirection = paredBajo; //Por algún motivo, aparece muy arriba y muy abajo.
                paredV3 = new Vector3(0, -boxCollider.size.y/2, 0);
                print(" paredBajo ");
            Contador.objDirecction = "_Ab_";
        }
        if (x == 3)
        {
                // exploxionDirection = paredArriba;
                paredV3 =  new Vector3(0, boxCollider.size.y/2, 0);
                print(" paredArriba");
            Contador.objDirecction = "_Arr_";
        }
        if (x == 4)
        {
                //exploxionDirection = paredIzquierda;
                paredV3 = new Vector3(0, 0, -boxCollider.size.z);
                print(" paredIzquierda");
            Contador.objDirecction = "_Izq_";
        }
        if (x == 5)
        {
                //exploxionDirection = paredDerecha;
                paredV3 = new Vector3(0, 0, boxCollider.size.z);
                print(" paredDerecha");
            Contador.objDirecction = "_Der_";
        }

            /*
            if (x == 99)
            {
                    paredFrontal.SetActive(false);
                    paredArriba.SetActive(false);
                    paredBajo.SetActive(false);
                    paredTrasera.SetActive(false);
                    paredIzquierda.SetActive(false);
                    paredDerecha.SetActive(false);
            }
            */

        //
        Contador.flechaPosition = transform.position + paredV3 * 1;
        Contador.flechaRotation = transform.rotation; //no terminado. iz/der no aputan a su sitio
        Contador.flechaScale = new Vector3(boxCollider.transform.localScale.x, boxCollider.transform.localScale.y, boxCollider.transform.localScale.z);
    }
}
