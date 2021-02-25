using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;//TEmp


public class Contador : MonoBehaviour
{
    public Text objWithSpeedForce;
    public Text contObjWithSpeedForce;
    public Text objDirecctionWithSpeedForce;
    public static string nameObj;
    public static float contSpeedForce; //Contador de objetos con SpeedForce activa en la escena
    public static string objDirecction; //Ayuda visual.

    //
    public static Vector3 flechaPosition;
    public static Quaternion flechaRotation;
    public static Vector3 flechaScale;
    Vector3 flechaScaleOriginal;
    public GameObject flechaS;

    void Start()
    {
        flechaScaleOriginal = flechaS.transform.localScale; //Guarda su tamaño original para hacer el re-escalado.
    }

    // Update is called once per frame
    void Update()
    {
        objWithSpeedForce.text = nameObj;
        contObjWithSpeedForce.text = contSpeedForce.ToString();
        objDirecctionWithSpeedForce.text = objDirecction;

        //TEmp
        if (Input.GetKeyDown(KeyCode.T))
        {
            SceneManager.LoadScene("SampleScene");

            nameObj = " ";
            contSpeedForce = 0;
        }

        ColocarFlecha();
        if(Time.timeScale == 1) // Saca a la flecha de la vista del usuario //También se puede desactivar,alternativa.
        {
            flechaS.transform.position = new Vector3(0,-20,0);
        }
    }

    void ColocarFlecha()
    {
        //
        flechaS.transform.position = flechaPosition;
        //No acaba de orientarse bien, revisar. Arreglar a Futuro, cuando se decida el tipo de efecto que tendrá
        flechaS.transform.rotation = flechaRotation;

        //Funciona pero quiero que con los objetos grandes sea grande y con los pequeños pequeño
        //Pos: Si es menor que X multiplicar Scale por XX, así para que según el tamaño del objeto sea de un tamaño u otro.

        flechaS.transform.localScale = new Vector3 (flechaScaleOriginal.x * (flechaScaleOriginal.x / flechaScale.x), flechaScaleOriginal.y * (flechaScaleOriginal.y / flechaScale.y), flechaScaleOriginal.z * (flechaScaleOriginal.z / flechaScale.z));
    }
}
