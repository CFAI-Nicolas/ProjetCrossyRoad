using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class sonsvoitures : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Obtenez le composant AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    // Cette fonction active le son
    public void ActiverSon()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            UnityEngine.Debug.Log("Aucun composant AudioSource trouvé!");
        }
    }
}