using System.Collections;
using UnityEngine;

public class CreditsState : UIStateBase
{
    public CreditsState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(ShowHint());
    }

    IEnumerator ShowHint()
    {
        GameManager.Instance.SceneLoader.PreloadTitleScreen();

        yield return TransformUtil.ScaleObject(_manager.HintText, 0.0f, 0.15f, _hintScaleDuration);
        yield return new WaitForSeconds(_hintDelay);

        yield return TransformUtil.ScaleObject(_manager.gameObject, 1.0f, 4.0f, _uIScaleDuration);

        _manager.HintText.GetComponent<RectTransform>().localScale = Vector3.zero;

        _manager.ChangeState(new ResourceContextState(_manager));
    }
}
