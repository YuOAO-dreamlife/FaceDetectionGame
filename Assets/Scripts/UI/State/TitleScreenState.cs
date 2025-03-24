using System.Collections;
using UnityEngine;

public class TitleScreenState : UIStateBase
{
    public TitleScreenState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(TitleScreenShowInstruction());
    }

    IEnumerator TitleScreenShowInstruction()
    {
        GameManager.Instance.SceneLoader.PreloadNextScene();

        _manager.Instruction.GetComponent<RectTransform>().localPosition = new Vector3(0, 12, 0);

        yield return TransformUtil.ScaleObject(_manager.Instruction, 0.0f, 0.3f, _instructionScaleDuration);
        
        _manager.ChangeState(new WaitCircleBarState(_manager));
    }
}
