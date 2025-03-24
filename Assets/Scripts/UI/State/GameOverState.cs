using System.Collections;
using UnityEngine;

public class GameOverState : UIStateBase
{
    public GameOverState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(ShowGameOverHint());
    }

    IEnumerator ShowGameOverHint()
    {
        GameManager.Instance.SceneLoader.PreloadTitleScreen();

        _manager.AudioSource.PlayOneShot(_gameOverSoundtrack);

        yield return TransformUtil.ScaleObject(_manager.HintText, 0.0f, 0.15f, _hintScaleDuration);
        yield return new WaitForSeconds(7);

        _manager.ChangeState(new BlackUIState(_manager));
    }
}
