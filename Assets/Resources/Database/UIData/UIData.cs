using UnityEngine;

[CreateAssetMenu(fileName = "NewUIData", menuName = "Scene/NewUIData")]
public class UIData : ScriptableObject
{
    public Sprite InstructionImage;

    [TextArea]
    public string InstructionText;

    [TextArea]
    public string HintText;
}