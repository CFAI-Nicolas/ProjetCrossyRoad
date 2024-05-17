// Coin.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int points = 10;

    private void OnTriggerEnter(Collider other)
    {
        // Vérifier si l'objet entrant a le script PlayerMovement
        if (other.GetComponent<PlayerMovement>() != null)
        {
            Debug.Log("Contact avec une pièce!");
            Debug.Log("Valeur de la pièce : " + points);
            ScoreManager.Instance.IncrementScore(points);
            Debug.Log("Pièce collectée! Valeur: " + points);
            Destroy(gameObject);
        }
    }
}
