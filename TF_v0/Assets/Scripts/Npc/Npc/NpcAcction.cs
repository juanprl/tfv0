using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;//Temp


public class NpcAcction : MonoBehaviour
{
    Actor m_actor;
    
    [Tooltip("Indica la cantidad de puntos en el que el personaje huirá")]
    [Range(1, 226)]
    public float fearMax = 100;
    [Tooltip("Si el fear llega a X puntos el personaje huirá")]
    //Si el script fuese más complejo, habría que hacer que se perdiese/bajase con el tiempo.
    public/**/ float fear;
    [Tooltip("Indica la cantidad de puntos/fear en el que el personaje cambiará de punto de huida (fearMax + fearChangePoint)")]
    public float fearChangePoint = 30;
    [Tooltip("Radio de detección de fear")]
    public float fearRadious = 3;
    [Header("Fear Points")]
    //Habría que hacer que sacasen estos valores de un script global y que fuese diferente para policía, enemigos, y npc. 
    public float fearEnemyAttackingPoints = 30;
    public float fearWachDeadBodyPoints = 50;
    public float fearWachPeopleEscapingPoints = 15;
    public float fearWachEnemy = 35;


    //¿Si mientras un personaje se mueve a ese punto y es golpeado/empujado se ve afectado? O el character controller se lo impide.


    List<GameObject> idRunPoints;    //Los puntos de huida el script los saca del escenario por su tag característico, EscapePoint
    Vector3 huirDeEsto;
    Transform huirDeEstoTemp; 
    Transform target;

    //
    NavMeshAgent m_navMeshAgent;
    [Tooltip("Guarda la velocidad original, es un valor fijo")]
    float m_navMeshAgentSpeed;
    float timeToWait;//El tiempo que le queda para poder moverse.
    float timeToStart = 0;//Calculos no tocar
    float timeToCheck = 0;//Calculos no tocar

    //Temp boora
    bool a = true;

    bool escaping; //Si esta huyendo la acción de "parar" no funciona
    public/**/ bool kidnapped; //Si es true, no puedes moverte/escaping.

    void Start()
    {
        m_actor = GetComponent<Actor>();
        //
        idRunPoints = new List<GameObject>();

        GameObject[] allRunPoints = GameObject.FindGameObjectsWithTag("EscapePoint");
        foreach (GameObject go in allRunPoints)
        {
            idRunPoints.Add(go);
        }

        //
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_navMeshAgentSpeed = m_navMeshAgent.speed;

    }

    // Update is called once per frame
    void Update()
    {
        Actor_No_NpcAcction(); 
        
        
        AddFear();
        //Que no funcionen si están muertos
      
        
        if (fear < fearMax) // hay que romper el tiempo de espera si el fear es > fearMax, para huir
        {
            escaping = false;
        }
        else
        {
            escaping = true;
        }

        WaitHere();

        //Hay que ponerlo debajo para que nointerfiera con Waithere -escaping
        //si las condiciones son NoEstarSecuestrado && enemy esta lejos o muerto o inconsciente puede huir
        if (!kidnapped)
        {
            //Cuando se una al script más elaborado, le enviará al principal que mientras kidnape = true 
            //le indicará que la speed es 0 para que no se mueva y una vez pueda moverse se dirija al sitio adecuado.

            m_navMeshAgent.speed = m_navMeshAgentSpeed;
        }
        else
        {
            m_navMeshAgent.speed = 0;
        }
        SetOffKidnapped();
    }


    GameObject FindClosestEscapePoint()   //EncuentraDondeHuir - Elige el punto más cercano de la lista
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = huirDeEsto;
        foreach (GameObject go in idRunPoints)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    public Transform GetTarget()
    {
        return target;
    }

    public void AddFear()
    {
        float fearTemp; 
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, fearRadious);

