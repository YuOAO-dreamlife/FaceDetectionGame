using UnityEngine;

[CreateAssetMenu(fileName = "NewDatabase", menuName = "Scene/Database")]
public class Database : ScriptableObject
{
    public Sprite instructionImage;

    [TextArea]
    public string instruction;

    [TextArea]
    public string hint;

    public int gameTime;
}