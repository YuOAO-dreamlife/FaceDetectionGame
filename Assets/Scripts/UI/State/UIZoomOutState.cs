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
        GameManager.Instance.EndTheMission();

        _preBackToUIDelay = GameManager.Instance.PreBackToUIDelay;

        yield return new WaitForSeconds(_preBackToUIDelay);

        if (GameManager.Instance.MissionFailure) 
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
