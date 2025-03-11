using System.Collections;
using UnityEngine;

public class UIZoomInState : UIStateBase
{
    public UIZoomInState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(UIZoomIn());
    }

    IEnumerator UIZoomIn()
    {
        yield return ScaleObject(_manager.gameObject, 1.0f, 4.0f, _uIScaleDuration);

        yield return FadeObject(_manager.gameObject, 1.0f, 0.0f, _uITransparentDuration);

        GameManager.Instance.gameStart = true;

        _manager.ChangeState(new GamingState(_manager));
    }
}
