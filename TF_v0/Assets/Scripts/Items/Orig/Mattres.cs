using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mattres : MonoBehaviour
{
    Rigidbody rb;
    bool attract;
    Vector3 targetPosition;

    List<Vector3> positionsForce;
    List<float> massAdd;


    //Posible logica: Objeto que choque con objeto con gameobject y namme = "Colchon", se pega en la posición donde lo toque
//la velocidad del rigidbody del objeto pegado se vuelve 0 y su peso, pero el colchón recibe un impacto con addforce pero reducido a la mitad?  en esa misma zona y fuerza, pra
//simular el golpe, y el peso del rigidbody del colchón aumenta, ya uqe, el que simula el impacto es el colchón. Si se unen más objetos procesose repite. 

    //podríamos hacer que mientras lleves el colchón el daño por impacto se reduce y evita que mueras por caída/golpes quedandose la vida en 1,
    //pero el fuego y balas, ignoran esa protección. 

    //Colchon se pega a objeto con speedforce, porque así el colchón sigue al objeto y no interfiere con el aspecto de 
    //chocarse.si otros objetos se chocan con el colchon estos siguen al colchon pero con addforce  para darle más dinamismo al movimiento.
    void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    void Update()
    {       
        if (attract)
        {
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, 10000);
            rb.AddForce((transform.position - targetPosition) * 30 * Time.fixedDeltaTime);
            GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

    void Launch()
    {
       // rb.addForce();
    }

    public void AddMass(float x, Vector3 v3)
    {
        positionsForce.Add(v3);
        massAdd.Add(x);

        rb.mass = rb.mass + x;
    }
    
    /*
     * Si el tiempo es 1 se activa lo de pegarse, de esta forma permitimos que los objetos puedan ser movidos múltiples
     * veces en tiempo lennto sin tener que añadir código.
     * 
     * al entrar en contacto el objeto con el colchón, se hará un lerp con su rotación para que acabe cojiendo su rotación y evitar situaciones
     * de que un personaje vaya dando vueltas como si la cama fuese un sombrero. Hay que ver situaciones como con coche se respeta o no
     * */
}
