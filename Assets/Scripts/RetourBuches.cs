using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetourBuches : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("buche"))
        {
            other.transform.Translate(0,0, -20);
        }
    }
}
