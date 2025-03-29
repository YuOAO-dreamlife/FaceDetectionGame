using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : HeadTransformController
{
    [SerializeField] private SkinnedMeshRenderer meshRenderer;

    protected override void PlayerController()
    {
        RotateHead();
        ChangeFaceEmotions(meshRenderer);
    }
}
