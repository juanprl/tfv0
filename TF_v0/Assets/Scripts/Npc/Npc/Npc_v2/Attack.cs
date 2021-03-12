using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;


public class Attack : MonoBehaviour
{ 
    float lookRadius = 0f;
    float lookRadiusCopy = 0f;
    float lookRadiusMultiply = 1f;
    float stoppingDistance = 999f;
    float speedNavMeshCopy;
    Transform firePoint;
    float distance;
    float attackCoolDown;

    //Conditions
    string conditions;
    char[] letras;
    [Tooltip("Poner el GameObject del Npc")]
    Transform targetNpc; //Este es el que se utiliza dentro, equivaldría a Target.
    bool stand; //Se queda quieto al disparar.
    float aCount = 0f;


    //List
    float shootRadiusWp;
    //Temp
    float attackCoolDownWp = 1.5f;
    GameObject wp;
    GameObject bulletWp;
    GameObject shootParticleWp;
    string specialConditions0;
    float speedAttack;

    string searchFollow;//Indica a quien buscar.
    Transform target; //Sustituye a SetDestination() //Este es el que se envía a otros Códigos. 
    Transform targetTemp; //Este es el que se utiliza dentro, equivaldría a Target.

    NpcsController m_npcsController;
    WeaponsController m_weaponsController;
    //
    [Tooltip("Poner el GameObject que almacena todas las Armas")]
    public GameObject weapons;
    [HideInInspector]
    public GameObject[] weapons_gestor; //El número a la derecha indica el máximo de armas que puedes cargar. 
    [HideInInspector]
    public Gun_v2[] weapons_Scriptable;
    public GameObject sphereDetection;

    int contWeapon = 0;
    public float f_radiousGrabWeapon = 0;

    //Daño en Área.
    bool playerDamage = true;
    bool allyDamage = true;
    bool enemyDamage = true;
    bool objectDamage = true;

    //Sonido
    AudioSource audioSource;

    //Las armas que usen en Tf serán invisibles y las que se verán serán armas que esten unidas a la posición de un gobj
    // por aim constrain, ya que, se deben poder quitar dentro del juego. Es decir, disparará del invisible pero se verá el interactuable.
    //Hay que tocar el rigidbody cuando copia la transformación del insible

    //Debería haber dos script Attack uno para personajes estáticos que solo van a tener sus armas, y otros que van a depender de agarrar/perserla.
    //Pos: Los que no cambian de arma tienen el mismo código que los que si que cambian, pero se pone que no pueden perderla/separse para reutilizarlo.

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        //
        m_npcsController = GetComponent<NpcsController>();
        m_weaponsController = GetComponent<WeaponsController>();

        lookRadius = GetComponent<NpcsController>().lookRadius;
        lookRadiusMultiply = GetComponent<NpcsController>().lookRadiusMultiply;
        stoppingDistance = GetComponent<NavMeshAgent>().stoppingDistance;
        //
        lookRadiusCopy = lookRadius;

        //
        playerDamage = GetComponent<NpcsController>().playerDamage;
        allyDamage = GetComponent<NpcsController>().allyDamage;
        enemyDamage = GetComponent<NpcsController>().enemyDamage;
        objectDamage = GetComponent<NpcsController>().objectDamage;

        //
        speedNavMeshCopy = GetComponent<NavMeshAgent>().speed;

        //Guarda las armas del Npc en un array
        if (weapons)
        {
           /*/ while(contWeapon < weapons.transform.childCount)//weapons.transform.GetChild(contWeapon)
            {
                //Npc tiene en su gameobject otros X gameobject que llevará un tag (por ahora, luego se sustituirá para que sea leyendo
                //el nombre), este tag puede ser corta, larga, mediana, muy larga(arma gigante). SI Npc tiene vacio un Gobj con el mismo tag
                //que el arma en el suelo, la coge, enviando la información al objeto.
                //En un futuro habria que meter animaciones de cambiar de arma,coger,etc.
                //Si tuviese su Gobj hueco habría que tener encuenta un sistema de decisión.
                weapons_gestor[contWeapon] = weapons.transform.GetChild(contWeapon).gameObject;

                //print(contWeapon);
                contWeapon++; 
            }*/

            //Temp
            /*int i = 0;
            while (i< weapons_gestor.Length-1)
            {
                print( "Npc arma " +i + " " + weapons_gestor[i].name);
                i++;
            }*/
            //Al hacer esto, provoca que si no haces métodos para recargar datos, el Npc solo podrá tener tantas armas como gameobject activos tenga al inicio en "weapons"
            contWeapon = 0;
            weapons_gestor = new GameObject[weapons.transform.childCount];
            while (contWeapon < weapons.transform.childCount)
            {
                weapons_gestor[contWeapon] = weapons.transform.GetChild(contWeapon).gameObject;
                contWeapon++;
            }
            weapons_Scriptable = new Gun_v2[weapons.transform.childCount];

            int i = 0;
            while (i < weapons_gestor.Length - 1)
            {
                print("Npc arma " + i + " " + weapons_gestor[i].name);
                i++;
            }
        }

