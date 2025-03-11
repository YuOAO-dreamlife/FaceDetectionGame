using System.Collections;
using UnityEngine;

public class HintState : UIStateBase
{
    public HintState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(ShowHint());
    }

    IEnumerator ShowHint()
    {
        yield return ScaleObject(_manager.HintText, 0.0f, 0.15f, _hintScaleDuration);
        yield return new WaitForSeconds(_hintDelay);

        _manager.HintText.GetComponent<RectTransform>().localScale = Vector3.zero;

        _manager.ChangeState(new UIZoomInState(_manager));
    }
}
