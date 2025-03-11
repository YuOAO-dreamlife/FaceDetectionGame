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
        if (GameManager.Instance.failed)
        {
            int currentLiveIndex = GameManager.Instance.originalLifeCount - GameManager.Instance.lifeCount;
            _manager.LifeUI[currentLiveIndex].GetComponent<Image>().sprite = _manager.LifeUIDead;
            ScaleObject(_manager.LifeUI[currentLiveIndex], 3.0f, 1.5f, 0.2f);
            GameManager.Instance.lifeCount--;
        }
        
        if (GameManager.Instance.lifeCount > 0)
        {
            GameManager.Instance.SceneLoader.PreloadNextScene();
        }
        else
        {
            GameManager.Instance.SceneLoader.PreloadGameOver();
        }

        yield return new WaitForSeconds(_postBackToUIDelay);

        GameManager.Instance.changeScene = true;
    }
}
