using System.Collections;
using UnityEngine;

public class GameOverState : UIStateBase
{
    [SerializeField] private float hintScaleDuration = 0.15f;
    private AudioClip gameOverSoundtrack = Resources.Load<AudioClip>("Soundtracks/GameOver");
    public GameOverState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(ShowGameOverHint());
    }

    IEnumerator ShowGameOverHint()
    {
        GameManager.Instance.sceneLoader.PreloadTitleScreen();

        manager.audioSource.PlayOneShot(gameOverSoundtrack);

        yield return ScaleObject(manager.hintText, 0.0f, 0.15f, hintScaleDuration);
        yield return new WaitForSeconds(7);

        manager.ChangeState(new BlackUIState(manager));
    }
}
