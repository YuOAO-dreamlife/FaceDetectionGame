using System.Collections;
using UnityEngine;

public class UIZoomOutState : UIStateBase
{
    public UIZoomOutState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(UIZoomOut());
    }

    IEnumerator UIZoomOut()
    {
        GameManager.Instance.gameEnd = true;

        if (
            (GameManager.Instance.countDownType.tag == "KeepAliveInTime" && GameManager.Instance.failed) 
            || (GameManager.Instance.countDownType.tag == "TimeLimitMission" && GameManager.Instance.success)
        )
        {
            _preBackToUIDelay = 2.0f;
        }
        else
        {
            _preBackToUIDelay = 0;
        }

        yield return new WaitForSeconds(_preBackToUIDelay);

        if (GameManager.Instance.failed) 
        {
            _manager.AudioSource.PlayOneShot(_failedSoundtrack);
        }
        else 
        {
            _manager.AudioSource.PlayOneShot(_successSoundtrack);
        }

        yield return FadeObject(_manager.gameObject, 0.0f, 1.0f, _uITransparentDuration);

        yield return ScaleObject(_manager.gameObject, 4.0f, 1.0f, _uIScaleDuration);

        _manager.ChangeState(new UIResultState(_manager));
    }
}
