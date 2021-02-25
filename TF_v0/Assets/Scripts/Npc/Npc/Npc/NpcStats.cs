using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcStats : MonoBehaviour
{

    string[] arrayNames = { "XX_Sakamoto", "Kirito68", "greenlife01", "11","12", "13", "14", "15", "16", "17", "18", "19", "20", };
    public Text t_name;

    void Start()
    {
        Name();
        RandomColor();
    }

    void Update()
    {
        
    }

    //Hay que hacer que los nombres no se repitan, no puede haber varios personajes llamados iguales
    void Name()
    {
        int random = Random.Range(0, arrayNames.Length-1);
        t_name.text = arrayNames[random];
    }

    //Mientras que los Weebs si pueden tener distintos colores, los NPC de cada nivel tendrán el mismo para qe no se confundan
    void RandomColor()
    {
        GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);        
    }
}
