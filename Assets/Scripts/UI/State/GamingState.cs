using UnityEngine;

public class GamingState : UIStateBase
{
    public GamingState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        if (GameManager.Instance.success || GameManager.Instance.failed)
        {
            _manager.ChangeState(new UIZoomOutState(_manager));
        }
    }
}
