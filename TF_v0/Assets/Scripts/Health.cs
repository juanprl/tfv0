using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
   //Script para objetos y Npc, guarda y suma/resta vida. El animator controller va en otro script que saca de aquí los valores
   //de esta forma podemos herir a todos los objetos sin distinción.

    public float lifePoints = 1;
    public float maxLifePoints = 1;
    public Text t_lifeMarket; 
    [Tooltip("Altura a la que se te considera caído del escenario")]
    public float killHeight = -20;


    public GameObject danhoSimbolo;
    void Start()
    {
        MostrarVidaEnemiga();

        // maxLifePoints = lifePoints; //El máximo de vida será con el que empiece.

    }

    void Update()
    {
        if (lifePoints <= 0 || transform.position.y < killHeight)
        {
            //anim.SetBool("Muerto", true);
            Destroy(gameObject, 3f);
            //Invoke("Morir", 5f);
            //Alternativa a Invoke
            //StartCoroutine(DoDamage(5f));
        }

        //Temp // Para evitar llamadas innecesarias lo suyo sería que se llamase cada vez que recibe o pierde vida
        MostrarVidaEnemiga();
    }

    public void TakeDamage(float amount)
     {
        lifePoints -= amount;
        MostrarVidaEnemiga();

        if (danhoSimbolo)
        {
            danhoSimbolo.SetActive(true);
            Invoke("QuitarSimbolo",1f);
        }

        //Si vida es inferior a 0 habrá desmembramientos o destruccioes mas grandes.
     }

    public void TakeHeal(float recover)
    {
        //Esto evita recobrar salud si estas muerto.
      /*  if(m_actor.t_states != Actor.States.Dead)
        {
            lifePoints += recover;
            if (lifePoints > maxLifePoints)
            {
                lifePoints = maxLifePoints;
            }
            MostrarVidaEnemiga();
            Debug.Log("Aliado Curado");
        }*/

    }

    public void MostrarVidaEnemiga()
    {
        if(t_lifeMarket != null)
        {
            string lifeMarket = Mathf.Round(lifePoints) + "/" + maxLifePoints;
            t_lifeMarket.text =  lifeMarket;
        }
       /* //Mostrar la vida enemiga
        string lifeMarket = lifePoints + "/" + maxLifePoints;
        t_lifeMarket.text = lifeMarket;*/
    }

    public void SetLifePoints(float xlife)
    {
        lifePoints = xlife;
    }
    public float GetLifePoints()
    {
        return lifePoints;
    }
}
