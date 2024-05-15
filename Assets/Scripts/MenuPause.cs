using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;

public class MenuPause : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject gameOverMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameOverMenu.activeSelf)
            {
                return;
            }

            if (Time.timeScale == 1)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Demo1"); // vérifier le nom avec Paul
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("Sortie du jeu"); // utilisation correcte de Debug
        Application.Quit();
    }
}
