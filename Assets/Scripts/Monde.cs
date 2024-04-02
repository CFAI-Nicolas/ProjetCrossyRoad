using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monde : MonoBehaviour
{
    public int forwardDistance = 0; // Vous pouvez ajuster cette distance selon vos besoins
    public GameObject[] grille; // Assurez-vous que ce tableau est bien rempli dans l'éditeur Unity
    public int GrillesDevantJoueur;
    private void Start()
    {
        for (int i = 0; i < GrillesDevantJoueur; i++)
        {
            CreateGrille();
        }
    }

    public void CreateGrille()
    {
        // Instancie un GameObject aléatoire à partir du tableau grille à la position calculée
        GameObject toInstantiate = grille[Random.Range(0, grille.Length)];
        Vector3 position = new Vector3(0, 0, forwardDistance); // Utilise forwardDistance pour la position en Z
        Instantiate(toInstantiate, position, Quaternion.identity);
        forwardDistance++;
    }
}
