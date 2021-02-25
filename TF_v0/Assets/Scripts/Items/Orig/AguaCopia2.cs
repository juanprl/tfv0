using System;
using System.Collections;
using UnityEngine;

public class AguaCopia2 : MonoBehaviour
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
	
	/*void OnTriggerEnter(Collider col)
    {
		if(col.tag == "Agua")
		{
			dentroAgua = true;
 		}
	}
	void OnTriggerExit(Collider other)
    {
       if(other.tag == "Agua")
		{
			dentroAgua = false;
 		}
    }*/
	void OnTriggerStay(Collider other)
    {
       if(other.tag == "Agua")
		{
			dentroAgua = true;
			//Calcula la altura
				 Vector3 sizeVec = other.bounds.size;//Vector3 sizeVec = GetComponent<Collider>().bounds.size;
				 Debug.Log(sizeVec.y);
			//
			waterLevel = sueloObjeto.transform.position.y + (sizeVec.y - 0); //Revisar si es verdad. Te dice la altura de la base por lo que necesitamos saber el alto del mesh para sumarlo, y que el objeto quede en la superficie o haga otro calculo
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
//Hacer para que solo funcione cuando este en contacto con agua. Ver tipos de coll, hay uno solo para cuando este en contacto.

	
	/*
	Teletransportarlos a todo con NPC o teletransportable tag

	void OnTriggerEnter (Collider coll)
	{
		if (coll.tag == "Portal1CaraA")//Hazlo segun el nombre y no tag. Por limpieza. Que gire según lacara tantosº
		{
			Teleport();
		}
	}

	public Transform reciever;

		if (Collider tag = portal)
		{
		
				// Teleport him!
				float rotationDiff = -Quaternion.Angle(transform.rotation, reciever.rotation);
				rotationDiff += 180;
				player.Rotate(Vector3.up, rotationDiff);

				Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
				player.position = reciever.position + positionOffset;

				playerIsOverlapping = false;

				send
			}
		}
	}

}

	
}*/


