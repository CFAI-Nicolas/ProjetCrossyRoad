using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static System.Net.Mime.MediaTypeNames;

public class GameOverMenuController : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject gameOverMenu;
    private AudioSource audioSource;

    void Start()
    {
        gameOverMenu.SetActive(false);
        audioSource = GetComponent<AudioSource>();
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
        audioSource.Stop();
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("SampleScene");
    }

    public void LoadMainMenu()
    {
        audioSource.Stop();
        // SceneManager.LoadScene("MainMenu"); // à activer après le merge avec la branche de Paul
        SceneManager.LoadScene("Demo1"); // Utilisation correcte du nom de la scène
    }

    public void QuitGame()
    {
        audioSource.Stop();
        UnityEngine.Debug.Log("Fermeture du jeu"); // vérifier après compilation
        Application.Quit();
    }
}
