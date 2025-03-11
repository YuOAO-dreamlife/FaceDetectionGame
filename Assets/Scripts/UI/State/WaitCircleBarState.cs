using UnityEngine;

public class WaitCircleBarState : UIStateBase
{
    public WaitCircleBarState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        _manager.LandmarkInfo.OnFaceAppear += TitleScreenAnimation;
    }

    void TitleScreenAnimation(bool noFaceExist)
    {
        if (noFaceExist) 
        {
            _manager.RadialProgressBar.GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        else 
        {
            _manager.RadialProgressBar.GetComponent<RectTransform>().localScale = Vector3.one * 1.5f;
        }
    }

    public override void Exit()
    {
        _manager.LandmarkInfo.OnFaceAppear -= TitleScreenAnimation;
    }
}
