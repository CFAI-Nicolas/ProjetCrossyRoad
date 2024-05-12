using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerMovement playerMovement; // Assurez-vous que ceci est bien attaché dans l'inspecteur Unity
    public float speed = 0.90f;
    public Vector3 offset; // Ajoutez une variable offset que vous pouvez ajuster dans l'inspecteur Unity

    void LateUpdate()
    {
        // Assurez-vous que playerMovement n'est pas null
        if (playerMovement != null)
        {
            // Calcule la position cible de la caméra basée sur la position du joueur et le décalage
            Vector3 targetPosition = playerMovement.currentPosition + offset;
            // Interpole doucement la position de la caméra vers la position cible
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            Debug.LogError("PlayerMovement script not assigned in CameraFollow script.", this);
        }
    }
}
