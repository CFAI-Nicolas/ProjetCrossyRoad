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
    public float distanceDeVue = 1;
    public bool vivant = true;

    void Start()
    {
        forwardDistance = 0;
        lateralPosition = 0;
        speed = 18;
        InvokeRepeating("Noyer",1,0.5f);
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
    }

    private void OnDrawGizmos()
    {
        Ray rayo = new Ray(Graphique.position + Vector3.up * 0.5f, Graphique.forward); // Correction de l'instanciation du rayon
        Gizmos.color = Color.green;
        Gizmos.DrawLine(Graphique.position + Vector3.up * 0.5f, Graphique.position + Vector3.up * 0.5f + Graphique.forward * distanceDeVue);
    }

    void UpdatePosition()
    {
        if (!vivant)
        {
            return;
        }
        currentPosition = new Vector3(lateralPosition, 0, forwardDistance);
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
    }

    public void Noyer()
    {
        RaycastHit hit;
        Ray rayo = new Ray(transform.position + Vector3.up, Vector3.down);

        if (Physics.Raycast(rayo, out hit, 3, CoucheEau))
        {
            if (hit.collider.CompareTag("eau"))
            {
                vivant = false;
            }
        }
    }

}