using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Voiture : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,0,speed * Time.deltaTime);
    }
}
