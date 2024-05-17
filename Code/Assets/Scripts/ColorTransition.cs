using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTransition : MonoBehaviour
{
    public float transitionDuration = 2.0f; // secondes
    private Image imageComponent;
    private float timer = 0;
    private Color startColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    private Color endColor = new Color(1.0f, 0.0f, 0.0f, 0.6f);

    void Start()
    {
        imageComponent = GetComponent<Image>();
        imageComponent.color = startColor;
    }

    void Update()
    {
        if (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / transitionDuration;
            imageComponent.color = Color.Lerp(startColor, endColor, progress);
        }
    }
}