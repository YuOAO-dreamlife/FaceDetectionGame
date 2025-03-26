using System;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using UnityEngine;

public abstract class HeadTransformController : MonoBehaviour
{
    [Header("Necessary Components")]
    [SerializeField] private FaceLandmarkerRunner _landmarkInfo;
    [SerializeField] private Camera _mainCamera;

    private int CameraToUIOffset;
    [SerializeField] protected int UIWidth;
    [SerializeField] protected int UIHeight;
    
    private Vector3 _smoothVelocity;
    [SerializeField] private float _moveSmoothTime;
    [SerializeField] private float _rotationSmoothSpeed = 30f;

    private bool _eyeBlink;
    public event Action OnEyeBlink;
    protected bool EyeBlink
    {
        get { return _eyeBlink; }
        private set
        {
            if (_eyeBlink != value)
            {
                _eyeBlink = value;
                if (_eyeBlink)
                {
                    OnEyeBlink?.Invoke();
                }
            }
        }
    }

    private bool _eyeLookInLeft;
    public event Action OnEyeLookInLeft;
    protected bool EyeLookInLeft
    {
        get { return _eyeLookInLeft; }
        private set
        {
            if (_eyeLookInLeft != value)
            {
                _eyeLookInLeft = value;
                if (_eyeLookInLeft)
                {
                    OnEyeLookInLeft?.Invoke();
                }
            }
        }
    }

    private bool _eyeLookInRight;
    public event Action OnEyeLookInRight;
    protected bool EyeLookInRight
    {
        get { return _eyeLookInRight; }
        private set
        {
            if (_eyeLookInRight != value)
            {
                _eyeLookInRight = value;
                if (_eyeLookInRight)
                {
                    OnEyeLookInRight?.Invoke();
                }
            }
        }
    }

    private bool _headRotateLeft;
    public event Action OnHeadRotateLeft;
    protected bool HeadRotateLeft
    {
        get { return _headRotateLeft; }
        private set
        {
            if (_headRotateLeft != value)
            {
                _headRotateLeft = value;
                if (_headRotateLeft)
                {
                    OnHeadRotateLeft?.Invoke();
                }
            }
        }
    }

    void Start()
    {
        CameraToUIOffset = (int)Mathf.Abs(transform.position.z - _mainCamera.transform.position.z);
        _moveSmoothTime = 0.1f;
    }

    void Update()
    {
        PlayerController();
    }

    protected abstract void PlayerController();

    protected void MoveHeadInXY()
    {
        Vector3 leftMost_2D = new Vector3((_landmarkInfo.LeftEarLandmark.x - 0.5f) * UIWidth, (1 - _landmarkInfo.LeftEarLandmark.y - 0.5f) * UIHeight, CameraToUIOffset) + _mainCamera.transform.position;
        Vector3 rightMost_2D = new Vector3((_landmarkInfo.RightEarLandmark.x - 0.5f) * UIWidth, (1 - _landmarkInfo.RightEarLandmark.y - 0.5f) * UIHeight, CameraToUIOffset) + _mainCamera.transform.position;
        Vector3 currentHeadPos = (leftMost_2D + rightMost_2D) / 2;
        transform.position = Vector3.SmoothDamp(transform.position, currentHeadPos, ref _smoothVelocity, _moveSmoothTime);
    }

    protected void RotateHead()
    {
        Vector3 leftEar = new Vector3(_landmarkInfo.LeftEarLandmark.x, 1 - _landmarkInfo.LeftEarLandmark.y, _landmarkInfo.LeftEarLandmark.z);
        Vector3 rightEar = new Vector3(_landmarkInfo.RightEarLandmark.x, 1 - _landmarkInfo.RightEarLandmark.y, _landmarkInfo.RightEarLandmark.z);
        Vector3 noseTip = new Vector3(_landmarkInfo.NoseTipLandmark.x, 1 - _landmarkInfo.NoseTipLandmark.y, _landmarkInfo.NoseTipLandmark.z);
        Vector3 headPos = (leftEar + rightEar) / 2;
        Quaternion currentHeadRot = Quaternion.LookRotation(noseTip - headPos, Vector3.Cross(noseTip - rightEar, noseTip - leftEar));
        transform.rotation = Quaternion.Slerp(transform.rotation, currentHeadRot, _rotationSmoothSpeed * Time.deltaTime);
    }

    protected void CheckEyeBlinkOrNot()
    {
        EyeBlink = _landmarkInfo.eyeBlink;
    }

    protected void CheckEyeLookLeftOrNot()
    {
        EyeLookInLeft = _landmarkInfo.eyeLookInLeft;
    }

    protected void CheckEyeLookRightOrNot()
    {
        EyeLookInRight =  _landmarkInfo.eyeLookInRight;
    }

    // protected void CheckHeadRotateLeftOrNot()
    // {
    //     if (transform.rotation.y <)
    //     {
            
    //     }
    // }
}
