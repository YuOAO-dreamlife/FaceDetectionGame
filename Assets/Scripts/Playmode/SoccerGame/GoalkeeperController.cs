using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalkeeperController : HeadTransformController
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _huhFace;

    protected override void PlayerController()
    {
        if (!GameManager.Instance.MissionFailure)
        {
            MoveHeadInXY();
        }
    }

    void GoalkeeperLookBack()
    {
        _renderer.material = _huhFace;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 160, 0), 0.5f * Time.deltaTime);
    }
}
