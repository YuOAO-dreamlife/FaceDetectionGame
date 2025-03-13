using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : HeadTransformController
{
    void Update()
    {
        PlayerController();
    }

    protected override void PlayerController()
    {
        if (!GameManager.Instance.MissionFailure)
        {
            MoveHeadInXY();
            RotateHead();
        }
        else
        {
            HeadFaceTheCamera();
        }
    }
}
