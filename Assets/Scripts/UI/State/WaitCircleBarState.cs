using UnityEngine;

public class WaitCircleBarState : UIStateBase
{
    public WaitCircleBarState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        manager.LandmarkInfo.OnFaceAppear += TitleScreenAnimation;
    }

    void TitleScreenAnimation(bool noFaceExist)
    {
        if (noFaceExist) 
        {
            manager.radialProgressBar.GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        else 
        {
            manager.radialProgressBar.GetComponent<RectTransform>().localScale = Vector3.one * 1.5f;
        }
    }

    public override void Exit()
    {
        manager.LandmarkInfo.OnFaceAppear -= TitleScreenAnimation;
    }
}
