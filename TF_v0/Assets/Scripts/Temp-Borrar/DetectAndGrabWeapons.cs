using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectAndGrabWeapons : MonoBehaviour
{
    public GameObject weaponFather;
    Gun_v2[] weapons_Scriptable;
    GameObject[] weapons_gestor; 


    void Start()
    {
        SetValores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        
        //Mejora una vez que todos los huecos de armas estén llenos, hay un if para que no busque entre los colliders. //Hacer que si su inventario de armas esta lleno y con munición no busque.

        //En una versión más trabajada, si detecta un arma en este área se dirige al arma y luego cuando se haga la animación de coger se pasa a la mano. Se podría poner un gameobject en la mano y cuando este cerca lo coge. Es decir, un area para detectar y otro para activar el Constrain Parent

        if (collider.GetComponent<Gun_v2>() && collider.CompareTag("little_weapon") || collider.CompareTag("medium_weapon") /*|| collider.CompareTag("")*/) //Es un flitro para evitar que gaste recursos tratando de coger cosas que no son armas
        {
            for (int i = 0; i < weapons_gestor.Length - 1; i++)
            {
                if ( collider.CompareTag(weapons_gestor[i].tag) && collider.GetComponent<SujetarObjeto>().f_weight_read_only == 0f /*&& weapons_gestor[i] == null*/ && weapons_gestor[i].activeSelf) //Sí comparte tag, su Gobj = null y no está desactivado, y el Gobj no esta sociado a nada
                {
                  /*  collider.GetComponent<SujetarObjeto>().SetConstraint(weapons_gestor[i]);
                    i = 99;*/

                    //try 2, copiar valores y usarlos, una vez se suelte el arma, actualizar los valores del arma.

                    //try 2.5 Solución temporal, las armas son solo visual y siguen las armas invisibles.

                    //tr
                    /* pasamos bulletstatics a Scriptable, hacemos que cuando agarre un objeto guarde ese script o una copia
                     * para usarlo, modifica ese y luego al soltar el arma lo actualiza**/
                    //weapons_Scriptable[i] = collider.GetComponent<GunStatistics_v2>();
                    /*Pos solución OnTriggerEnter parece funcionar*/

                    weapons_Scriptable[i] = collider.GetComponent<Gun_v2>();

                    /*
                        try
                        {
                            if (weapons_Scriptable[i].m_gunStatistics_V2 != null)
                            {
                                print(" Maybe " + weapons_Scriptable[i].m_gunStatistics_V2.bulletsCont);
                            }
                        }
                        catch (NullReferenceException ex)
                        {
                            Debug.Log("Salto");
                        }
                    */
                    print(collider.gameObject.name + " (weapon) ha sido cogida");
                }
            }
        }
    }

    //
    public void SetValores()
    {
        weapons_gestor = new GameObject[weaponFather.GetComponent<Attack>().weapons_gestor.Length];
        weapons_gestor = weaponFather.GetComponent<Attack>().GetWeaponGestor();
        weapons_Scriptable = new Gun_v2[weaponFather.GetComponent<Attack>().weapons_gestor.Length];
        weapons_Scriptable = weaponFather.GetComponent<Attack>().GetWeapons_Scriptabler();
    }

    public Gun_v2[] GetWeaponScriptable()
    {
        return weapons_Scriptable;
    }

    public void Mostrar()
    {
        for( int i=0; i < 9; i++)
        {
            try 
            {
                print(" sssss " + weapons_Scriptable[i].m_gunStatistics_V2.bulletsCont + " num " + i);
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("Salto");
            }
        }
        //Instantiate(weapons_Scriptable[7].m_gunStatistics_V2.bulletWp);
    }
}
