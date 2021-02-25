using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //Scrip que se coloca en la cámara para que siga al ratón
    public float mouseSensitivity = 100f;
    public float speedInSlowTime = 0.78f;//Esto podría cogerse de una variable global para facilitar el código

    float mouseX, mouseY;

    public Transform playerBody;

    float xRotation = 0f;



    void Start()
    {
        //  Cursor.lockState = CursorLockMode.Locked; //Tener este activo te impide usar el canvas
    }

    // Algún día puede que MouseX/Y cambie la termiinación ese día ver la nueva clasificación.
    void Update()
    {
        if (Time.timeScale == 1)
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        }
        else
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * speedInSlowTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * speedInSlowTime;
        }        

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);// Son los ángulos en los que te permite ver (-90 y 90, Arriba y abajo)

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