        //HAcer que ssolo se active una vez por cada vez que entran.
        //Pos: Que los personajes tengan un script para llevar el control de esto.   
        foreach (Collider collider in colliders)
        {
            fearTemp = fear;
            huirDeEstoTemp = collider.transform;

            if (collider.GetComponent<Actor>()) 
            {
                if (/*collider.tag == "Npc" &&*/ collider.GetComponent<Actor>().t_character == Actor.Character.Enemy)
                {
                    fear += fearWachEnemy;

                    //Temp Borrar
                    if (a)
                    {
                        SetWaitHere(2);
                        a = false;
                    }
                    
                    //El sistema de miedo es algo complejo, pero hay varias soluciones
                    /*-Solucion 1, la más sencilla: 
                     * Cada motivo de miedo encontrarse un cadáver, ver un enemigo, etc...
                     * solo puede pasar cada x tiempo e ignoramos quien la ha producido.
                     * -Solución 2, chapuzas:
                     * Instanciamos un prefab que tenga un script en su GameObject. En ese script añadimos los valores identificafivos a usar.
                     * Cada vez que se encuentra un nuevo peligro que no se encuentra entre los prefab, crea uno nuevo y mete los datos.
                     * A la hora de editar sus acciones lo que se hará será acceder a un list donde se almacenarán los prefab según se creen,
                     * buscar el que necesitas editar y hacerlo.
                     * Problemas:Cada Npc podría acabar con 20 prefabs, que realmente consumen poco ya que solo son listas, pero son cosas a tener en cuenta.
                     * 
                    -Sol 3, sencilla pero más completa: Crear un list que se guardan los gameobjects de los enemigos a los que te encuentras, 
                    si esta ahí no te da miedo porque ya te lo ha dado.  Y cada Xs se borra de la lista por si te lo vuelves a encontrar.
                    Problemas: solo guarda si te lo has encontrado o no, nada más.
                     */

                    /*
                    //Dispara
                    if ( collider.GetComponent<Attack>().Shooting == true )
                    {
                        fear += 1;
                    }*/

                    //Para que Funcione adecuadamente, los personajes deben estar orientados hacia la X. Mirar hacia X en la rotación local.
                    //Huirdeesto coge solo la posicion del último GameObject del que se huye que entra en el collider e invierte la posición para indicar a la IA que vaya a un punto cercano a esa nueva posición.
                    //De esta forma el Npc se dirige a la posición que no este mirando el enemigo.


                    if (fear >= fearMax && fearTemp < fearMax)//Si fearTemp es menor que fearMAx significa que antes de sumarle fear por entrar en el collider, todavía no se había fijado de lo que huir. Esto impide que se actualice constantemente.
                    {
                        huirDeEsto = collider.transform.position;

                        CalcularNuevoHuirDeEsto();
                    }
                    //Hace lo que debe, si va raro es por huirDeEsto
                    if (fear >= fearMax + fearChangePoint && fearTemp < fearMax + fearChangePoint) //Si alcanza el punto de fearChange, y antes no lo estaba busca un nuevo sitio.
                    {
                        
                        huirDeEsto = collider.transform.position;

                        CalcularNuevoHuirDeEsto();
                    }
                }
            }

            if (collider.tag == "Object"/* && enemyDamage == true*/) 
            { 
                 
            }
        }
    }
    void CalcularNuevoHuirDeEsto()
    {
        if (huirDeEstoTemp.eulerAngles.y >= 0 && huirDeEstoTemp.eulerAngles.y <= 90)
        {
            huirDeEsto = new Vector3(huirDeEsto.x - huirDeEsto.x, huirDeEsto.y, huirDeEsto.z - huirDeEsto.z);
        }
        if (huirDeEstoTemp.eulerAngles.y >= 90 && huirDeEstoTemp.eulerAngles.y <= 180)
        {
            huirDeEsto = new Vector3(huirDeEsto.x + huirDeEsto.x, huirDeEsto.y, huirDeEsto.z + huirDeEsto.z);
        }
        if (huirDeEstoTemp.eulerAngles.y >= 180 && huirDeEstoTemp.eulerAngles.y <= 270)
        {
            huirDeEsto = new Vector3(huirDeEsto.x + huirDeEsto.x, huirDeEsto.y, huirDeEsto.z + huirDeEsto.z);
        }
        if (huirDeEstoTemp.eulerAngles.y >= 270 && huirDeEstoTemp.eulerAngles.y <= 360)
        {
            huirDeEsto = new Vector3(huirDeEsto.x - huirDeEsto.x, huirDeEsto.y, huirDeEsto.z - huirDeEsto.z);
        }

        target = FindClosestEscapePoint().transform;
        fear = fearMax; //Reinicia

        //Temp m_navMeshAgent.SetDestination(target.position);       
    }
    public void SetKidnapped(bool x)
    {
        kidnapped = x;
    }
    void SetOffKidnapped()
    {
        //Por ahora estar secuestrado se quita si no hay enemigos cercanos vivos, pero habría que añadir
        //restricciones como esposas y demás que le impidan moverse.

        int x = 0;
        
        //Sí alrededor del Npc no hay nadie que sea Enemigo el Npc podrá huir.
        //Problema: Si empiezan el juego ya secuestrados puede que aunque su fear ya sea 100, no tengan sitio al que huir
        //por lo que habrá que situar puntos de miedo cercanos a ellos para que puedan huir, se alejen de ahí. O ponerles un sitio de huir predefinido.
        if (kidnapped) 
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, fearRadious * 2.5f /**/);
 
            foreach (Collider collider in colliders)
            {
                if (collider.GetComponent<Actor>())
                {
                    if (collider.GetComponent<Actor>().t_character == Actor.Character.Enemy)
                    {
                        x += 1;
                        if (collider.GetComponent<Actor>().t_states == Actor.States.Dead)
                        {
                            x--;
                        }
                    }
                }
            }
        }

        if (x == 0) kidnapped = false;
    }

    public void SetWaitHere(float timeWait) //Semarofos, puertas automáticas, el ascensor llegue y mientras esta dentro, que te sirvan en una tienda, colas... 
    {
        timeToWait += timeWait;
    }
    void WaitHere() //Controla el tiempo que un personaje se tiene que esperar  
    {
        //Mejora a futuro: crear un sistema de prioridades, donde ciertas acciones si o sí se tengan que esperar. Ejem: Puerta ascensor, esta tarda x seg en abrir, si sigue hacia delante se cae o se choca contra la puerta.
        //Dichos objetos como puertas automáticas y ascensores, que tengan health y si llegan a cero no envíen waithere, en el sentido. La puerta ha sido destruida, por qué te esperas X seg?
        
   
        if (!escaping)    //Si esta esperando y no está secuestrado o con enemigos cerca, y fear sube a fearMax, waithere se anula para que este pueda huir hacia la nueva posición.
        {
            timeToWait = 0;
        }

        timeToStart = Time.time + timeToWait;
        if (timeToStart <= Time.time)
        {
            m_navMeshAgent.speed = m_navMeshAgentSpeed;
        }
        else
        {
            m_navMeshAgent.speed = 0;
        }

        if (timeToCheck < (int)Time.time) //Ha pasado un segundo
        {
            timeToWait -= 1;
            if (timeToWait < 0)
            {
                timeToWait = 0;
            }
        }
        timeToCheck = (int)Time.time;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fearRadious);
    }

    //
    void Actor_No_NpcAcction() //Si el "Actor" pasa de Acction a Resting/Attackin/Dead anulará el resto de limitaciones de este script 
    {
        if (m_actor.t_states != Actor.States.Attacking && m_actor.t_character == Actor.Character.Npc) 
        { 
            timeToWait = 0;
        }
            
    }
}
