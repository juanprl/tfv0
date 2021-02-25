using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Tooltip("//0 bala normal, 1 bala que sigue, 2 Bala Parabola, 3 Zona de daño normal")]
    public int be = 0;
    int cont = 0;
    //
    private Transform target;
    public float speed = 30f;
    public float lifeTimeBullet = 3f;
    public float damageBullet = 15f;
    public float explosionRadius = 0f;
    public float explosionForce = 3f;
    bool activar = true;
    public GameObject impactParticle;
    public GameObject explosionParticle;

    public bool hurtPlayer = true;
    //
    Vector3 shootDir;
    Quaternion shootRot;
    //
    Rigidbody r;
    //
    public bool playerDamage = true;
    public bool weebDamage = true;
    public bool enemyDamage = true;
    public bool objectDamage = true;

    void Start()
    {
        //0
        r = GetComponent<Rigidbody>();
        //1
        if (target != null)
        {
            be = 1;
        }

    }


    void Update()
    {      
        if (be == 0)
        {
            Shoot();
        }

        if (be == 1)
        {
            ShootFollow();
        }

        if( be == 2)
        {
             
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

        if (be !=3)
        {
            if (other.gameObject.tag != "")
            {
                Destroy(gameObject, 3f);
            }

            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "Npc")
            {
                GameObject xxx = (GameObject)Instantiate(impactParticle, other.transform.position, other.transform.rotation);

                Health m_health = other.GetComponent<Health>();
                m_health.TakeDamage(damageBullet);

                damageBullet = 0f;//Esto es porque si no el daño se repetía varias veces

                Destroy(xxx, 1f);
                Destroy(gameObject, 3f);
            }
            //
            if (explosionRadius > 0 && activar)
            {
                Explode();
                activar = false;//Debido a que cada vez que entra en contacto con un Collider se activaría la función Explode, he puesto una segunda condición.
            }
        }

        if (be == 3)
        {
            //Corregir para que se adapte según quien lo usa, alguna función para encontrar al padre en la jerarquia, o hacerlo manual con tres opciones. O un script solo para objetos simples en vez de reciclar código.
            if (hurtPlayer)
            {
                if (other.gameObject.tag == "Player" || other.gameObject.tag == "Npc")
                {
                    Health m_health = other.GetComponent<Health>();
                    m_health.TakeDamage(damageBullet);
                    Debug.Log("Espada " + other.gameObject.tag); 
                }
            }
            else
            {
                if (other.gameObject.tag == "Enemy")
                {
                    Health m_health = other.GetComponent<Health>();
                    m_health.TakeDamage(damageBullet);
                }
            }
        }
    }

    void Explode()
    {    
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {        
            //Daño
            if (collider.tag == "Player" || collider.tag == "Enemy")//Enemy, Player, Npc, Objetos con vida //Habrá que revisarlo cuando decidamos si hay daño amigo o no.
            {
                Health m_health = collider.GetComponent<Health>();
                m_health.TakeDamage(damageBullet);
                Debug.Log("BBBBBBBBBBBBBBBB");//3
            }
        }
        
        Collider[] colliders2 = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders2)
        {
            //Onda Expansiva
            if (collider.tag == "Player" || collider.tag == "Enemy")
            {
                //Añadir ondas expansivas
                Rigidbody rg = collider.GetComponent<Rigidbody>();
                if (rg != null)
                {
                    rg.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
                    Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAA");//3
                }
            }
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

    void TargetHitted()
    {
      //  if(playerDamage)
    }
}
