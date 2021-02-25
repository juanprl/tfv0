using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class  Resting: MonoBehaviour
{
   //FollowOrder_v3 Versión para personaje.

    //No poner los objetos atravesando el suelo parcialmente o enteros o afectará al calcular las rutas y no lo detectará.
    
    //¿cómo hacer que esquiven obstáculos?  Pos 1:¿se puede hacer que gOb lleven Obstacle nav mesh apesar de moverse?
    //Pos 2: el npc lleva una lista de posiciones, en caso de encontrar un objeto que no se mueve y le impide avanzar, se generará un nuevo 
    //punto al que desplazarse y al llegar, volverá a desplazarse al punto original que se habñia guardado en el list. Problema: que pasa si se actualiza el destino final por otro script, pos solución: se limpia la lista si la nueva posición viene de otro script o se actualizan las posiciones.
    public int idPatrulla;

    public float _totalWaitTime = 1f;
    [Tooltip("Probabilidad para cambiar el orden del movimiento. Poner un valor de 0.01 -> 1, cuanto mayor sea, mayor será la probabilidad de cambiar orden. 0 para que no ocurra")]
    public float _swichProbability = 0f;//0.2f
    List<GameObject> idWayPoints;

    public bool _travelling;

    [Tooltip("Escribe el número del punto con el que quieres empezar. Sí no existe, comenzará al más cercano. El 99 está betado")]
    public int guardarPunto = 99;

    //
    Transform target;

    //Con Prefab en vez de Tag
    public GameObject pathToFollow;
    int contFollow = 0;

    //
    public float stoppingDistance = 999f;

    void Start()
    {
        stoppingDistance = GetComponent<NavMeshAgent>().stoppingDistance;


        idWayPoints = new List<GameObject>();


        //También se puede hacer que cojas un prefab y que los hijos se guarden su posición  y no usar tags.
        /*if (pathToFollow)
        {
            while (contFollow < pathToFollow.transform.childCount)
            {
                idWayPoints.Add( pathToFollow.transform.GetChild(contFollow).gameObject);
                contFollow++;
            }
        }*/

        
        //
        GameObject[] allWayPoints = GameObject.FindGameObjectsWithTag("WayPoint");
        foreach(GameObject go in allWayPoints)
        {
            if (go.GetComponent<Waypoint>().idPatrulla == idPatrulla)
            {
                idWayPoints.Add(go);
            }
        }
        Debug.Log("Puntos de Patrulla: " + idWayPoints.Count);

        //Temp //Nos dice el nombre de cada GameObject almcenado.
        /*int i = 0;
        while (i<contFollow)
        {
           print( idWayPoints[i].name);
           i++;
        }*/

        OrdenarArray();
        //Aquí veremos, si guardarPunto existe, si no es que el usuario se ha equivocado.Si no existe, guardarPunto = 99   
        ComprobarPuntoPreEstablecido();
        //Si hay dos idPunto iguales, no afecta ,ya que, están ordenados en el listado. Aun así se podría avisar por Debug.
        //CorregirPuntos();

            //Si no hay asignado ningún Punto por defecto, empezará llendo al más cercano.
            if (idWayPoints != null && idWayPoints.Count >= 2 && guardarPunto != 99)
            {
                ChangePatrolPoint();
                _travelling = true;
            }
            if (idWayPoints != null && idWayPoints.Count >= 2 && guardarPunto == 99)
            {
                _travelling = true; 
                target = FindClosestEnemy().transform;
                Debug.Log("Más Cercano");
            }

        //
        //Si el stopDistance es igual a 0 no funcionará, el seguir rutas. Por eso le asignamos uno por defecto.
        if (idWayPoints.Count >= 2 && stoppingDistance == 0)
        {
            stoppingDistance = 1;
        }
    }

    public Transform GetTarget()
    {
        return target;
    }

    GameObject FindClosestEnemy()
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in idWayPoints)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;

                guardarPunto = go.GetComponent<Waypoint>().idPoint;
            }
        }
        return closest;
    }

    void Update()
    {
        //Aquí puede dar error dependiendo del radio del agente, si es muy grande, puede que no llegue y hay que 
        //aumnetar el valor para que la distancia alcance
        if (_travelling && Vector3.Distance(target.position, transform.position) <= stoppingDistance) 
        {
            _travelling = false;
            //Se recomienda usar co-rutinas, para procesos complejos mejorando así el rendimiento.
            Invoke("SetUp",_totalWaitTime);
        }  
    }

    void SetUp()
    {
        ChangePatrolPoint();
        _travelling = true;
    }

    void ChangePatrolPoint()
    {
        if (UnityEngine.Random.Range(0, idWayPoints.Count-1) <= _swichProbability && _swichProbability != 0)
        {
            guardarPunto = idWayPoints[Random.Range(0, idWayPoints.Count - 1)].GetComponent<Waypoint>().idPoint;
            Debug.Log("Random PointPatrol: " + guardarPunto);
            ChoosePoint();
        }
        else
        {
            ChoosePoint();
        }
    }

    void ChoosePoint()
    {
        int i = 0;
        int i2 = 0;
        //Si los elementos del idWayPoints estuviesen ordenados de mayor a menor o viceversa. 
        //De esa forma, si entre los idPoint falta uno, el código funcionaría sin error, ya que, 
        //saltaría al siguiente de la lista.
        if (guardarPunto == idWayPoints[idWayPoints.Count-1].GetComponent<Waypoint>().idPoint) 
        {
            //Como esta ordenado, se sustituye el loop por un SetDestination[0]
            target = idWayPoints[0].transform;
          guardarPunto = idWayPoints[0].GetComponent<Waypoint>().idPoint;
        }
        else
        {
            foreach (GameObject go in idWayPoints)
            {
                //Tengo que saber en que posición se encuentra el último punto guardado.
                // Debido a que están ordenados, se ignora el idPoint y avanzan una posición
                if (idWayPoints[i].GetComponent<Waypoint>().idPoint == guardarPunto)
                {
                    target = idWayPoints[i/*+1*/].transform;
                    i2 = i+1;
                }              
                i++;
            }
            guardarPunto = idWayPoints[i2].GetComponent<Waypoint>().idPoint;
            i2 = 0;
        }
    }

    void OrdenarArray()
    {
        GameObject temporal;

        for (int i = 0; i < idWayPoints.Count-1; i++)
        {
            for (int j = 0; j < idWayPoints.Count - 1; j++)
            {
                if (idWayPoints[j].GetComponent<Waypoint>().idPoint > idWayPoints[j + 1].GetComponent<Waypoint>().idPoint)
                { // Ordena el array de mayor a menor, cambiar el "<" a ">" para ordenar de menor a mayor
                    temporal = idWayPoints[j];
                    idWayPoints[j] = idWayPoints[j + 1];
                    idWayPoints[j + 1] = temporal;
                }
            }
        }

        //temp
        /*foreach (GameObject go in idWayPoints)
        {
            print("Orden: " +go.GetComponent<Waypoint>().idPoint);
        }*/
    }

    void ComprobarPuntoPreEstablecido()
    {
        int i = 0;

        foreach (GameObject go in idWayPoints)
        {
            if(idWayPoints[i].GetComponent<Waypoint>().idPoint != guardarPunto)
            {
                i++;
            }           
        }
        if(i == idWayPoints.Count)
        {
            guardarPunto = 99;
            Debug.Log("El punto a buscar no existe en el contexto actual. No coincide con el IDList o idPoint");
        }
        else
        {
            Debug.Log("Punto pre-establecido: "+guardarPunto);
        }
    }
}
