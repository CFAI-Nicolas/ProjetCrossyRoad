using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// On utilise ce script pour calculer les pas en avant du personnage
public class DistanceDisplay : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public TextMeshProUGUI distanceText;

    void Update()
    {
        if (playerMovement != null && distanceText != null)
        {
            distanceText.text = "Distance parcourue : " + playerMovement.GetMaxForwardDistance().ToString();
        }
    }
}
