using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public int idPatrulla; // Diferenciar unas rutas de otras.
    public int idPoint; //El número es para que vaya en orden, 0->1->2... 9->10

    //Lo bueno de este método, sería que nos permitiría cambiar de idPatrulla si lo indicamos. Pero con Tag, y no con GameObject.
}
