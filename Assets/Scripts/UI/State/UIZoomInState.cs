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
        yield return TransformUtil.ScaleObject(_manager.gameObject, 1.0f, 4.0f, _uIScaleDuration);

        yield return TransformUtil.FadeObject(_manager.gameObject, 1.0f, 0.0f, _uITransparentDuration);

        GameManager.Instance.StartTheMission();

        _manager.ChangeState(new GamingState(_manager));
    }
}
