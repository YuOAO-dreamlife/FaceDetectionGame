using UnityEngine;

public class ResourceContextState : UIStateBase
{
    public ResourceContextState(UIManager manager) : base(manager) {}

    public override void Enter()
    {
        ShowContext();
    }

    void ShowContext()
    {
        _manager.button.SetActive(true);
        _manager.Instruction.GetComponent<RectTransform>().localScale = Vector3.one * 0.4f;
    }
}
