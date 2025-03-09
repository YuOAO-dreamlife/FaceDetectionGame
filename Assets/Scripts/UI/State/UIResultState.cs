using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResultState : UIStateBase
{
    [SerializeField] private float PostBackToUIDelay = 3.5f;
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
            manager.lifeUI[currentLiveIndex].GetComponent<Image>().sprite = manager.lifeUIDead;
            ScaleObject(manager.lifeUI[currentLiveIndex], 3.0f, 1.5f, 0.2f);
            GameManager.Instance.lifeCount--;
        }
        
        if (GameManager.Instance.lifeCount > 0)
        {
            GameManager.Instance.sceneLoader.PreloadNextScene();
        }
        else
        {
            GameManager.Instance.sceneLoader.PreloadGameOver();
        }

        yield return new WaitForSeconds(PostBackToUIDelay);

        GameManager.Instance.changeScene = true;
    }
}
