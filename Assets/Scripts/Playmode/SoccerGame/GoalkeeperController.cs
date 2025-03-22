using UnityEngine;

public class GoalkeeperController : HeadTransformController
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _huhFace;
    [SerializeField] private float _failedAnimationDuration = 1.5f;
    [SerializeField] private float _lookBackRotationY = 150;

    protected override void PlayerController()
    {
        if (!GameManager.Instance.MissionFailure)
        {
            MoveHeadInXY();
        }
    }

    void GoalkeeperFailedAction()
    {
        _renderer.material = _huhFace;
        if (transform.position.x >= 80)
        {
            _lookBackRotationY = -_lookBackRotationY;
        }
        Quaternion lookBackRotation = Quaternion.Euler(0, _lookBackRotationY, 0);
        StartCoroutine(TransformUtil.RotateToQuat(transform, transform.rotation, lookBackRotation, _failedAnimationDuration));
    }

    void OnEnable()
    {
        GameManager.Instance.OnMissionFailure += GoalkeeperFailedAction;
    }

    void OnDisable()
    {
        GameManager.Instance.OnMissionFailure -= GoalkeeperFailedAction;
    }
}
