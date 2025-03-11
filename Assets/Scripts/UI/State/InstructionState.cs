using System.Collections;
using UnityEngine;

public class InstructionState : UIStateBase
{
    public InstructionState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(ShowInstruction());
    }

    IEnumerator ShowInstruction()
    {
        _manager.AudioSource.PlayOneShot(_intermissionSoundtrack);

        yield return ScaleObject(_manager.Instruction, 0.0f, 0.4f, _instructionScaleDuration);
        yield return new WaitForSeconds(_instructionDelay);

        yield return ScaleObject(_manager.Instruction, 0.4f, 0.0f, _instructionScaleDuration);

        _manager.Instruction.SetActive(false);

        _manager.ChangeState(new HintState(_manager));
    }
}
