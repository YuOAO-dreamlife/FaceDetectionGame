using UnityEngine;

public class GamingState : UIStateBase
{
    public GamingState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        if (GameManager.Instance.success || GameManager.Instance.failed)
        {
            manager.ChangeState(new UIZoomOutState(manager));
        }
    }
}
