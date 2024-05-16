using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class LevelManager : MonoBehaviour
{
    public Transform mainMenu, optionMenu, characterMenu, touchesMenu;
    public Transform viewMenu, musicMenu;
    public Transform musicOn, musicOff;
    public InputField userInputField;

    public AudioSource audio;

    public string username;

    // Ajoutez ces variables pour stocker les touches de déplacement
    public KeyCode moveUp = KeyCode.Z;
    public KeyCode moveDown = KeyCode.S;
    public KeyCode moveLeft = KeyCode.Q;
    public KeyCode moveRight = KeyCode.D;

    public void NameClarify()
    {
        //userInputField.text = "Enter PlayerName Here...";
        username = userInputField.text.ToString();
        PlayerPrefs.SetString("Name", username);
        //print(username);
        print(PlayerPrefs.GetString("Name"));
    }

    public void ChangeScene(string SampleScene)
    {
        SceneManager.LoadScene(SampleScene);
    }

    public void QuitGame()
    {
        UnityEngine.Application.Quit();
    }


    public void OptionMenu(bool clicked)
    {
        if (clicked == true)
        {
            optionMenu.gameObject.SetActive(clicked);
            mainMenu.gameObject.SetActive(false);
        }
        else
        {
            optionMenu.gameObject.SetActive(clicked);
            mainMenu.gameObject.SetActive(true);
        }
    }

    public void character(bool clicked)
    {
        if (clicked == true)
        {
            optionMenu.gameObject.SetActive(clicked);
            characterMenu.gameObject.SetActive(false);
        }
        else
        {
            optionMenu.gameObject.SetActive(clicked);
            characterMenu.gameObject.SetActive(true);
        }
    }

    public void TouchesMenu(bool clicked)
    {
        if (clicked == true)
        {           
            optionMenu.gameObject.SetActive(clicked);
            touchesMenu.gameObject.SetActive(false);
        }
        else
        {
            optionMenu.gameObject.SetActive(clicked);
            touchesMenu.gameObject.SetActive(true);
            
        }
    }

    public void MusicMenu(bool clicked)
    {
        if (clicked == true)
        {
            musicMenu.gameObject.SetActive(clicked);
            optionMenu.gameObject.SetActive(false);
        }
        else
        {
            musicMenu.gameObject.SetActive(clicked);
            optionMenu.gameObject.SetActive(true);
        }
    }

    public void MusicOn(bool clicked)
    {
        //if (clicked == true) {
        audio.Play();
        //}
    }

    public void MusicOff(bool clicked)
    {
        //if (clicked == true) {
        audio.Pause();
        //}
    }

    void Update()
    {
        // Utilisez les variables de touche pour les déplacements
        if (Input.GetKey(moveUp))
        {
            // Déplacer vers le haut
        }
        if (Input.GetKey(moveDown))
        {
            // Déplacer vers le bas
        }
        if (Input.GetKey(moveLeft))
        {
            // Déplacer vers la gauche
        }
        if (Input.GetKey(moveRight))
        {
            // Déplacer vers la droite
        }
    }

    // Ajoutez une fonction pour modifier les touches de déplacement
    public void SetMoveKeys(KeyCode up, KeyCode down, KeyCode left, KeyCode right)
    {
        moveUp = up;
        moveDown = down;
        moveLeft = left;
        moveRight = right;
    }
}
