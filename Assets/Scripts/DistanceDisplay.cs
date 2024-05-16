// DistanceDisplay.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceDisplay : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public TextMeshProUGUI distanceText;

    void Update()
    {
        if (playerMovement != null && distanceText != null)
        {
            distanceText.text = "Distance parcourue : " + playerMovement.GetForwardDistance().ToString();
        }
    }
}
