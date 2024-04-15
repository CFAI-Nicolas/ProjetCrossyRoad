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

    private Rigidbody playerRigidbody; // Rigidbody du joueur
    private bool onBuche; // Indique si le joueur est sur la buche
    private Transform bucheTransform; // Transform de la buche

    void Start()
    {
        forwardDistance = 0;
        lateralPosition = 0;
        speed = 18;
        InvokeRepeating("Noyer", 1, 0.5f);
        playerRigidbody = GetComponent<Rigidbody>();
        onBuche = false;
        bucheTransform = null;
    }

    void Update()
    {
        UpdatePosition();
        Noyer();

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

        // Si le joueur est sur la buche, mettre à jour sa position pour suivre la buche
        if (onBuche && bucheTransform != null)
        {
            currentPosition = bucheTransform.position;
            transform.position = currentPosition;
        }
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

        // Si le joueur n'est pas sur la buche, déplacer normalement
        if (!onBuche)
        {
            currentPosition = new Vector3(lateralPosition, 0, forwardDistance);
            transform.position = Vector3.Lerp(transform.position, currentPosition, speed * Time.deltaTime);
        }
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
            // Le joueur entre en collision avec la buche
            onBuche = true;
            bucheTransform = other.transform;
            // Ajouter le joueur comme enfant de la buche
            transform.parent = bucheTransform;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("buche"))
        {
            // Le joueur quitte la buche
            onBuche = false;
            bucheTransform = null;
            // Détacher le joueur de la buche
            transform.parent = null;
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
}
