using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetourVoitures : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("voiture"))
        {
            other.transform.Translate(0,0, -40);
        }
    }
}
