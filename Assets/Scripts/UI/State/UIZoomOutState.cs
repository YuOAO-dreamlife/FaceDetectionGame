using System.Collections;
using UnityEngine;

public class UIZoomOutState : UIStateBase
{
    [SerializeField] private float PreBackToUIDelay;
    [SerializeField] private float UIScaleDuration = 0.2f;
    [SerializeField] private float UITransparentDuration = 0.2f;
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
            PreBackToUIDelay = 2.0f;
        }
        else
        {
            PreBackToUIDelay = 0;
        }

        yield return new WaitForSeconds(PreBackToUIDelay);

        if (GameManager.Instance.failed) 
        {
            manager.audioSource.PlayOneShot(manager.failedSoundtrack);
        }
        else 
        {
            manager.audioSource.PlayOneShot(manager.successSoundtrack);
        }

        yield return FadeObject(manager.gameObject, 0.0f, 1.0f, UITransparentDuration);

        yield return ScaleObject(manager.gameObject, 4.0f, 1.0f, UIScaleDuration);

        manager.ChangeState(new UIResultState(manager));
    }
}
