using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCharacter : MonoBehaviour
{
    Animator anim;
    Rigidbody rgbody;

    //NpcsController m_npcsController;
    Actor m_actor;

    void Start()
    {
        anim = GetComponent<Animator>();
        rgbody = GetComponent<Rigidbody>();

        // m_npcsController = GetComponent<NpcsController>();
        m_actor = GetComponent<Actor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_actor.t_states != Actor.States.Dead)
        {
            if (rgbody.velocity.magnitude > 0 /* && grounded == true*/)
            {
                //anim correr
            }
            if (rgbody.velocity.magnitude == 0 /* && grounded == true*/)
            {
                //anim idle
            }

            if (m_actor.t_states == Actor.States.Resting)
            {
              /*  if (rgbody.velocity.magnitude == 0 )
                {
                    anim.SetBool("Idle", true);
                }
                else
                {
                    anim.SetBool("Idle", false);
                }*/

                if (rgbody.velocity.magnitude > 0 /* && grounded == true*/)
                {
                    anim.SetBool("Running", true);
                }else
                {
                    anim.SetBool("Running", false);
                }
            }
            else
            {
                anim.SetBool("Running", false);
                anim.SetBool("Idle", false);
            }

            if (m_actor.t_states == Actor.States.Attacking)
            {
                anim.SetBool("Attacking", true);
            }
            else
            {
                anim.SetBool("Attacking", false);
            }
            /*if (grounded == false)
            {
                //anim jump
            }*/

        }
    }
}
