using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnClick : MonoBehaviour
{
    // Le nom de la sc�ne vers laquelle changer
    [SerializeField] private string Demo1;

    void Update()
    {
        // V�rifie si un clic de souris est d�tect�
        if (Input.GetMouseButtonDown(0))
        {
            // Charge la sc�ne sp�cifi�e
            SceneManager.LoadScene("Menu");
        }
    }
}

