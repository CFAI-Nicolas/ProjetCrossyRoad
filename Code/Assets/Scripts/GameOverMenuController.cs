using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverMenuController : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject gameOverMenu;
    public GameObject inGameScoreCanvas; // Ajouter la référence au InGameScoreCanvas
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI bestScoreText;
    private AudioSource audioSource;

    void Start()
    {
        gameOverMenu.SetActive(false);
        if (inGameScoreCanvas != null)
        {
            inGameScoreCanvas.SetActive(true); // Assurez-vous que le InGameScoreCanvas est actif au début
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CheckPlayerStatus();
    }

    void CheckPlayerStatus()
    {
        if (!playerMovement.IsAlive && !gameOverMenu.activeSelf)
        {
            ShowGameOverMenu();
        }
    }

    public void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);

        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (inGameScoreCanvas != null)
        {
            inGameScoreCanvas.SetActive(false); // Désactive le InGameScoreCanvas
        }

        if (ScoreManager.Instance != null)
        {
            int currentScore = ScoreManager.Instance.GetCurrentScore();
            int bestScore = ScoreManager.Instance.GetBestScore();

            Debug.Log("Current Score: " + currentScore);
            Debug.Log("Best Score: " + bestScore);

            currentScoreText.text = "Score actuel : " + currentScore.ToString();
            bestScoreText.text = "Record : " + bestScore.ToString();

            // Sauvegarder le score actuel après l'affichage
            ScoreManager.Instance.SaveCurrentScore();
        }
        else
        {
            Debug.LogError("ScoreManager.Instance is null");
        }
    }

    public void RestartGame()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
        Debug.Log("Fermeture du jeu");
        Application.Quit();
    }
}
