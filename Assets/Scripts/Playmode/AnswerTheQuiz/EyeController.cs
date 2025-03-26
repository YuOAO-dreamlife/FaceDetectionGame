using System;
using UnityEngine;

public class EyeController : HeadTransformController
{
    protected override void PlayerController()
    {
        if (GameManager.Instance.MissionStart && !GameManager.Instance.MissionEnd)
        {
            CheckEyeLookLeftOrNot();
            CheckEyeLookRightOrNot();
        }
    }
}
