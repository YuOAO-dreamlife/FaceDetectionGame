using System.Collections;
using UnityEngine;

public class HintState : UIStateBase
{
    [SerializeField] private float hintScaleDuration = 0.15f;
    [SerializeField] private float hintDelay = 1.0f;
    public HintState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(ShowHint());
    }

    IEnumerator ShowHint()
    {
        yield return ScaleObject(manager.hintText, 0.0f, 0.15f, hintScaleDuration);
        yield return new WaitForSeconds(hintDelay);

        manager.hintText.GetComponent<RectTransform>().localScale = Vector3.zero;

        manager.ChangeState(new UIZoomInState(manager));
    }
}