        //Aquí lo suyo sería que lo cogiese automaticamente
        //targetNpc = GetComponent<>();   
    }

    void Update()
    {
        //
        GrabWeapon();

        attackCoolDown -= Time.deltaTime;

        if (searchFollow != null)
        {
            targetTemp = FindClosestEnemy().transform;

            distance = Vector3.Distance(targetTemp.position, transform.position);

            if (distance <= lookRadius)
            {
                lookRadius = lookRadiusCopy * lookRadiusMultiply;//Una vez el Npc encuentre al Player, su visión aumenta.
                //print(lookRadius);

                //
                if(!stand)
                {
                    target = FindClosestEnemy().transform;
                    //Moverse();
                }
                
                //
                if( Vector3.Distance(targetTemp.position , transform.position) < stoppingDistance*1.0f)
                {
                    Parar();
                }
                else
                {
                    Moverse();
                }

                if (attackCoolDown <= 0)
                {
                    if (weapons)//hay un arma asignada
                    {
                        //Se queda quieto al disparar.                        
                        if (stand)
                        {
                            //Parar();
                            /* targetNpc = FindClosestEnemy().transform;
                            targetNpc.rotation = transform.rotation;
                            target = targetNpc;*/

                            //target = transform;

                            //Hay que revisarlo//Se para,  pero se mira a sí mismo de en vez al objetivo.
                            //anim.Set
                            print("asdsadasdsadsa");
                        }
                        SelectWeapon();//
                    }
                }              
            }
            else
            {
                lookRadius = lookRadiusCopy; //Una vez lo pierdas, vuelve a ser la original. 
            }
            //anim.SetInteger("condition", 0);
        }
    }

    //Animaciones
   void AttackX()
   {
        if (wp)
        {
            //wp.SetActive(true);
            //anim.SetBool("Sword slash", true);
        }
   }
    void NoAttack()
    {
        //wp.SetActive(false);
        //anim.SetBool("Sword slash", false);
    }
    void GrabWeapon()
    {
        //Hacer que si su inventario de armas esta lleno y con munición no busque.

        Collider[] colliders = Physics.OverlapSphere(transform.position, f_radiousGrabWeapon);
        foreach (Collider collider in colliders)
        {
            //Mejora una vez que todos los huecos de armas estén llenos, hay un if para que no busque entre los colliders.
            if (collider.CompareTag("little_weapon") || collider.CompareTag("medium_weapon") /*|| collider.CompareTag("")*/) //Es un flitro para evitar que gaste recursos tratando de coger cosas que no son armas
            {
                for (int i = 0; i < weapons.transform.childCount - 1; i++)
                {
                    if (collider.CompareTag(weapons_gestor[i].tag) && collider.GetComponent<SujetarObjeto>().f_weight_read_only == 0f /*&& weapons_gestor[i] == null*/ && weapons_gestor[i].activeSelf) //Sí comparte tag, su Gobj = null y no está desactivado, y el Gobj no esta sociado a nada
                    {
                        collider.GetComponent<SujetarObjeto>().SetConstraint(weapons_gestor[i]);
                        i = 99; //exit 

                        //Rev 
                        //No va - try 1// weapons_gestor[i] = collider.gameObject;
                        //No funciona weapons_usable[i] = collider.GetComponentInParent<GameObject>(); 


                        //try 2, añadir  el script de GStatic al gameobj, copiar valores y usarlos, una vez se suelte el arma, actualizar los valores del arma.
                        /*weapons_gestor[i].AddComponent<GunStatistics>();
                        weapons_gestor[i].Guns*/

                        //try 2.5 Solución temporal, las armas son solo visual y siguen las armas invisibles.

                        //try 3 Instantiate(collider.GetComponent<GunStatistics>().bulletWp);

                        //tr
                        /* pasamos bulletstatics a Scriptable, hacemos que cuando agarre un objeto guarde ese script o una copia
                         * para usarlo, modifica ese y luego al soltar el arma lo actualiza**/
                        //weapons_Scriptable[i] = collider.GetComponent<GunStatistics_v2>();
                       /*Pos solución OnTriggerEnter parece funcionar*/
                     /*   if ( collider.GetComponent<Gun_v2>() )
                        {
                            weapons_Scriptable[i] = collider.GetComponent<Gun_v2>();
                             

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
                        }*/
                    }
                    //print(collider.gameObject.name + " (weapon) ha sido cogida");
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, f_radiousGrabWeapon);
    }

    void SelectWeapon()
    {
        weapons_Scriptable = sphereDetection.GetComponent<DetectAndGrabWeapons>().GetWeaponScriptable();
        sphereDetection.GetComponent<DetectAndGrabWeapons>().Mostrar();

        bool shootNow = true;
        //
        if (weapons_Scriptable.Length > 1)
        {
            foreach (Gun_v2 go in weapons_Scriptable)
            {
                //si esta desactivado el weapongestor i el gunv2 Debe estar a null
                if (go.m_gunStatistics_V2 != null)
                {
                        print("");
                        GameObject tempBullet2 = Instantiate(go.GetComponent<GunStatistics_v2>().bulletWp, go.tr_firePoint.position - (Vector3.forward * 1), go.tr_firePoint.rotation);

                        //Reinicia
                        if (go.GetComponent<GunStatistics_v2>().shootProbably <= 0) go.GetComponent<GunStatistics_v2>().shootProbably = 1f;

                        //
                        if (distance <= go.GetComponent<GunStatistics_v2>().shootRadiusWp)
                        {
                            if (go.GetComponent<GunStatistics_v2>().bulletWp && attackCoolDown <= 0)
                            {
                                //Si el aleatorio es menor que su probabilidad de disparar, disparará. El shootNow es para que cuando un arma dispare, no dispare la siguiente arma. Y si son balas positivas/tiene balas.
                                if (UnityEngine.Random.Range(0f, 1f) <= go.GetComponent<GunStatistics_v2>().shootProbably /**/&& shootNow && go.GetComponent<GunStatistics_v2>().bulletsCont > 0)
                                {
                                    GameObject tempBullet = Instantiate(go.GetComponent<GunStatistics_v2>().bulletWp, go.tr_firePoint.position - (Vector3.forward * 1), go.tr_firePoint.rotation);
                                   // Destroy(tempBullet, 3f);//Lo suyo sería que se destruyese cuando impactase 

                                    //Para objetos/balas que no tengan el BulletController Script
                                    if (!tempBullet.GetComponent<BulletController_v2>())
                                    {
                                        print("No tiene BulletController_v2");

                                    }
                                    else
                                    {
                                        if (tempBullet.GetComponent<BulletController_v2>().bulletType == 1)
                                        {
                                            tempBullet.GetComponent<BulletController_v2>().Seek(target);
                                        }
                                        tempBullet.GetComponent<BulletController_v2>().playerDamage = playerDamage;
                                        tempBullet.GetComponent<BulletController_v2>().allyDamage = allyDamage;
                                        tempBullet.GetComponent<BulletController_v2>().enemyDamage = enemyDamage;
                                        tempBullet.GetComponent<BulletController_v2>().objectDamage = objectDamage;
                                    }

                                    //Sonido
                                    if (go.GetComponent<GunStatistics_v2>().soundEfect)
                                    {
                                        audioSource.clip = go.GetComponent<GunStatistics_v2>().soundEfect;
                                        audioSource.Play();
                                    }

                                    GameObject particleDestroy = Instantiate(go.GetComponent<GunStatistics_v2>().shootParticleWp, go.tr_firePoint.position - (Vector3.forward * 0.5f), go.tr_firePoint.rotation);
                                    Destroy(particleDestroy, 3f);

                                    go.GetComponent<GunStatistics_v2>().shootProbably -= 0.25f;
                                    go.GetComponent<GunStatistics_v2>().bulletsCont -= 1;
                                    shootNow = false;
                                    print(go.name + " ha sido disparada.");

                                    attackCoolDown = attackCoolDownWp;// Este coldown es general, es decir, el personaje no dispara en X. //Una mejora sería hacerlo para cada arma.

                                    conditions = go.GetComponent<GunStatistics_v2>().specialConditions0;
                                    SetConditions();

                                    print("Ha disparado " + name);
                                }
                            }
                            else
                            {
                                //SetAnimacion atacar con arma cuerpov cuerpo.
                                //SetConditions();
                            }
                        }
                }
            }
        }
        /*else
        {
            foreach (GameObject go in weapons_usable)
            {
                print("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

                GameObject tempBullet2 = Instantiate(go.GetComponent<GunStatistics>().bulletWp, go.GetComponent<GunStatistics>().firePoint.position - (Vector3.forward * 1), go.GetComponent<GunStatistics>().firePoint.rotation);

                //Reinicia
                if (go.GetComponent<GunStatistics>().shootProbably <= 0) go.GetComponent<GunStatistics>().shootProbably = 1f;

                //Si no tiene balas, es un arma cuerpo a cuerpo.
                if (go.GetComponent<GunStatistics>().bulletWp && attackCoolDown <= 0)
                {
                    GameObject tempBullet = Instantiate(go.GetComponent<GunStatistics>().bulletWp, go.GetComponent<GunStatistics>().firePoint.position - (Vector3.forward * 1), go.GetComponent<GunStatistics>().firePoint.rotation);
                    Destroy(tempBullet, 3f);//Lo suyo sería que se destruyese cuando impactase 
                                            //Para objetos/balas que no tengan el BulletController Script
                    if (!tempBullet.GetComponent<BulletController_v2>())
                    {
                        print("No tiene BulletController_v2");

                    }
                    else
                    {
                        if (tempBullet.GetComponent<BulletController_v2>().bulletType == 1)
                        {
                            tempBullet.GetComponent<BulletController_v2>().Seek(target);
                        }
                        tempBullet.GetComponent<BulletController_v2>().playerDamage = playerDamage;
                        tempBullet.GetComponent<BulletController_v2>().allyDamage = allyDamage;
                        tempBullet.GetComponent<BulletController_v2>().enemyDamage = enemyDamage;
                        tempBullet.GetComponent<BulletController_v2>().objectDamage = objectDamage;
                    }

                    //Sonido
                    if (go.GetComponent<GunStatistics>().soundEfect)
                    {
                        audioSource.clip = go.GetComponent<GunStatistics>().soundEfect;
                        audioSource.Play();
                    }

                    GameObject particleDestroy = Instantiate(go.GetComponent<GunStatistics>().shootParticleWp, go.GetComponent<GunStatistics>().firePoint.position - (Vector3.forward * 0.5f), go.GetComponent<GunStatistics>().firePoint.rotation);
                    Destroy(particleDestroy, 3f);

                    go.GetComponent<GunStatistics>().shootProbably -= 0.25f;
                    go.GetComponent<GunStatistics>().bulletsCont -= 1;

                    conditions = go.GetComponent<GunStatistics>().specialConditions0;
                    SetConditions();

                    attackCoolDown = attackCoolDownWp;// Este coldown es general, es decir, el personaje no dispara en X. //Una mejora sería hacerlo para cada arma.
                }
                else
                {
                    //SetAnimacion atacar con arma cuerpov cuerpo.
                    //SetConditions();
                }
            }
        }*/
    }
    //
    public Transform GetTarget()
    {
        return target;
    }

    //Aquí otros Scripts le dirán a quién buscar.
    public void SearchFollow(string follow)
    {
        searchFollow = follow;
    }

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

    void SetConditions()
    {
        //int i = 0;

        if (conditions != null)
        {
            letras = conditions.ToCharArray();
        }

        for(int i=0; i < letras.Length; i++)
        {
            switch (letras[i])
            {
                case 'a':
                    aCount += 1f;
                    break;
                case 'b':
                    //Temp
                    attackCoolDown += 1f;
                    break;
            }
        }

        //Es acumulativo!!!!???? Cómo hacerlo acumulativo???¿¿¿
        if (aCount > 0)
        {
            stand = true;
            print(aCount);
            Invoke("TimeMeter", aCount);
        }      

        // 
        conditions = null;
        aCount = 0;
    }
    void TimeMeter()
    {
        stand = false;
        aCount = 0;
    }

    void Moverse()
    {
        GetComponent<NavMeshAgent>().speed = speedNavMeshCopy;
    }

    void Parar()
    {
        GetComponent<NavMeshAgent>().speed = 0;
    }

    //Armas
    public GameObject[] GetWeaponGestor()
    {
        return weapons_gestor;
    }
    public Gun_v2[] GetWeapons_Scriptabler()
    {
        return weapons_Scriptable;
    }
}
