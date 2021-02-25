using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaAlante : MonoBehaviour
{
    public float speed;

    public float x,y,z;

    Rigidbody rg;

    bool a;

    public bool rodar;

    void Start()
    {
        rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(new Vector3(x,y,z) * Time.deltaTime *speed);

       
        if (!a)//Según el caso que no sea inmediato, que a veces tarde 1 o 2s o algo así.
        {
            if (rodar)
            {
                //Usa Tr.Global //Usarlo con el coche de lado, gracioso?
                rg.velocity = new Vector3(x * speed, rg.velocity.y, z * speed);
            }
            else
            {

                //Usa Tr.Local, pero no tiene inercia del rigidbody eso provoca que cuando dejamos de usar esa función se queda quieto en el sitio
                //por lo que no queda bien al chocar, buscar SOLUCIÓN
                //pos: tener un boost cuando se desactive y simular la fuerza acorrespondiente que tendría en el rigidbody.
                //pos2: usar el rigidbody en un Empty que contenga el objeto y oriemtar este segúnj las neecesidades.
                transform.Translate(Vector3.left * Time.deltaTime * speed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (/*!other.CompareTag("Floor")*/other.gameObject.layer != LayerMask.NameToLayer("Ground"))
        {
            a = true;
            //print("a<<<<<<<<<<<");
        } 
    }
}
