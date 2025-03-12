using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResultState : UIStateBase
{
    public UIResultState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(UIResult());
    }

    IEnumerator UIResult()
    {
        if (GameManager.Instance.MissionFailure)
        {
            int currentLiveIndex = 4 - GameManager.Instance.LifeCount;
            _manager.LifeUI[currentLiveIndex].GetComponent<Image>().sprite = _manager.LifeUIDead;
            yield return ScaleObject(_manager.LifeUI[currentLiveIndex], 3.0f, 1.5f, 0.2f);
            GameManager.Instance.LoseALife();
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
