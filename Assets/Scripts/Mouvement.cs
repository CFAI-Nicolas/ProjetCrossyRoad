// Mouvement.cs
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
    public bool vivant = true; // passé en privé
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

        if (currentLog != null)
        {
            currentPosition = new Vector3(currentLog.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            currentPosition = new Vector3(lateralPosition, 0, forwardDistance);
        }

        transform.position = Vector3.Lerp(transform.position, currentPosition, speed * Time.deltaTime);
    }

    public IEnumerator ChangePosition()
    {
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
    /*
    public void MoveForward()
    {
        Graphique.eulerAngles = new Vector3(0, 0, 0);
        if (RegardAvant())
        {
            return;
        }
        forwardDistance++;
        ScoreManager.Instance.IncrementScore(); // Incrémente le score

        if (forwardDistance > maxForwardDistance)
        {
            maxForwardDistance = forwardDistance;
            monde.CreateGrille();
        }

        RaycastHit logHit;
        if (Physics.Raycast(transform.position, Vector3.down, out logHit, 1f, CoucheEau))
        {
            if (logHit.collider.CompareTag("buche"))
            {
                MoveUpOnLog(logHit.collider.gameObject);
                return;
            }
        }

        StartCoroutine(ChangePosition());
    }
    */
    public void MoveForward()
    {
        Graphique.eulerAngles = new Vector3(0, 0, 0);

        if (RegardAvant())
        {
            Debug.Log("Obstacle détecté, mouvement interrompu.");
            return;
        }

        forwardDistance++;
        ScoreManager.Instance.IncrementScore(); 
        Debug.Log("Score incrémenté : " + ScoreManager.Instance.GetCurrentScore());

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

        Debug.Log("Déplacement normal.");
        StartCoroutine(ChangePosition());
    }



    public void MoveUpOnLog(GameObject log)
    {
        if (log != null)
        {
            currentLog = log.transform;

            if (!IsMovingWithLog())
            {
                Vector3 currentPlayerPosition = transform.position;
                transform.position = new Vector3(currentLog.position.x, currentPlayerPosition.y, currentPlayerPosition.z);
                lateralPosition = (int)currentLog.position.x;
                transform.SetParent(currentLog);
                StartCoroutine(MoveWithLog());
            }
        }
    }

    private bool IsMovingWithLog()
    {
        return currentLog != null && Mathf.Approximately(speed, 0f);
    }

    private IEnumerator MoveWithLog()
    {
        if (currentLog == null)
        {
            Debug.LogError("La bûche de référence est null.");
            yield break;
        }

        while (currentLog != null)
        {
            float xDifference = currentLog.position.x - transform.position.x;
            transform.position += new Vector3(xDifference, 0, 0);
            yield return null;
        }
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
            transform.SetParent(null);
            currentLog = null;
        }
    }

    public AudioClip deadwater;
    public void Noyer()
    {
        RaycastHit hit;
        Vector3 rayStartPoint = transform.position + Vector3.up * 0.1f;

        Debug.DrawRay(rayStartPoint, Vector3.down * 3, Color.red);

        if (Physics.Raycast(rayStartPoint, Vector3.down, out hit, 3))
        {
            if (hit.collider.CompareTag("eau"))
            {
                if (hit.collider.CompareTag("buche"))
                {
                    return;
                }
                animations.SetTrigger("Noyer");
                vivant = false;
                AudioSource audioSource = GetComponent<AudioSource>();
                audioSource.PlayOneShot(deadwater);
            }
        }
    }

    void CheckBoundsAndKill()
    {
        int positionX = currentLog != null ? Mathf.RoundToInt(currentLog.position.x) : lateralPosition;

        if (positionX < -5 || positionX > 5)
        {
            animations.SetTrigger("Ecraser");
            vivant = false;
        }
    }

    public bool IsAlive
    {
        get { return vivant; }
    }

    // Ajout de la méthode GetForwardDistance
    public int GetForwardDistance()
    {
        return forwardDistance;
    }
}
