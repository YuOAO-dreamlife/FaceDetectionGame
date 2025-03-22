using UnityEngine;

public class GhostController : HeadTransformController
{
    [SerializeField] private float _failedAnimationDuration = 1;
    [SerializeField] private float _faceCameraRotationY = 180;

    protected override void PlayerController()
    {
        if (!GameManager.Instance.MissionFailure)
        {
            MoveHeadInXY();
            RotateHead();
        }
    }

    void GhostFailedAction()
    {
        Quaternion faceCameraRotation = Quaternion.Euler(0, _faceCameraRotationY, 0);
        StartCoroutine(TransformUtil.RotateToQuat(transform, transform.rotation, faceCameraRotation, _failedAnimationDuration));
    }

    void OnEnable()
    {
        GameManager.Instance.OnMissionFailure += GhostFailedAction;
    }

    void OnDisable()
    {
        GameManager.Instance.OnMissionFailure -= GhostFailedAction;
    }
}
