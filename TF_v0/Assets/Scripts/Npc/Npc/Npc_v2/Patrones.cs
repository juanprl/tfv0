using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Poner que sea obligatorio tener el script de Actor asociado
public class Patrones : MonoBehaviour
{
    //
    Actor m_actor;
    NpcsController m_npcsController;
    
    Attack m_attack;
    Resting m_resting;
    Dead m_dead;
    NpcAcction m_npcAcction;

    //Elementos para decidir en que estado está.
    float lookRadius = 0f;
    float lookRadiusCopy = 0f;
    float lookRadiusMultiply = 1f;

    //Ayuda visual para saber en que estado está el Npc.
    public GameObject cubo;

    public string search;//A quien tiene que seguir/atacar/...


    void Start()
    {
        //
        m_actor = GetComponent<Actor>();
        m_npcsController = GetComponent<NpcsController>();
        
        m_attack = GetComponent<Attack>();
        m_resting = GetComponent<Resting>();
        m_dead = GetComponent<Dead>();
        m_npcAcction = GetComponent<NpcAcction>();

        //
        lookRadius = GetComponent<NpcsController>().lookRadius;
        lookRadiusMultiply = GetComponent<NpcsController>().lookRadiusMultiply;
        lookRadiusCopy = lookRadius;
    }

    void FixedUpdate()
    {
        Actions();

        //Si el enemigo del Npc se sale de su campo de acción, pasa de Attacking a Resting.
        // En una versión más trabajada que usen un área para detectar, y un cono o cámara que emule el campo de visión. Tipo todo lo que esta en el cono reacciona. Pero lo que esta de espalda lo ignora a no ser que suceda X (si hace ruido suficiente ruido se gira, detectar enemigos no ocultos etc)
        //No sé si usar un cono con un Box collider-Trigger ya que consume menos que una cámara?, y si hay muchos npc costaría mucho rendimiento si fuesen cámaras. Pero si hay un objeto entre él y otro Npc como reaccionaría?. Hacer pruebas de rendimiento para enterder la diferencia. Posibilidad: el uso de cámaras solo esta activa para ciertos tipos de npc o npc cercanos al jugador y los otros usan un sistema menos preciso. Si se quiere usar un cono creo que se podría pero habría que usar mucho código para emular uuna cámara básica. Tipo: enfrente de tí a Xm hay El GObj X que mide Xm y tiene Xm de ancho. Y mis medidas son X, puedo psar para hacer X, si o no, No, haz rodeo.


        int cont = 0;
        Collider[] colliders = Physics.OverlapSphere(transform.position, lookRadius);
        foreach (Collider collider in colliders)
        {
            //Daño
            if (collider.tag == "Npc" || collider.tag == "Police")//Enemy, Player, Npc, Objetos con vida //Habrá que revisarlo cuando decidamos si hay daño amigo o no.
            {
                cont++;
                Debug.Log(collider.name + "en área de acción de " + name);//3
            }

            if (cont > 0 && m_actor.t_states != Actor.States.Dead)
            {
                lookRadius = lookRadiusCopy * lookRadiusMultiply;
                m_actor.t_states = Actor.States.Attacking;
            }
            else
            {
                lookRadius = lookRadiusCopy;
                m_actor.t_states = Actor.States.Resting;
            }
        }
        //if(m_actor.t_character == Actor.Character.Ally && pulsas X) {// El estado del Weeb pasa a Resting y se pone a seguir al Jugador. TAmbién hay que poner el radio al que funciona o si son a todos los weebs de la escena o si hay límite numerico }

        MostrarEstado();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    void Actions()
    {
        /* if (m_actor.t_character == Actor.Character.Enemy) { EnemyActions();}
         if (m_actor.t_character == Actor.Character.Npc) { NpcActions();}
         if (m_actor.t_character == Actor.Character.Ally) { WeebActions();}*/

        if (m_actor.t_states != Actor.States.Dead) 
        {
            if (m_actor.t_states == Actor.States.Attacking && m_actor.t_character != Actor.Character.Npc) { Attacking(); }
            if (m_actor.t_states == Actor.States.Resting) { Resting(); }
            if (m_actor.t_states == Actor.States.Attacking && m_actor.t_character == Actor.Character.Npc) { Acting(); } //Los Npc en vez atacar actuan. Por ahora.
        }
    }

    void Attacking()
    {
        search = "Player";

        //Hay que añadir que también ataque a los Weeb cercanos al jugador y no los ignore. Es decir, que cambie de objetivo. // Podemos hacer  como el ArmaParabola y que tengan un radio alrededor en el que si entran Weeb o Player ataque al más cercano de esos.
        if (m_actor.t_character == Actor.Character.Enemy) { search = "Police"; }   //"Player"
        if (m_actor.t_character == Actor.Character.Police) { search = "Enemy"; }
        m_attack.SearchFollow(search);
        m_npcsController.SetDestination(m_attack.GetTarget());

    }

    void Resting()//Follow Patrol
    {
        m_npcsController.SetDestination(m_resting.GetTarget());
    }

    void Acting()
    {
        m_npcsController.SetDestination(m_npcAcction.GetTarget());

        //Mejora: Tener en cuenta que un Npc puede pasar de huir a combatiente o tener múltiples posibles acciones.
        //En este juego no es necesario debido a su corta duración.
    }

    void Dead()
    {

    }

    void MostrarEstado()
    {
        if (m_actor.t_states == Actor.States.Dead) { cubo.GetComponent<Renderer>().material.color = new Color(0, 0, 0, 1); }
        if (m_actor.t_states == Actor.States.Attacking) { cubo.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 1); }
        if (m_actor.t_states == Actor.States.Resting) { cubo.GetComponent<Renderer>().material.color = new Color(0, 0, 1, 1); }

        //Añadir rotación al cubo para que sea más vistoso.
    }
}
