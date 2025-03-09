using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimator : MonoBehaviour
{
    protected void ScaleObject(GameObject gameObject, float originalScale, float targetScale, float duration)
    {
        StartCoroutine(ScaleRoutine(gameObject, originalScale, targetScale, duration));
    }

    protected void FadeObject(GameObject gameObject, float originalAlpha, float targetAlpha, float duration)
    {
        StartCoroutine(FadeRoutine(gameObject, originalAlpha, targetAlpha, duration));
    }

    IEnumerator ScaleRoutine(GameObject obj, float startScale, float endScale, float duration)
    {
        float elapsedTime = 0f;
        RectTransform rect = obj.GetComponent<RectTransform>();

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            rect.localScale = Vector3.Lerp(Vector3.one * startScale, Vector3.one * endScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rect.localScale = Vector3.one * endScale;
    }

    IEnumerator FadeRoutine(GameObject obj, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
