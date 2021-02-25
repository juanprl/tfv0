using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //https://docs.unity3d.com/ScriptReference/Rigidbody.AddExplosionForce.html

    public float moveZ = 30f;

    Rigidbody rb;

    [Header("Daño en Área")]
    [Tooltip("ExplosionRadius tiene que ser mayor que 0 para activarse")]
    public float explosionRadius = 10f;
    public float explosionForce = 59f;
    public GameObject explosionParticle;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter()
    {
        Collider[] colliders2 = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders2)
        {
            //Onda Expansiva
            ForceExplosion(collider.gameObject); 
        }
        GameObject explosion = (GameObject)Instantiate(explosionParticle, transform.position, transform.rotation);
        Destroy(explosion, 3f);
    }

    //que la fuerza con la que salen lanzados sea proporcional al peso porque sino un 50kg se va 5m pero si pesa 1kg se 1000m,

    void ForceExplosion(GameObject tempGameObject3)
    {
        //Añadir ondas expansivas
        Rigidbody rg = tempGameObject3.GetComponent<Rigidbody>();
        if (rg != null)
        {
            rg.AddExplosionForce(rg.GetComponent<Rigidbody>().mass*explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            Debug.Log(tempGameObject3.name + " ha recibido daño en área");//3
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
