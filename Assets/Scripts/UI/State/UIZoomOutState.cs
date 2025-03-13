using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIZoomOutState : UIStateBase
{
    public UIZoomOutState(UIManager manager) : base(manager) {}

    private Sprite[] _defaultEmotions;
    private int _defaultRandom;

    public override void Enter()
    {
        _defaultEmotions = Resources.LoadAll<Sprite>("Sprites/UI/Emotions/Default");
        _defaultRandom = Random.Range(0, _defaultEmotions.Length);
        StartTrackedCoroutine(UIZoomOut());
    }

    IEnumerator UIZoomOut()
    {
        GameManager.Instance.EndTheMission();

        _manager.ScreenEmotion.SetActive(true);
        _manager.ScreenEmotion.GetComponent<Image>().sprite = _defaultEmotions[_defaultRandom];

        _preBackToUIDelay = GameManager.Instance.PreBackToUIDelay;

        yield return new WaitForSeconds(_preBackToUIDelay);

        if (GameManager.Instance.MissionFailure) 
        {
            _manager.AudioSource.PlayOneShot(_failedSoundtrack);
        }
        else 
        {
            _manager.AudioSource.PlayOneShot(_successSoundtrack);
        }

        yield return FadeObject(_manager.gameObject, 0.0f, 1.0f, _uITransparentDuration);

        yield return ScaleObject(_manager.gameObject, 4.0f, 1.0f, _uIScaleDuration);

        _manager.ChangeState(new UIResultState(_manager));
    }
}
