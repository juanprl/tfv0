using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinConditions : MonoBehaviour
{
    [Header("Condiciones")]
    public bool wachtTime; //Completar el nivel en X tiempo. //Igualmente se usará para saber el tiempo que has tardado sea condicionante o no.
    public bool salvarCiudadanos; //Hay Npc que no pueden morir para completar el nivel. El número se determina en la función.
    public bool incapacitarEnemigos; //Matar o quedar inconsciente o atrapados // Si son matados dan menos puntos. //En  dificultades altas, matar enemigos para pasar de nivel esta limitado.
    public bool herirNpc; //matar-herir por accidente a Npc ajenos. //En los primeros niveles será solo informativo, en dificultades alta estará limitado

    [Header("Objetos Condicionantes de victoria")]
    public GameObject[] npc;
    public GameObject[] enemy;
    
    //Header
    [Range(0, 15)] public int saveNpcCount = 0;
    int countNpc;

    //
    public int arrestEnemyCount = 0;
    int countEnemy;
    //Tendríamos que tener un int para enemigos muertos, enemigosArrestados o incapacitados, para hacer los cálculos de los puntos. //Para hacer la cuenta de arrestEnemyCount se sumarán los diferentes int.
   
    //
    float time;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void SaveCiudadanos()
    {
        if(npc.Length > 0 && salvarCiudadanos)
        {
            //Sustituir por Text en el UI
            print("Salva a " + npc.Length);

            for(int i = 0; i < npc.Length /* Revisar*/; i++)
            {
                if (npc[i].GetComponent<Health>().lifePoints > 0f)
                {
                    countNpc++;
                }
                
            }

            if(countNpc > saveNpcCount)
            {
                //WinGame();
            }
            else
            {
                //LostGame();
            }
        }

        //Contar por cuantos se compone el gameobject npc y pasarlos a un array?
        //Si savaNpcCount > 0 y GameObject npc > 0
        //contar ciudadanos que su vida es mayor a 0,//si el numero es menor que savaNpcCount. misión fracasada
    }

    void ArrestEnemigos()
    {
        if (enemy.Length > 0 && incapacitarEnemigos)
        {
            //Sustituir por Text en el UI
            print("Incapacita a " + npc.Length);

            for (int i = 0; i < enemy.Length  /* Revisar*/; i++)
            {
                if (npc[i].GetComponent<Health>().lifePoints < 0f /*|| Actor == inconsciente o atrapado*/ )
                {
                    countEnemy++;
                }

            }

            if (countNpc > arrestEnemyCount)
            {
                //WinGame();
            }
            else
            {
                //LostGame();
            }
        }
    }

    void TimeControl()
    {
        time = Time.time;

        if (wachtTime)
        {

        }
    }

    void LostGame()
    {
        //Activar paneles/canvas/
    }

    void WinGame()
    {
        //Activar paneles/canvas/
    }
}
