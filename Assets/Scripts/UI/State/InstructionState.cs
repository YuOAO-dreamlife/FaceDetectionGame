using System.Collections;
using UnityEngine;

public class InstructionState : UIStateBase
{
    [SerializeField] private float instructionScaleDuration = 0.15f;
    [SerializeField] private float instructionDelay = 1.5f;
    private AudioClip intermissionSoundtrack = Resources.Load<AudioClip>("Soundtracks/Default");
    public InstructionState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        StartTrackedCoroutine(ShowInstruction());
    }

    IEnumerator ShowInstruction()
    {
        manager.audioSource.PlayOneShot(intermissionSoundtrack);

        yield return ScaleObject(manager.instruction, 0.0f, 0.4f, instructionScaleDuration);
        yield return new WaitForSeconds(instructionDelay);

        yield return ScaleObject(manager.instruction, 0.4f, 0.0f, instructionScaleDuration);

        manager.instruction.SetActive(false);

        manager.ChangeState(new HintState(manager));
    }
}
