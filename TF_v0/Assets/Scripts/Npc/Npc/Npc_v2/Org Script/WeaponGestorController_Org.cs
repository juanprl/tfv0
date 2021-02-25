using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGestorController_Org : IComparable<WeaponGestorController>
{
    //
    public List<WeaponGestorController> l_weaponGestorControllers;

    float shootRadiusWp; 
    float attackCoolDownWp;
    GameObject wp; 
    GameObject bulletWp;
    GameObject shootParticleWp;  
    string specialConditions0;
    float speedAttack;

    public WeaponGestorController_Org(float shootRadiusWpX, float attackCoolDownWpX, GameObject wpX, GameObject bulletWpX, GameObject shootParticleWpX,string specialConditions0X, float speedAttackX)
    {
        shootRadiusWp = shootRadiusWpX;
        attackCoolDownWp = attackCoolDownWpX;
        wp = wpX;
        bulletWp = bulletWpX; 
        shootParticleWp = shootParticleWpX;  
        specialConditions0 = specialConditions0X;
        speedAttack = speedAttackX;
    }

    public int CompareTo(WeaponGestorController other)
    {
        //Calcular 
        return 0;
    }
}
