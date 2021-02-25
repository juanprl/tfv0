using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mattres1 : MonoBehaviour
{
    Rigidbody rb;
    bool attract;
    Vector3 targetPosition;

    //Posible logica: Objeto que choque con objeto con gameobject y namme = "Colchon", se pega en la posición donde lo toque
//la velocidad del rigidbody del objeto pegado se vuelve 0, pero el colchón recibe un impacto con addforce pero reducido a la mitad?  en esa misma zona y fuerza, pra
//simular el golpe. Si se unen más objetos procesose repite. 


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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SpeedForce>() )
        {
            if (other.GetComponent<SpeedForce>().itMoved)
            {
                attract = true;
                targetPosition = other.transform.position;

            }
        }
    }
}
