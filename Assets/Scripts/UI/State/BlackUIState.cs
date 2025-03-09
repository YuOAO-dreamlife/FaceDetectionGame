using System.Collections;
using UnityEngine;

public class BlackUIState : UIStateBase
{
    [SerializeField] private float BlackUITransparentDuration = 2.0f;
    public BlackUIState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(BlackOutScreen());
    }

    IEnumerator BlackOutScreen()
    {
        yield return FadeObject(manager.blackUI, 0.0f, 1.0f, BlackUITransparentDuration);
        yield return new WaitForSeconds(1);

        GameManager.Instance.changeScene = true;
    }
}
