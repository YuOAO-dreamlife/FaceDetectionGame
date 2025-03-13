using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaitCircleBarState : UIStateBase
{
    public WaitCircleBarState(UIManager manager) : base(manager) {}

    private Image _progressImage;
    private bool _isActive = false;
    private float _indicatorTimer;
    private float _maxIndicatorTimer = 3;

    public override void Enter()
    {
        _progressImage = _manager.RadialProgressBar.GetComponent<Image>();
        _manager.LandmarkInfo.OnFaceAppear += TitleScreenAnimation;
    }

    void TitleScreenAnimation(bool noFaceExist)
    {
        if (noFaceExist) 
        {
            _manager.RadialProgressBar.GetComponent<RectTransform>().localScale = Vector3.zero;
            _indicatorTimer = 0;
            _isActive = false;
            _progressImage.fillAmount = 0;
        }
        else 
        {
            _manager.RadialProgressBar.GetComponent<RectTransform>().localScale = Vector3.one * 1.5f;
            if (!_isActive)
            {
                _isActive = true;
                StartTrackedCoroutine(UpdateProgress());
            }
        }
    }

    IEnumerator UpdateProgress()
    {
        while (_isActive)
        {
            _indicatorTimer += Time.deltaTime;
            _progressImage.fillAmount = _indicatorTimer / _maxIndicatorTimer;

            if (_indicatorTimer >= _maxIndicatorTimer)
            {
                GameManager.Instance.ChangeTheScene();
                yield break;
            }

            yield return null;
        }
    }

    public override void Exit()
    {
        _manager.LandmarkInfo.OnFaceAppear -= TitleScreenAnimation;
        StopTrackedCoroutine();
    }
}
