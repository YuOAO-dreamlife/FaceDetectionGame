using UnityEngine;

public class HeadRotateController : HeadTransformController
{
    protected override void PlayerController()
    {
        RotateHead();
        CheckHeadRotateLeftOrNot();
        CheckHeadRotateRightOrNot();
        CheckHeadRotateUpOrNot();
        CheckHeadRotateDownOrNot();
    }
}
