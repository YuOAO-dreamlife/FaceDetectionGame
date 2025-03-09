using System.Collections;
using UnityEngine;

public class UIZoomInState : UIStateBase
{
    [SerializeField] private float UIScaleDuration = 0.2f;
    [SerializeField] private float UITransparentDuration = 0.2f;
    public UIZoomInState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(UIZoomIn());
    }

    IEnumerator UIZoomIn()
    {
        yield return ScaleObject(manager.gameObject, 1.0f, 4.0f, UIScaleDuration);

        yield return FadeObject(manager.gameObject, 1.0f, 0.0f, UITransparentDuration);

        GameManager.Instance.gameStart = true;

        manager.ChangeState(new GamingState(manager));
    }
}
