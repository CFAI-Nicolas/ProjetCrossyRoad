using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuController : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject gameOverMenu;
    void Start()
    {
        gameOverMenu.SetActive(false);
    }

    void Update()
    {
        // Vérifie si le menu de fin de partie doit être affiché
        CheckPlayerStatus();
    }

    void CheckPlayerStatus()
    {
        // Vérifie si le joueur n'est plus vivant et si le menu de fin de partie n'est pas déjà affiché
        if (!playerMovement.IsAlive && !gameOverMenu.activeSelf)
        {
            ShowGameOverMenu();
        }
    }

    public void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        // SceneManager.LoadScene("MainMenu"); // à activer après le merge avec la branche de Paul
    }

    public void QuitGame()
    {
        Debug.Log("Fermeture du jeu"); // vérifier après compilation
        Application.Quit();
    }
}
