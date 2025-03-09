using System.Collections;
using UnityEngine;

public abstract class UIStateBase
{
    protected UIManager manager;
    private Coroutine currentCoroutine;
    public UIStateBase(UIManager manager)
    {
        this.manager = manager;
    }

    public abstract void Enter();
    public virtual void Exit()
    {
        if (currentCoroutine != null)
        {
            manager.StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    protected void StartTrackedCoroutine(IEnumerator routine)
    {
        currentCoroutine = manager.StartCoroutine(routine);
    }

    protected IEnumerator ScaleObject(GameObject obj, float startScale, float endScale, float duration)
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

    protected IEnumerator FadeObject(GameObject obj, float startAlpha, float endAlpha, float duration)
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