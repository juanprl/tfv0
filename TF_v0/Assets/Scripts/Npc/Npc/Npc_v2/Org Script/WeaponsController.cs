using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    //Si radio de disparo es menor que X, es un arma cuerpo a cuerpo. Aunque dispare balas// Revisar.
    //Condiciones especiales en armas comunes estan asociadas al nombre, es decir, el arma parabola te quedas quieto siempre para evitar chocarte, el lanzacohetes solo tiene dos misiles, etc.
    //Las armas "especiales" en vez de un bool tendrán un array del 0 al X, cada valor equivale a una condción. X ataques y luego se rompe el arma, etc.
    public Transform firePoint;
                [Header("Weapon 1")]   
    public float shootRadiusWp1 = 30f; //Distancia a la que disparan.
    public float attackCoolDownWp1 = 1.5f;
    public GameObject wp1; //gun
    public GameObject bulletWp1; //bullet
    public GameObject shootParticleWp1; //shootParticle
    public string specialConditions01;
    public float speedAttackWp1 = 1f;
    //podemos usar un string, algo tipo A= sin munición B = X.
    //Y que el resultado sea algo como " A B X Z"
    //Ej: Si al disparar tiene que pararse. Número de disparos limitados. 
                [Header("Weapon 2")]
    public float shootRadiusWp2 = 3f;
    public float attackCoolDownWp2 = 1.5f;
    public GameObject wp2;  
    public GameObject bulletWp2; 
    public GameObject shootParticleWp2;
    public string specialConditions02;
    public float speedAttackWp2 = 1f;
                [Header("Weapon 3")]
    public float shootRadiusWp3 = 12f;
    public float attackCoolDownWp3 = 1.5f;
    public GameObject wp3;
    public GameObject bulletWp3;
    public GameObject shootParticleWp3;
    public string specialConditions03;
    public float speedAttackWp3 = 1f;
    //
    [Tooltip("GameObjects por defecto")]
    public GameObject shootParticleDefect;
    public List<WeaponGestorController> l_weaponGestorControllers;

    void Start()
    {
        //Asignar partícula de impacto por defecto, si no tiene una puesta.
        if (wp1 && !shootParticleWp1 ) { shootParticleWp1 = shootParticleDefect; }
        if (wp2 && !shootParticleWp2) { shootParticleWp2 = shootParticleDefect; }
        if (wp3 && !shootParticleWp3) { shootParticleWp3 = shootParticleDefect; }

        List<WeaponGestorController> l_weaponGestorControllers = new List<WeaponGestorController>();
        if (wp1) { l_weaponGestorControllers.Add(new WeaponGestorController (shootRadiusWp1, attackCoolDownWp1, wp1, bulletWp1, shootParticleWp1, specialConditions01, speedAttackWp1)); }
        if (wp2) { l_weaponGestorControllers.Add(new WeaponGestorController (shootRadiusWp2, attackCoolDownWp2, wp2, bulletWp2, shootParticleWp2, specialConditions02, speedAttackWp2)); }
        if (wp3) { l_weaponGestorControllers.Add(new WeaponGestorController (shootRadiusWp3, attackCoolDownWp3, wp3, bulletWp3, shootParticleWp3, specialConditions03, speedAttackWp3)); }
    }
}
