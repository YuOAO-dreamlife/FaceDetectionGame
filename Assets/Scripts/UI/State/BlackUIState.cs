using System.Collections;
using UnityEngine;

public class BlackUIState : UIStateBase
{
    public BlackUIState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(BlackOutScreen());
    }

    IEnumerator BlackOutScreen()
    {
        yield return TransformUtil.FadeObject(_manager.BlackUI, 0.0f, 1.0f, _blackUITransparentDuration);
        yield return new WaitForSeconds(1);

        GameManager.Instance.ChangeTheScene();
    }
}
