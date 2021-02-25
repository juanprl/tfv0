using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; //Necesario para usar NavMesAgent


public class MovementNpc : MonoBehaviour
{
    public Transform objetive;
    NavMeshAgent agent;

    string search = "Finish";
    GameObject searchTag;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void objetivos()
    {
        if(Input.GetKeyDown(KeyCode.F))//Por algún motivo no funciona con el tag "Player"
        {
            search = "FollowPlayer";
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            search = "Enemy";
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            search = "Respawn";
        }
    }
    
    void Update()
    {
        objetivos();
        searchTag = GameObject.FindWithTag(search);
        objetive = searchTag.transform;
        agent.destination = objetive.position;

    }
}
