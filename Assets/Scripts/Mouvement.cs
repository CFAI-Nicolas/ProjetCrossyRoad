using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int lateralPosition; // Position horizontale du joueur
    public int minForwardDistance; // Distance maximale en avant que le joueur peut parcourir
    public Vector3 currentPosition; // Position actuelle du joueur
    public float speed;
    public Monde monde;
    private int forwardDistance; // Distance parcourue en avant par le joueur
    public Transform Graphique;
    public LayerMask CoucheObstacles;
    public LayerMask CoucheEau;
    public LayerMask CoucheBuche; // Nouvelle couche pour les rondins
    public float distanceDeVue = 1;
    public bool vivant = true;

    private bool onBuche; // Indique si le joueur est sur la buche
    private Transform bucheTransform; // Transform de la buche

    void Start()
    {
        forwardDistance = 0;
        lateralPosition = 0;
        speed = 18;
        InvokeRepeating("Noyer", 1, 0.5f);
        onBuche = false;
        bucheTransform = null;
    }

    void Update()
    {
        UpdatePosition();
        Noyer();

        if (!vivant)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) // 'Z' sur un clavier AZERTY
        {
            Graphique.eulerAngles = new Vector3(0, 0, 0); // S'assure que le personnage regarde vers l'avant
            MoveForward();
            if (onBuche) ExitLog();
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) // 'S'
        {
            Graphique.eulerAngles = new Vector3(0, 180, 0); // Regarde vers l'arrière
            MoveBackward();
            if (onBuche) ExitLog();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) // 'D'
        {
            Graphique.eulerAngles = new Vector3(0, 90, 0); // Regarde à droite
            MoveLateral(1);
            if (onBuche) ExitLog();
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) // 'Q' sur un clavier AZERTY
        {
            Graphique.eulerAngles = new Vector3(0, -90, 0); // Regarde à gauche
            MoveLateral(-1);
            if (onBuche) ExitLog();
        }
    }

    void ExitLog()
    {
        onBuche = false;
        bucheTransform = null;
        // Réinitialisez ici toute autre logique nécessaire pour gérer correctement la sortie de la bûche
    }

    private void OnDrawGizmos()
    {
        Ray rayo = new Ray(Graphique.position + Vector3.up * 0.5f, Graphique.forward);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Graphique.position + Vector3.up * 0.5f, Graphique.position + Vector3.up * 0.5f + Graphique.forward * distanceDeVue);
    }

    void UpdatePosition()
    {
        if (!vivant)
        {
            return;
        }

        if (onBuche && bucheTransform != null)
        {
            // Suit le mouvement de la bûche
            currentPosition = new Vector3(bucheTransform.position.x, 0, bucheTransform.position.z);
        }
        else
        {
            currentPosition = new Vector3(lateralPosition, 0, forwardDistance);
        }

        transform.position = Vector3.Lerp(transform.position, currentPosition, speed * Time.deltaTime);
    }


    public void MoveForward()
    {
        if (!vivant)
        {
            return;
        }
        Graphique.eulerAngles = new Vector3(0, 0, 0);
        if (RegardAvant())
        {
            return;
        }
        forwardDistance++;
        if (forwardDistance > minForwardDistance)
        {
            minForwardDistance = forwardDistance;
            monde.CreateGrille();
        }
    }

    public void MoveBackward()
    {
        if (!vivant)
        {
            return;
        }
        Graphique.eulerAngles = new Vector3(0, 180, 0);
        if (RegardAvant())
        {
            return;
        }

        if (forwardDistance > minForwardDistance - 3)
        {
            forwardDistance += -1;
        }
    }

    public void MoveLateral(int direction)
    {
        if (!vivant)
        {
            return;
        }
        Graphique.eulerAngles = new Vector3(0, 90 * direction, 0);
        if (RegardAvant())
        {
            return;
        }

        lateralPosition += direction;
        lateralPosition = Mathf.Clamp(lateralPosition, -5, 5);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("voiture"))
        {
            vivant = false;
        }

        if (other.CompareTag("buche"))
        {
            onBuche = true;
            bucheTransform = other.transform;  // Assurez-vous que le Transform est correctement assigné
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("buche"))
        {
            onBuche = false;
            bucheTransform = null;
        }
    }

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
                vivant = false;
            }
        }
    }

    // Fonction pour déplacer le joueur sur la buche
    private void MoveOnBuche(Vector3 direction)
    {
        if (!vivant)
        {
            return;
        }

        // Déplacer le joueur selon la direction donnée et la vitesse
        currentPosition += direction * speed * Time.deltaTime;

        // Mettre à jour la position du joueur
        transform.position = currentPosition;
    }
}
