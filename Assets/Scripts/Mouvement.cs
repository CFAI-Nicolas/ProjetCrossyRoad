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
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) // 'S'
        {
            Graphique.eulerAngles = new Vector3(0, 180, 0); // Regarde vers l'arrière
            MoveBackward();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) // 'D'
        {
            Graphique.eulerAngles = new Vector3(0, 90, 0); // Regarde à droite
            MoveLateral(1);
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) // 'Q' sur un clavier AZERTY
        {
            Graphique.eulerAngles = new Vector3(0, -90, 0); // Regarde à gauche
            MoveLateral(-1);
        }

    }

    void ExitLog()
    {
        onBuche = false;
        bucheTransform = null;
        currentPosition = new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z));
        lateralPosition = Mathf.RoundToInt(currentPosition.x);
        forwardDistance = Mathf.RoundToInt(currentPosition.z);
    }


    private void OnDrawGizmos()
    {
        Ray rayo = new Ray(Graphique.position + Vector3.up * 0.5f, Graphique.forward);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Graphique.position + Vector3.up * 0.5f, Graphique.position + Vector3.up * 0.5f + Graphique.forward * distanceDeVue);
    }

    void UpdatePosition()
    {
        if (!vivant) return;  // Ne pas mettre à jour la position si le joueur est mort.

        if (onBuche && bucheTransform != null)
        {
            currentPosition = new Vector3(bucheTransform.position.x, 0, bucheTransform.position.z);
            transform.position = Vector3.Lerp(transform.position, currentPosition, speed * Time.deltaTime);
        }
        else
        {
            currentPosition = new Vector3(lateralPosition, 0, forwardDistance);
            transform.position = Vector3.Lerp(transform.position, currentPosition, speed * Time.deltaTime);
        }
    }



    void MoveForward()
    {
        if (!vivant || onBuche)
        {
            ExitLog();
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

    void MoveBackward()
    {
        if (!vivant || onBuche)
        {
            ExitLog();
        }
        Graphique.eulerAngles = new Vector3(0, 180, 0);
        if (RegardAvant())
        {
            return;
        }
        if (forwardDistance > minForwardDistance - 3)
        {
            forwardDistance--;
        }
    }

    void MoveLateral(int direction)
    {
        if (!vivant || onBuche)
        {
            ExitLog();
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
        if (!vivant) return; // Arrête le traitement si le joueur est déjà mort.

        if (other.CompareTag("voiture"))
        {
            StartCoroutine(DelayDeath());
        }
        else if (other.CompareTag("buche"))
        {
            onBuche = true;
            bucheTransform = other.transform;
            CenterOnLog(bucheTransform);
        }
    }

    void CenterOnLog(Transform logTransform)
    {
        if (!vivant) return;  // Arrête le repositionnement si le joueur est mort.

        Vector3 logCenter = logTransform.position;
        currentPosition = new Vector3(logCenter.x, transform.position.y, logCenter.z);
        transform.position = currentPosition;
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
        Vector3 rayStartPoint = transform.position + Vector3.up * 0.1f;
        Debug.DrawRay(rayStartPoint, Vector3.down * 3, Color.red);

        if (Physics.Raycast(rayStartPoint, Vector3.down, out hit, 3))
        {
            if (hit.collider.CompareTag("eau"))
            {
                StartCoroutine(DelayDeath());
            }
        }
    }

    IEnumerator DelayDeath()
    {
        // Attendre que le mouvement actuel soit terminé
        yield return new WaitForSeconds(0.1f); // Ajustez cette durée selon la durée du mouvement du personnage
        vivant = false;
    }
}
