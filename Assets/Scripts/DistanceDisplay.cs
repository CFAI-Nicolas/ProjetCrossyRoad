// DistanceDisplay.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText; 

    void Update()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + ScoreManager.Instance.GetCurrentScore().ToString();
        }
    }
}
