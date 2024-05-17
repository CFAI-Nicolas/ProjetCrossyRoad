using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("Score Text doit être assigné au script Distance Display !");
        }
    }

    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + ScoreManager.Instance.GetCurrentScore().ToString();
        }
    }
}
