using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject configurationUI;

    public GameObject cameraX;//

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
                configurationUI.SetActive(false);
            }
            else
            {
                Pause();
            }
        }

        //Temp
        if (Input.GetKeyDown(KeyCode.C))
        {
           
            configurationUI.SetActive(true);     
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

        cameraX.SetActive(true);//

        CloseConfiguration();

        Cursor.visible = false;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        cameraX.SetActive(false);//

        Cursor.visible = true;
    }

    public void UI_Start()
    {
        SceneManager.LoadScene("lvl1");
    }

    public void OpenConfiguration()
    {
        configurationUI.SetActive(true);
    }

    public void CloseConfiguration()
    {
        configurationUI.SetActive(false);
    }

    public void CloseGame()
    {
        if (!Application.isEditor) 
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        } 
    }
}
