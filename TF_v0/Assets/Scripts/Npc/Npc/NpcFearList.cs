using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFearList /*: IComparable<NpcFearList>*/
{
    /*Quiero que cada Npc tenga un script que haga de mini base de datos donde
     * se apunte a cada otro Npc que se han encontrado y que apunte sus acciones de forma ordenada
     * , para acceder a dicha información más adelante. */
    
    public int idList;
    public string name;
    public string tag;
    public bool haSidoVisto; //Cuando es visto por primera vez
    public bool haMuerto; //Cuando ve un cadáver, hacer que haSidoVisto no se active si esta muerto
    //Atacando será sacado del script de pelea, y se pondrá un temporizador o algo para que no se repita tanto.
    //Si una personaje esta huyendo será sacado del script correscpondiente, tendrá algo para que no se repita o algo // Habrá que poner si una persona huye y eso le da los suficientes puntos de fear para fearMax, deberá huir a ese mismo punto y no al contrario.

    //Hay que sacar variaciones para cuando es un objeto y: Explota, Va a demasiada velocidad y pasa cerca de ellos,

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
