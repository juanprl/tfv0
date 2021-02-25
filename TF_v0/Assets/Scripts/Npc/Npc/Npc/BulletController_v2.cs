using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletController_v2 : MonoBehaviour
{
    [Tooltip("0 bala normal, 1 bala que sigue, 2 Bala Parabola, 3 ")]
    public int bulletType = 0;
    int cont = 0;
    //
    private Transform target;
    public float speed = 30f;
    public float lifeTimeBullet = 3f;
    public float damageBullet = 15f;
                [Header("Daño en Área")]
    [Tooltip("ExplosionRadius tiene que ser mayor que 0 para activarse")]
    public float explosionRadius = 0f;
    public float explosionForce = 5f;
    public GameObject explosionParticle;
    bool danhoArea = true;

    public GameObject impactParticle;

    //
    Vector3 shootDir;
    Quaternion shootRot;
    //
    Rigidbody r;
    //
                [Header("Activar/Desactivar Fuego Amigo, NO tocar")]
    public bool playerDamage = true;
    public bool allyDamage = true;
    public bool enemyDamage = true;
    public bool objectDamage = true;

    void Start()
    {
        //0
        r = GetComponent<Rigidbody>();
        //1
        if (target != null)
        {
            bulletType = 1;
        }      
    }


    void Update()
    {
        //
        switch (bulletType)
        {
            case 0:
                Shoot();
                break;
            case 1:
                ShootFollow();
                break;
            case 3:

                break;
        }


        Destroy(gameObject, lifeTimeBullet);
    }


    //Disparar 
    void Shoot()
    {

        r.AddForce(this.transform.forward * speed);


        //this.transform.rotation = shootRot;
        //transform.position += shootDir * speed * Time.deltaTime;

        //transform.position += transform.position * speed * Time.deltaTime;


        //******transform.Translate(shootDir.normalized * speed * Time.deltaTime, Space.Self);
        //o
        //que lo anterior lo haga en el código anterior y este solo le pase el resultado a ver que pasa
    }





    //Sigue al objetivo, hacer uno aparte que solo coja la dirección.
    //Poner tiempo máximo de seguimiento, luego desaparece/muere

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void ShootFollow()
    {
        if (target == null) //Si no hay objetivo, la bala se destruye
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if(dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {
        GameObject bulletGO = Instantiate(impactParticle, transform.position, transform.rotation);
        Destroy(bulletGO, 2f);
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.gameObject.name == "target_eliminar")
        {
            BulletController xxx = other.GetComponent<BulletController>();
            Health t_health = GetComponent<Health>();
            t_health.TakeDamage(xxx.damageBullet);
            Debug.Log("Parabola dada");
        }
        if (other.gameObject.name == "Floor")
        {
            Destroy(gameObject, 3f);
        }*/

        GameObject xxx = (GameObject)Instantiate(impactParticle, other.transform.position, other.transform.rotation);
        Destroy(xxx, 1f);

        /* if (other.gameObject.tag != "")
         {
             Destroy(gameObject, 3f);
         }*/

        //Posibilidad para reducir código, guardar other en otro GAmeObject y pasarselo a un método diferente.
        /*if (other.gameObject.tag == "Player" && playerDamage == true)
        {
            Health m_health = other.GetComponent<Health>();
            m_health.TakeDamage(damageBullet);

            damageBullet = 0f;//Esto es porque si no el daño se repetía varias veces

            Destroy(xxx, 1f);
            Destroy(gameObject, 3f);
        }    */
        if (other.gameObject.tag == "Player" && playerDamage == true) { TargetHitted(other.gameObject); }              
        if (other.gameObject.tag == "Enemy" && enemyDamage == true) { TargetHitted(other.gameObject); }     
        if (other.gameObject.tag == "Ally" && allyDamage == true) { TargetHitted(other.gameObject); }
        if (other.gameObject.tag == "ObjectDestructive" && objectDamage == true) { TargetHitted(other.gameObject); }

        //
        if (explosionRadius > 0 && danhoArea)
            {
                Explode();
                danhoArea = false;//Debido a que cada vez que entra en contacto con un Collider se activaría la función Explode, he puesto una segunda condición.
            }
    }

    void Explode()
    {    
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            //Daño
            if (collider.tag == "Player" && playerDamage == true) { ShootExplosion(collider.gameObject); }                              
            if (collider.tag == "Enemy" && enemyDamage == true) { ShootExplosion(collider.gameObject); }          
            if (collider.tag == "Ally" && allyDamage == true) { ShootExplosion(collider.gameObject); }          
            if (collider.tag == "ObjectDestructive" && objectDamage == true) { ShootExplosion(collider.gameObject); }
        }
        
        Collider[] colliders2 = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders2)
        {
            //Onda Expansiva
            if (collider.tag == "Player" && playerDamage == true) { ForceExplosion(collider.gameObject); }         
            if (collider.tag == "Enemy" && enemyDamage == true) { ForceExplosion(collider.gameObject); }       
            if (collider.tag == "Ally" && allyDamage == true) { ForceExplosion(collider.gameObject); }         
            if (collider.tag == "ObjectDestructive" && objectDamage == true) { ForceExplosion(collider.gameObject); }
        }
        GameObject explosion = (GameObject) Instantiate(explosionParticle, transform.position, transform.rotation);
        Destroy(explosion, 3f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
    
    //

    void TargetHitted(GameObject tempGameObject)
    {

            Health m_health = tempGameObject.GetComponent<Health>();
            m_health.TakeDamage(damageBullet);

            damageBullet = 0f;//Esto es porque si no el daño se repetía varias veces

            Destroy(gameObject, 3f);
    }

    void ShootExplosion(GameObject tempGameObject2)
    {
        Health m_health = tempGameObject2.GetComponent<Health>();
        m_health.TakeDamage(damageBullet);
        Debug.Log("Golpe en área");
    }
    void ForceExplosion(GameObject tempGameObject3)
    {
        //Añadir ondas expansivas
        Rigidbody rg = tempGameObject3.GetComponent<Rigidbody>();
        if (rg != null)
        {
            rg.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            Debug.Log(tempGameObject3.name + "ha recibido daño en área");//3
        }
    }
}
