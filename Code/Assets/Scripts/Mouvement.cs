using System.Collections;
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

    public void MoveUpOnLog(GameObject log)
    {
        if (log != null)
        {
            currentLog = log.transform;
            // Centrer le joueur sur la bûche
            Vector3 logCenter = new Vector3(currentLog.position.x, transform.position.y, currentLog.position.z);
            transform.position = logCenter;
            transform.SetParent(currentLog);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!vivant)
        {
            return;
        }

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

    public void Noyer()
    {
        if (!vivant)
        {
            return;
        }

        if (currentLog == null)
        {
            RaycastHit hit;
            Vector3 rayStartPoint = transform.position + Vector3.up * 0.1f;
            Debug.DrawRay(rayStartPoint, Vector3.down * 3, Color.red);

            if (Physics.Raycast(rayStartPoint, Vector3.down, out hit, 3))
            {
                if (hit.collider.CompareTag("eau"))
                {
                    animations.SetTrigger("Noyer");
                    vivant = false;
                }
            }
        }
    }

    void CheckBoundsAndKill()
    {
        if (!vivant)
        {
            return;
        }

        int positionX = currentLog != null ? Mathf.RoundToInt(currentLog.position.x) : lateralPosition;

        if (positionX < -5 || positionX > 5)
        {
            animations.SetTrigger("Ecraser");
            vivant = false;
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
            forwardDistance -= 1;
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

    public int GetMaxForwardDistance()
    {
        return maxForwardDistance;
    }
}
