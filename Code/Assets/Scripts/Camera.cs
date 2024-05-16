using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerMovement playerMovement; // Assurez-vous que ceci est bien attaché dans l'inspecteur Unity
    public float speed = 0.90f;
    public Vector3 offset; // Ajoutez une variable offset que vous pouvez ajuster dans l'inspecteur Unity

    void LateUpdate()
    {
        // Assurez-vous que playerMovement n'est pas null et que le joueur est vivant
        if (playerMovement != null && playerMovement.vivant)
        {
            // Obtient la position cible de la caméra
            Vector3 targetPosition = Vector3.zero;

            // Vérifie si le joueur est sur une bûche
            if (playerMovement.currentLog != null)
            {
                // Si le joueur est sur une bûche, la caméra suit la position de la bûche
                targetPosition = playerMovement.currentLog.position;
            }
            else
            {
                // Si le joueur n'est pas sur une bûche, la caméra suit la position du joueur
                targetPosition = playerMovement.currentPosition;
            }

            // Ajoute l'offset à la position cible de la caméra
            targetPosition += offset;

            // Interpole doucement la position de la caméra vers la position cible
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
        }
    }
}
