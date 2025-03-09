using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPulseEffect : MonoBehaviour
{
    private float minScale = 1.0f;
    private float maxScale = 1.025f;
    private float pulseSpeed = 12.0f;
    private RectTransform rectTransform;
    private float timer;
    private Vector3 originalScale;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    void Update()
    {
        timer += Time.deltaTime * pulseSpeed;
        float scale = Mathf.Lerp(minScale, maxScale, (Mathf.Sin(timer) + 1.0f) / 2.0f);
        rectTransform.localScale = originalScale * scale;
    }
}
