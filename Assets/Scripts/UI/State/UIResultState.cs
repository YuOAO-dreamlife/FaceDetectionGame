using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResultState : UIStateBase
{
    public UIResultState(UIManager manager) : base(manager) {}

    private Sprite[] _failedEmotions;
    private Sprite[] _successEmotions;
    private int _failedRandom;
    private int _successRandom;

    public override void Enter()
    {
        _failedEmotions = Resources.LoadAll<Sprite>("Sprites/UI/Emotions/Failed");
        _successEmotions = Resources.LoadAll<Sprite>("Sprites/UI/Emotions/Success");
        _failedRandom = Random.Range(0, _failedEmotions.Length);
        _successRandom = Random.Range(0, _successEmotions.Length);
        StartTrackedCoroutine(UIResult());
    }

    IEnumerator UIResult()
    {
        if (GameManager.Instance.MissionFailure)
        {
            _manager.ScreenEmotion.GetComponent<Image>().sprite = _failedEmotions[_failedRandom];
            int currentLiveIndex = 4 - GameManager.Instance.LifeCount;
            _manager.LifeUI[currentLiveIndex].GetComponent<Image>().sprite = _manager.LifeUIDead;
            yield return TransformUtil.ScaleObject(_manager.LifeUI[currentLiveIndex], 3.0f, 1.5f, 0.2f);
            GameManager.Instance.LoseALife();
        }
        else
        {
            _manager.ScreenEmotion.GetComponent<Image>().sprite = _successEmotions[_successRandom];
        }
        
        if (GameManager.Instance.LifeCount > 0)
        {
            GameManager.Instance.SceneLoader.PreloadNextScene();
        }
        else
        {
            GameManager.Instance.SceneLoader.PreloadGameOver();
        }

        yield return new WaitForSeconds(_postBackToUIDelay);

        GameManager.Instance.ChangeTheScene();
    }
}
