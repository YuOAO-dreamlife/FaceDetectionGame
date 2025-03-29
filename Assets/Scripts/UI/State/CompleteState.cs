using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteState : UIStateBase
{
    public CompleteState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(ShowCompleteHint());
    }

    IEnumerator ShowCompleteHint()
    {
        GameManager.Instance.SceneLoader.PreloadTitleScreen();

        _manager.AudioSource.PlayOneShot(_completeSoundtrack);

        yield return TransformUtil.ScaleObject(_manager.Instruction, 0.0f, 0.4f, _instructionScaleDuration);
        yield return new WaitForSeconds(_instructionDelay);
        yield return TransformUtil.ScaleObject(_manager.Instruction, 0.4f, 0.0f, _instructionScaleDuration);

        yield return TransformUtil.ScaleObject(_manager.HintText, 0.0f, 0.15f, _hintScaleDuration);
        yield return new WaitForSeconds(5);

        _manager.ChangeState(new BlackUIState(_manager));
    }
}
