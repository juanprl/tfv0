using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NpcsController : MonoBehaviour
{
                [Header("Características")]
    public float turnspeed = 5f;
    public float lookRadius = 5f; //Radio por el que Npc pasará a Ataque
    [Tooltip("Si se deja en uno, no pasará nada.")]
    public float lookRadiusMultiply = 1f; //Cuando un Npc entra en contacto con su enemigo, su Radio de visión aumentará.

                [Header("Activar/Desactivar Fuego Amigo para diferentes Tags")]
    public bool playerDamage = true;
    public bool allyDamage = true;
    public bool enemyDamage = true;
    public bool objectDamage = true;
   
    //
    string searchFollow;//Indica a quien buscar.
    Transform target;
    NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        agent.SetDestination(target.position);
        FaceTarget();
    }

    public void SetDestination(Transform xxx)
    {
        target = xxx;
    }

    void FaceTarget()
    {
        /*transform.LookAt(target);*/
         Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnspeed);
    }

    /*

     GameObject FindClosestEnemy()
     {
         GameObject[] gos;
         gos = GameObject.FindGameObjectsWithTag(searchFollow);
         GameObject closest = null;
         float distance = Mathf.Infinity;
         Vector3 position = transform.position;
         foreach (GameObject go in gos)
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
    */
}
