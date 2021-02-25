using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Health m_health;
    SpeedForce m_speedForce;

    public float explosionRadius = 1;
    public float explosionForce = 1;
    public float damage = 1;
    public bool a, b;

    void Start()
    {
        m_health = GetComponent<Health>();
        m_speedForce = GetComponent<SpeedForce>();
    }

    void Update()
    {
        if (m_health.lifePoints <= 50 || m_speedForce.itMoved  && !a) 
        {
            a = true;        
        }
        if (a && Time.timeScale == 1 && !b)
        {
            Explode();
            b = true;
        }
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            //el objeto esta afectado por los colliders y hay que excluirlo con el if o se verá afectado por su propia explosión
            if (collider.GetComponent<Health>() && collider.name != "BarrilExplosivoV2") //Daño
            {
                collider.GetComponent<Health>().TakeDamage(damage);
            }        
        }

        Collider[] colliders2 = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders2)
        {
            
            if (collider.GetComponent<Rigidbody>() && collider.name != "BarrilExplosivoV2") //Impulso
            {
                collider.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            }
        }

        /*GameObject explosion = (GameObject)Instantiate(explosionParticle, transform.position, transform.rotation);
        Destroy(explosion, 3f);*/

        //Destroy(this.gameObject, 3f);
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
