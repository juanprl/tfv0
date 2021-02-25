using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public Transform playerPosition;

    public GameObject panel;

    //Todo este parrafo se tiene que sustituir por una llamada desde el Npc a este u otro scrip, donde 
    [Tooltip("Te dice si sigues o no a un jugador")]
    public Text t_followPlayer;
    public int followPlayer = 2;

    //
    int i = 0, contVictoria = 0;
    public Text t_enemycont;

    float time;
    public Text t_time;
    bool act;
    /*

    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        time = Time.time;

        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (followPlayer % 2 == 0)
            {
                t_followPlayer.text = "Npc te siguen"; //"Tienes X seguidores en reddit"
            }else{
                t_followPlayer.text = "";
            }
            followPlayer++;
        }

        if (contVictoria == 1 && !act)
        {
            Debug.Log("Has ganado");
            panel.SetActive(true);
            Time.timeScale = 0.2f;
            Invoke("LoadSaveScore", 1.1f);
            act = true;
            //Text ("Tiempo: "+"/nZonas Secretas desbloqueadas " + z1 + "/2")
            //Enemigos derrotados, cuando se pulse el botón del final de nivel que se guarden cuantos enemigos se han derrotado.
        }

        //if (contVictoria == 2) Debug.Log("Has perdido"); Realmente no hace falta porque se puede poner en HEalth
        Search();
    }

    //Se podría mejorar el código si en vez de estar en Update, solo se activase cada vez que muere algún personaje.
    void Search()
    {
        i = 0;
        GameObject[] buscarGO;
        buscarGO = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject go in buscarGO)
        {
            i++;
        }

        t_enemycont.text = ("Destroy " + i + " enemies to win");

        if (i == 0) contVictoria = 1;

    }

    void LoadSaveScore()
    {
        //Guardar puntuaje
        SceneManager.LoadScene("Score");
    }*/
}
