using System.Collections;
using UnityEngine;

public static class TransformUtil
{
    public static IEnumerator MoveToPos(Transform transform, Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
    }

    public static IEnumerator RotateToQuat(Transform transform, Quaternion startQuat, Quaternion endQuat, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.rotation = Quaternion.Slerp(startQuat, endQuat, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endQuat;
    }

    public static IEnumerator ScaleObject(GameObject obj, float startScale, float endScale, float duration)
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

    public static IEnumerator FadeObject(GameObject obj, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = obj.AddComponent<CanvasGroup>();
        }

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
