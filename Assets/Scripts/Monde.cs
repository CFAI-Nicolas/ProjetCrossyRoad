using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monde : MonoBehaviour
{
    public int forwardDistance = 0; // Vous pouvez ajuster cette distance selon vos besoins
    public GameObject[] grille; // Assurez-vous que ce tableau est bien rempli dans l'éditeur Unity
    public int GrillesDevantJoueur;

    [Header("Coin Settings")]
    public GameObject coinPrefab;
    [Range(0f, 1f)]
    public float coinSpawnProbability = 0.5f;

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

        // Génère des pièces sur la grille
        GenerateCoins(position);

        forwardDistance++;
    }

    void GenerateCoins(Vector3 grillePosition)
    {
        // Détermine si une pièce doit être générée en fonction de la probabilité définie
        if (Random.value < coinSpawnProbability)
        {
            // Détermine une position aléatoire sur la grille pour la pièce
            Vector3 coinPosition = grillePosition + new Vector3(Random.Range(-5, 6), 0.5f, Random.Range(0, 1));
            Instantiate(coinPrefab, coinPosition, Quaternion.identity);
        }
    }
}
