using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStatistics : MonoBehaviour
{
    //Condiciones especiales en armas comunes estan asociadas al nombre, es decir, el arma parabola te quedas quieto siempre para evitar chocarte, el lanzacohetes solo tiene dos misiles, etc.
    //Las armas "especiales" en vez de un bool tendrán un array del 0 al X, cada valor equivale a una condción. X ataques y luego se rompe el arma, etc.
    public Transform firePoint;
    [Tooltip("Si radio de disparo es menor que X, es un arma cuerpo a cuerpo. Aunque dispare balas// Revisar.")]
    public float shootRadiusWp = 30f; //Distancia a la que disparan.
    public float attackCoolDownWp = 1.5f;
    public GameObject bulletWp; //bullet
    public GameObject shootParticleWp; //shootParticle
    [Tooltip("Ej: aaabbb o ababb. a = 1f Tiempo en el que el personaje no se mueve tras disparar b = 1f tiempo en el que no puede disparar otra arma ")] //Cada letra tiene un valor acumulativo, para pararlo 5f, pon cinco veces a (aaaaa). 
    public string specialConditions0;
    public float speedAttackWp = 1f;
    public int bulletsCont = 2000;
    public float shootProbably = 1f;
    //Header vacio para dejar espacio
    [Tooltip("Sonido de disparo")] 
    public AudioClip soundEfect;

    //¿ Poner cuanto resta cada disparo, float? tipo un Bazooca no resta igual que una pisola común?

    //podemos usar un string, algo tipo A= sin munición B = X.
    //Y que el resultado sea algo como " A B X Z"
    //Ej: Si al disparar tiene que pararse. Número de disparos limitados.

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
