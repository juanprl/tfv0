using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    //POner Actor al pasarlo al juego.
    public enum Character
    {
        Npc,
        Police,
        Enemy
    }
    public Character t_character;

    public enum States
    {
        Resting,
        Attacking,
        Dead
    }
    public States t_states;

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
