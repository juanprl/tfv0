using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCharacter2D : MonoBehaviour
{
    Animator anim;
    Rigidbody rgbody;

    //NpcsController m_npcsController;
    public Actor m_actor;

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
        
          

          
                if (rgbody.velocity.magnitude > 0 /* && grounded == true*/)
                {
                    anim.SetBool("Running", true); anim.SetBool("Attacking", true);
        }
                else
                {
                    anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
        }
            }
          

                }
    

