using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    Vector3 velocity;
    
    //Para que este código funcione, tienes que seleccionar en el código -> en Ground Mask una capa, por ejemplo Ground. Y poner que el suelo sea el mismo tipo de Layer(Unity)
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;

    public bool noSaltes = false;
    int cont = 0;

    public float x,z;

    public float speedInSlowTime = 0.78f;//Esto podría cogerse de una variable global para facilitar el código
    //Actualmente cae con gravedad y en tiempo parado va muy lento.  
    //Hacer que caiga por tiempo, pero que caiga lento tipo gravedad lunar, 

    //
    Animator anim;

    //
    bool moveSwitchReference = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
       // anim = GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
       
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            anim.SetInteger("condition", 0);
        }


        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        if (moveSwitchReference)
        {
            x = 0;
            z = 0;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        if (Time.timeScale == 1)
        {
            controller.Move(move * speed * Time.deltaTime);
        }
        else
        {
            controller.Move(move * speed * speedInSlowTime);
        }


        if (!isGrounded /*&& velocidadJugador > 0*/) //Es decir, está en el aire, pero se mueve (Es decir, esta cayendo), No puede moverse hacia delante. Pero si esta sobre un objeto sí
        {
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            noSaltes = false;//Doble salto

            anim.SetInteger("condition", 1);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    } 

    public void SetSwitchReference(bool x)
    {
        moveSwitchReference = x;
    }
}
