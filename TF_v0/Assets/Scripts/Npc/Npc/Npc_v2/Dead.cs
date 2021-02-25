using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : MonoBehaviour
{
    Animator anim;

    Health m_health;
    Actor m_actor; 

    void Start()
    {
        m_health = GetComponent<Health>();
        m_actor = GetComponent<Actor>();
    }

    void Update()
    {
        if(m_health.GetLifePoints() <= 0) 
        {
            m_actor.t_states = Actor.States.Dead;
            //anim. SetAnimación muerte. 
        }

        UltraDead();
    }

    public void Resurrection()
    {
        m_health.SetLifePoints(GetComponent<Health>().maxLifePoints);
        //anim. SetAnimación Resurrection.
        m_actor.t_states = Actor.States.Resting;
    }

    void UltraDead()
    {
        if (m_health.GetLifePoints() <= -(GetComponent<Health>().maxLifePoints))
        {
            print("Stop, he is already dead!");
            //Logro "Ensañador"
            //anim. SetAnimación Ultra muerte /Desmembramiento. 
        }
    }
}
