using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;
    private int bestScore = 0;
    private int lastScore = 0;

    private static ScoreManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static ScoreManager Instance
    {
        get { return instance; }
    }

    public void ResetCurrentScore()
    {
        currentScore = 0;
    }

    public void IncrementScore()
    {
        currentScore++;
    }

    public void SaveCurrentScore()
    {
        // à améliorer si possible pour que le bestScore se mette à jour mieux
        //lastScore = currentScore; 
        if (currentScore > bestScore)
        {
            bestScore = currentScore; 
        }
        lastScore = currentScore;
        currentScore = 0; 
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetLastScore()
    {
        return lastScore;
    }

    public int GetBestScore()
    {
        return bestScore;
    }
}
