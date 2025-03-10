using UnityEngine;

[CreateAssetMenu(fileName = "NewBasicData", menuName = "Scene/NewBasicData")]
public class BasicData : ScriptableObject
{
    public int GameTime;
    public CountdownType CountdownType;
}

public enum CountdownType
{
    KeepAliveInTime,
    TimeLimitMission,
}