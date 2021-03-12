using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage_v2_Temp : MonoBehaviour
{
    Rigidbody rb;
    float fallForce; 
    public float baseFallDamage; //Damage
    public float maxFallForce; //much force the player can take before getting injured

    Health m_health;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Chocar contra suelo
       /* if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if(fallForce > maxFallForce)
            {
                float damage = Mathf.RoundToInt(fallForce * baseFallDamage);
                fallForce = 0;

                m_health.TakeDamage(damage);
            }
        }
        else
        {
            float vY = Mathf.Abs(rb.velocity.y);
            fallForce = vY;
        }*/


        if(rb.velocity.y == 0)
        {
            //Si npc/player rg.velocity.y == 0, esta quieto y hace esto

            /**///Que algo se choque contra ti //Tiene que tener Rigidbody
            if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                float vY = Mathf.Abs(other.GetComponent<Rigidbody>().velocity.y);
                fallForce = vY;

                if (fallForce > maxFallForce)
                {
                    float damage = Mathf.RoundToInt(fallForce * baseFallDamage);
                    fallForce = 0;

                    m_health.TakeDamage(damage);
                }
            }

            // pos hacer una lista de lo que le ha hecho daño y que no pueda hacerle daño por golpe de nuevo en Xs para evitar golpes repetidos.
        }
        else
        {
            //Chocarse contra algo incluido suelo
            if (!other.CompareTag("Respawn"))
            {
                float vY = Mathf.Abs(rb.velocity.y);
                fallForce = vY;

                if (fallForce > maxFallForce)
                {
                    float damage = Mathf.RoundToInt(fallForce * baseFallDamage);
                    fallForce = 0;

                    m_health.TakeDamage(damage);
                }
            }
            //Mejora: si estas en movimiento calcula la diferencia de velocidades entre el npc/player y el objeto impactado para calcular el daño a recibir. 
        }
    }
}
