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
    public Animator animations;
    public AnimationCurve courbe;

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
    }

    public IEnumerator ChangePosition()
    {
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
        //animations.SetTrigger("Jump");
        if (forwardDistance > minForwardDistance)
        {
            minForwardDistance = forwardDistance;
            monde.CreateGrille();
        }
        StartCoroutine(ChangePosition());
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
            //animations.SetTrigger("Jump");
        }
        StartCoroutine(ChangePosition());

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
        //animations.SetTrigger("Jump");
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("voiture"))
        {
            animations.SetTrigger("Ecraser");
            vivant = false;
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
                animations.SetTrigger("Noyer");
                // Définit vivant sur false pour indiquer que le joueur est noyé
                vivant = false;
            }
        }
    }
}
