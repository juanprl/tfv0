using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    //Seleccionar y mover objetos del escenario

    public Camera rayCam;
    [Tooltip("Alcance de seleccionar un objeto")]
    public float rangeInteract = 100f;
    
    //Esto es para evitar que se instancie muchas veces, debido al que lanzar un raycast el objeto colisionado detecta múltiple rayos, me instancia muchas veces el objeto.
    bool desactivar;
    public float coldDown = 0.3f;

    //header
    public GameObject interactObject;//player reference
    public float speedMove = 1;
    public float speedRotation = 1;
    bool moveReference;//Activa/Desactiva la función de mover la referencia.
    bool rotateReference = true;//Switch para Rotar/Mover referencia. 
    bool moveSwitchReference = false;//Si pulsas R el jugador se mueve, pero no el objeto, y viceversa. 
    //public float maxMoveReference = 1; //Creo que hay que poner un máximo de distancia a la que se puede mover el objeto. Para no ponerlo en el cielo y a la, a ver que pasa.
    PlayerMovement playerMovement; //Para hacer que no se mueva mientras se mueva la referencia.     
    SpeedForce speedForce;

    public GameObject particleTemp;

    public int chooseForceExploxionDirection = 1;

    public float speedInSlowTime = 0.78f;//Esto podría cogerse de una variable global para facilitar el código

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))//Cl.Izq
        {
            Interact();

            //desactivar = false;           
        }

        if (Input.GetMouseButton(1))//Cl.Der //Cancela mover objeto
        {            
            moveReference = false;
            //Temp
            interactObject.SetActive(false);

            speedForce = null;
        }

        if (Input.GetKeyDown(KeyCode.H)) //Quiero que sea R, pero tiene que estar siendo utilizado en otro lado porque da error.
        {
            if (moveSwitchReference) //Si pulsas R/H el jugador se mueve, pero no el objeto, y viceversa.
            {
                moveSwitchReference = false;
                interactObject.SetActive(false);
            }
            else
            {
                moveSwitchReference = true;
                interactObject.SetActive(true);
            }

            playerMovement.SetSwitchReference(moveSwitchReference);//Creo que se puede mejorar si haces que cuando se coja el objeto, se empiece que el objeto se mueve y no el jugador.
        }

        if (moveReference == true)
        {
            if (Time.timeScale != 1)//Esto es para hacer que cuando el tiempo se reanude se "suelte" el objeto.
            {
                MoveReferenceObject();

                //
                if (Input.GetKey("up") || Input.GetKeyDown(KeyCode.V))
                {
                    chooseForceExploxionDirection += 1;
                }
                if (Input.GetKey("down") || Input.GetKeyDown(KeyCode.B))
                {
                    chooseForceExploxionDirection -= 1;
                }

                if (chooseForceExploxionDirection > 5) chooseForceExploxionDirection = 0;
                if (chooseForceExploxionDirection < 0) chooseForceExploxionDirection = 5;
            }
        }
    }

    void Interact()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        if (Physics.Raycast(ray, out hit, rangeInteract))
        {                    
            if (hit.transform.GetComponent<SpeedForce>() && /**/ Time.timeScale != 1)//Esto es para no selecionar objetos fuera del tiempo realentizado.
            {

                // Temp //Si lo tocas tiene speed force porque el tiempo del juego no es 0 es 0.01, es decir, los objetos se mueven y aunque dejases el objeto interactuado en su sitio original de la partida el resto del escenario se ha movido, rompiendo el puzzle.
                hit.transform.GetComponent<SpeedForce>().MoveObject();


                print("You selected the " + hit.transform.name); // ensure you picked right object

                moveReference = true;

                speedForce = hit.transform.GetComponent<SpeedForce>();

                if (!desactivar)
                {
                    GameObject impactGO = Instantiate(particleTemp, hit.point, Quaternion.LookRotation(hit.normal));
                    
                    //
                    desactivar = true;
                    Invoke("ActivarSeleccionarObjetos", coldDown);
                }
            }
            else
            {
                print("No tiene SpeedForce " + hit.transform.gameObject.name);
            }
        }


     /*   RaycastHit hit;
        if (Physics.Raycast(rayCam.transform.position, rayCam.transform.forward, out hit, rangeInteract))
        {
           
            */
            /*Health target = hit.transform.GetComponent<Health>();
           
              if (target != null && hit.transform.tag != "Npc")
            {
                target.TakeDamage(-damageP);
            }

            GameObject impactGO = Instantiate(impactShootW1, hit.point, Quaternion.LookRotation(hit.normal));
            */
     //   }
    }

    void MoveReferenceObject()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.E)) //Cambia entre Mover/Rotar object
        {
            if (rotateReference)
            {
                rotateReference = false;
            }
            else{
                rotateReference = true;
            }
        }

        if (rotateReference && moveSwitchReference && speedForce)
        {
            speedForce.transform.Rotate(0.0f, -Input.GetAxis("Horizontal") * speedRotation, -Input.GetAxis("Vertical") * speedRotation);

            //Obj Referencia
            interactObject.transform.Rotate(0.0f, -Input.GetAxis("Horizontal") * speedRotation, -Input.GetAxis("Vertical") * speedRotation);
        }
        if (!rotateReference && moveSwitchReference && speedForce)
        {
            speedForce.transform.position = speedForce.transform.position + new Vector3(horizontalInput * speedMove * speedInSlowTime, verticalInput * speedMove * Time.deltaTime, 0);
            
            //
            //speedForce.transform.position = speedForce.transform.position + new Vector3(horizontalInput * speedMove * Time.deltaTime, verticalInput * speedMove * Time.deltaTime, 0);

            //Obj Referencia
            interactObject.transform.position = interactObject.transform.position + new Vector3(horizontalInput * speedMove * speedInSlowTime, verticalInput * speedMove * Time.deltaTime, 0);
        }


        //Le dice de donde sale la fuerza
        speedForce.ChooseForceExploxionDirection(chooseForceExploxionDirection);
    }

    void ActivarSeleccionarObjetos()
    {
        desactivar = false;
    }
}
