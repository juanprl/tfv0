using System;
using System.Collections;
using UnityEngine;

public class Agua : MonoBehaviour
{
	//Según el objeto y el efecto deseasdo hará que ajustar los parametros idividualmete	
	//Los ojeto deería estar co Posicioes gloales o e el mismo cotexto

	//METER UN RIGBODY DEBAJO DEL PLAYER PARA MOVERSE Y UN IF POR LA ALTURA PARA FUNCIONAR

	public float waterLevel;//Debes poner el nivel del agua/superficie del objeto
	public float floatHeight;
	public float  bounceDamp;
	public Vector3 buoyancyCentreOffset;
	public GameObject sueloObjeto;

	Rigidbody rb;
	public bool dentroAgua;

	void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

	void OnTriggerStay(Collider other)
    {
       if(other.tag == "Water")
		{
			dentroAgua = true;
			//Calcula la altura
				 Vector3 sizeVec = other.bounds.size;//Vector3 sizeVec = GetComponent<Collider>().bounds.size;
				 Debug.Log(sizeVec.y);
			
			//waterLevel = sueloObjeto.transform.position.y + (sizeVec.y - 0); //Revisar si es verdad. Te dice la altura de la base por lo que necesitamos saber el alto del mesh para sumarlo, y que el objeto quede en la superficie o haga otro calculo
 		}
    }

	void OnTriggerExit(Collider other)
    {
       if(other.tag == "Water")
		{
			dentroAgua = false;
 		}
    }

	void FixedUpdate () {
		Vector3 actionPoint = transform.position + transform.TransformDirection(buoyancyCentreOffset);
		float forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);
		
		if (forceFactor > 0f && dentroAgua == true) {
			Vector3 uplift = -Physics.gravity * (forceFactor - rb.velocity.y * bounceDamp);
			rb.AddForceAtPosition(uplift, actionPoint);
		}
	}
}