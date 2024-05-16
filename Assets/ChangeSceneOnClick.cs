using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnClick : MonoBehaviour
{
    // Le nom de la scène vers laquelle changer
    [SerializeField] private string Demo1;

    void Update()
    {
        // Vérifie si un clic de souris est détecté
        if (Input.GetMouseButtonDown(0))
        {
            // Charge la scène spécifiée
            SceneManager.LoadScene("Menu");
        }
    }
}

