using UnityEngine;

public class GamingState : UIStateBase
{
    public GamingState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        GameManager.Instance.OnMissionSuccess += ChangeStateToUIZoomOut;
        GameManager.Instance.OnMissionFailure += ChangeStateToUIZoomOut;
    }

    void ChangeStateToUIZoomOut()
    {
        _manager.ChangeState(new UIZoomOutState(_manager));
    }

    public override void Exit()
    {
        GameManager.Instance.OnMissionSuccess -= ChangeStateToUIZoomOut;
        GameManager.Instance.OnMissionFailure -= ChangeStateToUIZoomOut;
    }
}
