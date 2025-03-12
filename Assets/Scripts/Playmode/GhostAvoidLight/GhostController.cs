using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : HeadTransformController
{
    void Start()
    {
        SetTheNecessaryElements();
    }

    void Update()
    {
        PlayerController();
    }

    protected override void SetTheNecessaryElements()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraToUI_offset = 100;
        UI_width = 200;
        UI_height = 100;
    }

    protected override void PlayerController()
    {
        if (!manager.MissionFailure)
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
