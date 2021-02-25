using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialsAttacks : MonoBehaviour
{
    /*Los ataque especiales, serán cosas como arrollar, saltos, y otros. 
    En el caso de arrollar tiene que empezar de lento a rápido hacia una dirección. Esta velocidad y dirección se calcula aquí, y se pasa al script Attack.
    Otros como el salto igual, se calcula aquí la velocidad a la que sube y donde cae.
    */
    //Las armas "especiales" en vez de un bool tendrán un array del 0 al X, cada valor equivale a una condción. X ataques y luego se rompe el arma, etc.
    public Transform firePoint;
    [Tooltip("Si radio de disparo es menor que X, es un arma cuerpo a cuerpo. Aunque dispare balas// Revisar.")]
    public float shootRadiusWp = 30f; //Distancia a la que disparan.
    public float attackCoolDownWp = 1.5f;
    //public GameObject bulletWp; //bullet
    public GameObject shootParticleWp; //shootParticle
    [Tooltip("Ej: aaabbb o ababb. a = 1f Tiempo en el que el personaje no se mueve tras disparar b = 1f tiempo en el que no puede disparar otra arma ")] //Cada letra tiene un valor acumulativo, para pararlo 5f, pon cinco veces a (aaaaa). 
    public string specialConditions0;
    public float speedAttackWp = 1f;
    public int bulletsCont = 2000;
    public float shootProbably = 1f;

    [Tooltip("GameObjects por defecto")]
    public GameObject shootParticleDefect;

    void Start()
    {
        //Hacer que Transform firepoint y shootParticleDefect se coja por script y no manual.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
