using System.Collections;
using UnityEngine;

public class TitleScreenState : UIStateBase
{
    [SerializeField] private float instructionScaleDuration = 0.15f;
    public TitleScreenState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(TitleScreenShowInstruction());
    }

    IEnumerator TitleScreenShowInstruction()
    {
        #if !UNITY_EDITOR
            GameManager.Instance.PreloadNextScene();
        #endif

        manager.instruction.GetComponent<RectTransform>().localPosition = new Vector3(0, 12, 0);

        yield return ScaleObject(manager.instruction, 0.0f, 0.3f, instructionScaleDuration);
        
        manager.ChangeState(new WaitCircleBarState(manager));
    }
}
