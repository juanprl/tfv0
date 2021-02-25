using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Attack1 : MonoBehaviour
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
    GameObject[] weapons_gestor = new GameObject[10];//El número a la derecha indica el máximo de armas que puedes cargar. 
    int contWeapon = 0;

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
            while(contWeapon < weapons.transform.childCount)//weapons.transform.GetChild(contWeapon)
            {
                weapons_gestor[contWeapon] = weapons.transform.GetChild(contWeapon).gameObject;
                //print(contWeapon);
                contWeapon++; 
            }
            /*
            //Temp
            int i = 0;
            while (i<contWeapon)
            {
                print( weapons_gestor[i].name);
                i++;
            }*/

        }
             
       //Aquí lo suyo sería que lo cogiese automaticamente
        //targetNpc = GetComponent<>();       
    }

    void Update()
    {
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
                        SelectWeapon();
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


    void SelectWeapon()
    {
        bool shootNow = true;
        //
        if ( weapons.transform.childCount > 1)
        {
            foreach (GameObject go in weapons_gestor)
            {
                //Reinicia
                if (go.GetComponent<GunStatistics>().shootProbably <= 0) go.GetComponent<GunStatistics>().shootProbably = 1f;

                //
                if (distance <= go.GetComponent<GunStatistics>().shootRadiusWp )
                {
                    if (go.GetComponent<GunStatistics>().bulletWp && attackCoolDown <= 0)//Habría que añadir si el arma/GameObject esta activo, por si esta en la lista pero no se puede usar todavía
                    {
                        //Si el aleatorio es menor que su probabilidad de disparar, disparará. El shootNow es para que cuando un arma dispare, no dispare la siguiente arma. Y si son balas positivas/tiene balas.
                        if (UnityEngine.Random.Range(0f, 1f) <= go.GetComponent<GunStatistics>().shootProbably /**/&& shootNow && go.GetComponent<GunStatistics>().bulletsCont > 0)
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
                            shootNow = false;
                            print(go.name + " ha sido disparada.");

                            attackCoolDown = attackCoolDownWp;// Este coldown es general, es decir, el personaje no dispara en X. //Una mejora sería hacerlo para cada arma.

                            conditions = go.GetComponent<GunStatistics>().specialConditions0;
                            SetConditions();

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
        else
        {
            foreach (GameObject go in weapons_gestor)
            {
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
        }
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
}
