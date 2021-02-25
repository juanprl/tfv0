using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcController : MonoBehaviour
{
                            [Header("Npc")]
    [Tooltip("0 es NPC, 1 es Enemy/Mod. Si es NPC, sus objetivos son los Enemy y viceveresa")]
    public int be;
    [Tooltip("Distancia a la que siguen a sus enemigos")]
    public float lookRadius = 10f;
    public float turnspeed = 5f;
    //public float damageArea = 1f;
                            [Header("Weapon 1")]
    [Tooltip("Distancia a la que disparan, si tienen el arma adecuada")]
    public float shootRadius = 30f;
    public GameObject gun;
    public GameObject bullet;
    public Transform firePoint;
    public ParticleSystem shootParticle;
                            [Header("Weapon 2")]
    [Tooltip("Distancia a la que golpean, si tienen el arma adecuada")]
    public GameObject weapon;
    public float puchRadius = 3.5f;
                            [Header("Weapon 3")]
    public GameObject parabolaBullet;
    public float parabolaCoolDown = 6f;
    public float parabolaRadius = 8f; //Tal vez lo mejor sería hacer un array con varias distancias, tipo esta guardado con 3 distancias, según la distancia cambia la posición del target y puede reducir o aumnetar el tiempo del disparo.

    [Header(" ")]
    //Estos valores habrá que pasarlos a las armas
    public float attack = 15f;
    public float attackSpeed = 1f;
    public float attackCoolDown = 0f;

    Transform target;
    NavMeshAgent agent;

    PlayerManager m_playerManager;

    public int followPlayer = 2;
    int cont = 0;
    string searchFollow;

    Vector3 sumarV3 = new Vector3(0, 0, 3);

    //
    Animator anim;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        
        if (be == 0) searchFollow = "Enemy"; //Weeb
        if (be == 1) searchFollow = "Player"; //Npc



    }

    void Update()
    {
        if (!anim.GetBool("Muerto"))//Solo se mueve si no esta la animación de muerto
        {
            MovementNpc();
        }
        attackCoolDown -= Time.deltaTime;

        //Weeb
        if (Input.GetKeyDown(KeyCode.F) && be != 1)//Si pulsas F, los Npc te siguen los enemy no.
        {
            if (followPlayer % 2 != 0)
            {
                //Este método solo funciona si solo hay un scrip ¿o objeto? llamado así
                m_playerManager = FindObjectOfType<PlayerManager>(); Transform targetXXX = m_playerManager.playerPosition;
                agent.SetDestination(targetXXX.position);
                searchFollow = "";
            }
            else
            {
                searchFollow = "Enemy";
                /* target = this.transform;
                searchFollow = "";*/
            }
            followPlayer++;
        }
        if (Input.GetKeyDown(KeyCode.G) && be != 1)//Si pulsas los Npc siguen a los enemy.
        {
            searchFollow = "Enemy";
            //followPlayer++;
        }     
    }

    void FaceTarget() 
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnspeed);
    }

    void MovementNpc()
    {
       

        target = FindClosestEnemy().transform;//llamar a este método tantas veces puede ser un consumo elevado de recursos (realmente nuestro juego está bastante vacío, y no afecta mucho). Pero podemos llamarle menos veces por segúndo, metiendo InvokeRepeating cada medio o un tercio de segundo en vez de cada frame

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {   
            lookRadius = lookRadius*2;//Una vez el Npc encuentre al Player, su visión se dobla.
            agent.SetDestination(target.position);
            //Para atacar cuerpo a cuerpo usaremos stoppingDistance
            if (distance <= agent.stoppingDistance)
            {
                //Npc
                if (be == 1)
                {
                    Shoot();                  
                }

                FaceTarget();
                anim.SetInteger("condition", 1);
            }
            //Para disparar usaremos shootRadius //Disparan corriendo o se parar y disparan? Depende del nivel. Los de nivel bajo son solo ven anime doblado para no tener que leer y atender a la historia, ellos solo pueden disparar quietos XD, no es seguro que lo dejemos. Tal vez esta capacidad es aleatoria
            
            //El problema de este sistema, es que dispararán o se pararán aunque haya paredes delante.
            if(distance < shootRadius && distance > puchRadius && weapon)
            {
                Shoot();              
            }
            if (distance < puchRadius)
            {
                Attack();
            }
            else
            {
                NoAttack();
            }

            if (distance < parabolaRadius && distance > puchRadius)
            {
                parabolaAttack();
            }
        }
        else
        { 
            lookRadius = 10f; //Una vez lo pierdas, vuelve a ser la original. 
        }
        anim.SetInteger("condition", 0);
    }

    void Shoot()
    {
        if(attackCoolDown <=0)
        {
            //sirve todavía??//target hacer daño weeb/player
            attackCoolDown = 1f / attackSpeed;
            if (gun)//hay un arma asignada
            {
                agent.SetDestination(transform.position);//Se para 
                //anim.Set
                
                Instantiate(bullet, firePoint.position - (Vector3.forward * 1), firePoint.rotation);

                //Bala que sigue
                /*GameObject bulletGO = (GameObject)Instantiate(bullet, firePoint.position, firePoint.rotation);
                 BulletController bulletSeek = bulletGO.GetComponent<BulletController>();
                if (bulletSeek)
                {
                    bulletSeek.Seek(target);
                }
                */
            }         
        }
    }

    void Attack()
    {
        if (weapon)
        {
            weapon.SetActive(true);
            anim.SetBool("Sword slash", true);
        }
    }
    void NoAttack()
    {
        weapon.SetActive(false);
        anim.SetBool("Sword slash", false);
    }

    //La parabola enemiga se lanza más aparecer 
    void parabolaAttack()
    {

        if (attackCoolDown <= 0)
        {
            //sirve todavía??//target hacer daño weeb/player
            attackCoolDown = 1f / attackSpeed;

            //Tiene que tener su propio enfriamiento y condiciones 
            GameObject parabolaGO = (GameObject)Instantiate(parabolaBullet, transform.position + sumarV3, transform.rotation);
            Destroy(parabolaGO, 7f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
         if (other.gameObject.tag == searchFollow)
         {
            cont++;//Tal vez si ponemos *TimeDeltatime podamos haacer que sea 1f por segundo// cont= +Time.deltaTime;
            Debug.Log("yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy" + cont);
         }      
    }

    private void OnTriggerExit(Collider other)
    {
      if (other.gameObject.tag == searchFollow)
      {
          Health target = other.gameObject.GetComponent<Health>();
          target.TakeDamage(cont);// * damageArea //Así solo harán daño si tienen daño en area.//ADEMÁS AÑADIR UN MULTIPLCADOR DE TIEMPO.
          //Debug.Log("Antes de ser 0, ha sido " + cont);
          cont = 0;
      }
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
}
