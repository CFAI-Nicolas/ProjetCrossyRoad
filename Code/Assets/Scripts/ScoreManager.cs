using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int currentScore = 0;
    private int bestScore = 0;

    private static ScoreManager instance;

    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScoreManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("ScoreManager");
                    instance = obj.AddComponent<ScoreManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

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

    public void ResetCurrentScore()
    {
        currentScore = 0;
    }

    public void IncrementScore(int points)
    {
        Debug.Log("Appel à IncrementScore avec : " + points); // Nouveau log
        Debug.Log("Ajout de points : " + points);
        currentScore += points;
        Debug.Log("Score actuel : " + currentScore);
    }

    public void SaveCurrentScore()
    {
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
        }
        currentScore = 0;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public int GetBestScore()
    {
        return bestScore;
    }
}
