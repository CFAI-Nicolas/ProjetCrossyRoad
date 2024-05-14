using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int lateralPosition; // Position horizontale du joueur
    public int maxForwardDistance; // Distance maximale en avant que le joueur peut parcourir
    public Vector3 currentPosition; // Position actuelle du joueur
    public float speed;
    public Monde monde;
    private int forwardDistance; // Distance parcourue en avant par le joueur
    public Transform Graphique;
    public LayerMask CoucheObstacles;
    public LayerMask CoucheEau;
    public float distanceDeVue = 1;
    public bool vivant = true;
    public Animator animations;
    public AnimationCurve courbe;
    public Transform currentLog; // Champ de classe pour stocker la référence à la buche actuelle

    void Start()
    {
        forwardDistance = 0;
        lateralPosition = 0;
        speed = 300;
        InvokeRepeating("Noyer", 1, 0.5f);
    }

    void Update()
    {
        UpdatePosition();
        Noyer();
        CheckBoundsAndKill();

        if (!vivant)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveForward();
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveBackward();
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveLateral(1);
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLateral(-1);
        }
    }

    void UpdatePosition()
    {
        if (!vivant)
        {
            return;
        }
    }

    public IEnumerator ChangePosition()
    {
        // Si le joueur est sur une buche, utilisez la position X de la buche pour mettre à jour la position latérale du joueur
        if (currentLog != null)
        {
            lateralPosition = Mathf.RoundToInt(currentLog.position.x);
        }

        currentPosition = new Vector3(lateralPosition, 0, forwardDistance);
        Vector3 posActuel = transform.position;

        for (int i = 0; i < 10; i++)
        {
            transform.position = Vector3.Lerp(posActuel, currentPosition, i * 0.1f) + Vector3.up * courbe.Evaluate(i * 0.1f);
            yield return new WaitForSeconds(1f / speed);
        }
    }


    public void MoveForward()
    {
        Graphique.eulerAngles = new Vector3(0, 0, 0);
        if (RegardAvant())
        {
            return;
        }
        forwardDistance++;

        if (forwardDistance > maxForwardDistance)
        {
            maxForwardDistance = forwardDistance;
            monde.CreateGrille();
        }

        // Vérifier s'il y a une buche dans la case actuelle
        RaycastHit logHit;
        if (Physics.Raycast(transform.position, Vector3.down, out logHit, 1f, CoucheEau))
        {
            if (logHit.collider.CompareTag("buche"))
            {
                Debug.Log("Touche buche");

                MoveUpOnLog(logHit.collider.gameObject);
                return;
            }
        }

        // Si le joueur n'est pas sur une buche, continuez avec le mouvement normal
        StartCoroutine(ChangePosition());
    }

    public void MoveUpOnLog(GameObject log)
    {
        // S'assurer que le joueur est sur la buche
        if (log != null)
        {
            // Stocker la bûche actuelle pour référence
            currentLog = log.transform;
            Debug.Log("Touche buche" + currentLog);

            // Si le joueur n'est pas déjà en train de bouger avec la bûche, déplacez-le sur la bûche
            if (!IsMovingWithLog())
            {
                // Sauvegarder la position actuelle du joueur sur les axes X et Y
                Vector3 currentPlayerPosition = transform.position;

                // Déplacer le joueur sur la bûche en ajustant uniquement les axes X et Y
                transform.position = new Vector3(currentLog.position.x, currentPlayerPosition.y, currentPlayerPosition.z);

                // Mettre à jour la position latérale du joueur pour correspondre à la position horizontale de la bûche
                lateralPosition = (int)currentLog.position.x;

                // Déplacer le joueur sur la bûche
                transform.SetParent(currentLog);

                // Appeler MoveWithLog pour suivre la bûche
                StartCoroutine(MoveWithLog());
            }
        }
    }




    private bool IsMovingWithLog()
    {
        // Vérifie si le joueur est déjà en train de bouger avec la bûche
        return currentLog != null && Mathf.Approximately(speed, 0f);
    }


    private IEnumerator MoveWithLog()
    {
        // Assurez-vous que la bûche et le joueur sont correctement configurés
        if (currentLog == null)
        {
            Debug.LogError("La bûche de référence est null.");
            yield break;
        }

        Debug.Log("Début du mouvement avec la bûche.");

        // Tant que le joueur est sur la bûche, ajustez sa position relative à la bûche pour suivre son mouvement
        while (currentLog != null)
        {
            // Calculer la différence de position sur l'axe X entre la bûche et le joueur
            float xDifference = currentLog.position.x - transform.position.x;

            // Mettre à jour la position du joueur relativement à la bûche sur l'axe X
            transform.position += new Vector3(xDifference, 0, 0);

            yield return null;
        }

        Debug.Log("Fin du mouvement avec la bûche.");
    }


    public void MoveBackward()
    {
        Graphique.eulerAngles = new Vector3(0, 180, 0);
        if (RegardAvant())
        {
            return;
        }

        if (forwardDistance > maxForwardDistance - 3)
        {
            forwardDistance += -1;
        }
        StartCoroutine(ChangePosition());

    }

    public void MoveLateral(int direction)
    {
        Graphique.eulerAngles = new Vector3(0, 90 * direction, 0);
        if (RegardAvant())
        {
            return;
        }

        lateralPosition += direction;
        lateralPosition = Mathf.Clamp(lateralPosition, -5, 5);
        StartCoroutine(ChangePosition());

    }

    public bool RegardAvant()
    {
        RaycastHit hit;
        Ray rayo = new Ray(Graphique.position + Vector3.up * 0.5f, Graphique.forward);

        if (Physics.Raycast(rayo, out hit, distanceDeVue, CoucheObstacles))
        {
            return true;
        }
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("buche"))
        {
            MoveUpOnLog(other.gameObject);
        }
        else if (other.CompareTag("voiture"))
        {
            animations.SetTrigger("Ecraser");
            vivant = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("buche"))
        {
            Debug.Log("Quitte buche");
            transform.SetParent(null);
            currentLog = null;
        }
    }

    public AudioClip deadwater;
    public void Noyer()
    {
        RaycastHit hit;

        // Ajuste la hauteur de départ du rayon vers le bas pour qu'il commence légèrement au-dessus de la position actuelle du joueur et reculé
        Vector3 rayStartPoint = transform.position + Vector3.up * 0.1f;

        // Lance un rayon vers le bas à partir du point ajusté et affiche-le en rouge dans l'éditeur Unity
        Debug.DrawRay(rayStartPoint, Vector3.down * 3, Color.red);

        // Vérifie s'il y a une collision avec un objet
        if (Physics.Raycast(rayStartPoint, Vector3.down, out hit, 3))
        {
            // Si le rayon touche un objet
            if (hit.collider.CompareTag("eau"))
            {
                // Vérifie si le joueur est sur une buche
                if (hit.collider.CompareTag("buche"))
                {
                    return; // Si le joueur est sur une buche, ne pas déclencher l'animation de noyade
                }
                animations.SetTrigger("Noyer");
                // Définit vivant sur false pour indiquer que le joueur est noyé
                vivant = false;
                // Jouer le son "deadwater"
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.PlayOneShot(deadwater);
            }
        }
    }

    void CheckBoundsAndKill()
    {
        // Obtenez la position latérale de la bûche si le joueur est sur une bûche
        int positionX = currentLog != null ? Mathf.RoundToInt(currentLog.position.x) : lateralPosition;

        // Vérifie si le joueur est en dehors des limites
        if (positionX < -5 || positionX > 5)
        {
            // Le personnage est mort
            animations.SetTrigger("Ecraser");
            vivant = false;
        }
    }


}
