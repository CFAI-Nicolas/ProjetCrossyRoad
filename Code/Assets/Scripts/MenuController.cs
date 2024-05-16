using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject optionMenu;
    public GameObject mainMenu;
    public Text optionText; // Le texte pour afficher les options

    private bool optionMenuActive = false;
    public void ChangeScene(string SampleScene)
    {
        SceneManager.LoadScene(SampleScene);
    }
    public void OnApplicationQuit()
    {
        Application.Quit();
    }
    public void OptionMenu()
    {
        optionMenuActive = !optionMenuActive; // Inverser l'état du menu des options

        if (optionMenuActive)
        {
            optionText.text = "PlayerMenu\nMusicMenu\nRetour"; // Définir le texte des options
            optionMenu.SetActive(true); // Activer le menu des options
            mainMenu.SetActive(false); // Désactiver le menu principal
        }
        else
        {
            optionMenu.SetActive(false); // Désactiver le menu des options
            mainMenu.SetActive(true); // Activer le menu principal
        }
    }
}
