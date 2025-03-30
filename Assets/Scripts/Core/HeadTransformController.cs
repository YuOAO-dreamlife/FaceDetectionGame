using System;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using UnityEngine;

public abstract class HeadTransformController : MonoBehaviour
{
    [Header("Necessary Components")]
    [SerializeField] protected FaceLandmarkerRunner _landmarkInfo;
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

    public event Action<string> OnEyeLookIn;
    private bool _eyeLookInLeft;
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
                    OnEyeLookIn?.Invoke("Left");
                }
            }
        }
    }
    private bool _eyeLookInRight;
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
                    OnEyeLookIn?.Invoke("Right");
                }
            }
        }
    }

    [SerializeField] private float _rotateAngle;
    [SerializeField] private float _rotateJudgeAngleRange = 30;

    public event Action<string> OnHeadRotate;
    private bool _headRotateLeft;
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
                    OnHeadRotate?.Invoke("Left");
                }
            }
        }
    }
    private bool _headRotateRight;
    protected bool HeadRotateRight
    {
        get { return _headRotateRight; }
        private set
        {
            if (_headRotateRight != value)
            {
                _headRotateRight = value;
                if (_headRotateRight)
                {
                    OnHeadRotate?.Invoke("Right");
                }
            }
        }
    }
    private bool _headRotateUp;
    protected bool HeadRotateUp
    {
        get { return _headRotateUp; }
        private set
        {
            if (_headRotateUp != value)
            {
                _headRotateUp = value;
                if (_headRotateUp)
                {
                    OnHeadRotate?.Invoke("Up");
                }
            }
        }
    }
    private bool _headRotateDown;
    protected bool HeadRotateDown
    {
        get { return _headRotateDown; }
        private set
        {
            if (_headRotateDown != value)
            {
                _headRotateDown = value;
                if (_headRotateDown)
                {
                    Debug.Log("D");
                    OnHeadRotate?.Invoke("Down");
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
        Vector3 leftEar = new Vector3(_landmarkInfo.LeftEarLandmark.x, 1 - _landmarkInfo.LeftEarLandmark.y, _landmarkInfo.LeftEarLandmark.z); // Unity 跟 Mediapipe 的Y軸座標相反
        Vector3 rightEar = new Vector3(_landmarkInfo.RightEarLandmark.x, 1 - _landmarkInfo.RightEarLandmark.y, _landmarkInfo.RightEarLandmark.z);
        Vector3 noseTip = new Vector3(_landmarkInfo.NoseTipLandmark.x, 1 - _landmarkInfo.NoseTipLandmark.y, _landmarkInfo.NoseTipLandmark.z);
        Vector3 headPos = (leftEar + rightEar) / 2;

        // noseTip - headPos 表示玩家(臉部)物件向前(Z軸)的方向
        // Vector3.Cross(noseTip - rightEar, noseTip - leftEar) 是為了要獲取向上(Y軸)的方向
        Quaternion currentHeadRot = Quaternion.LookRotation(noseTip - headPos, Vector3.Cross(noseTip - rightEar, noseTip - leftEar));  
        transform.rotation = Quaternion.Slerp(transform.rotation, currentHeadRot, _rotationSmoothSpeed * Time.deltaTime);
    }

    protected void CheckEyeBlinkOrNot()
    {
        EyeBlink = _landmarkInfo.BlendshapesBool["eyeBlinkLeft"];
        EyeBlink = _landmarkInfo.BlendshapesBool["eyeBlinkRight"];
    }

    protected void CheckEyeLookLeftOrNot()
    {
        EyeLookInLeft = _landmarkInfo.BlendshapesBool["eyeLookInLeft"];
    }

    protected void CheckEyeLookRightOrNot()
    {
        EyeLookInRight =  _landmarkInfo.BlendshapesBool["eyeLookInRight"];
    }

    protected void CheckHeadRotateLeftOrNot()
    {
        float currentHeadYAngle = transform.eulerAngles.y - 180;
        HeadRotateLeft = (currentHeadYAngle > _rotateAngle && currentHeadYAngle < (_rotateAngle + _rotateJudgeAngleRange)) ? true : false;
    }

    protected void CheckHeadRotateRightOrNot()
    {
        float currentHeadYAngle = 180 - transform.eulerAngles.y;
        HeadRotateRight = (currentHeadYAngle > _rotateAngle && currentHeadYAngle < (_rotateAngle + _rotateJudgeAngleRange)) ? true : false;
    }

    protected void CheckHeadRotateUpOrNot()
    {
        float currentHeadXAngle = 360 - transform.eulerAngles.x;
        HeadRotateUp = (currentHeadXAngle > _rotateAngle && currentHeadXAngle < (_rotateAngle + _rotateJudgeAngleRange)) ? true : false;
    }

    protected void CheckHeadRotateDownOrNot()
    {
        float currentHeadXAngle = transform.eulerAngles.x - 0;
        HeadRotateDown = (currentHeadXAngle > _rotateAngle && currentHeadXAngle < (_rotateAngle + _rotateJudgeAngleRange)) ? true : false;
    }

    protected void ChangeFaceEmotions(SkinnedMeshRenderer skinnedMesh)
    {
        foreach (var item in _landmarkInfo.BlendshapesWeight)
        {
            int blendshapeIndex = skinnedMesh.sharedMesh.GetBlendShapeIndex(item.Key);
            skinnedMesh.SetBlendShapeWeight(blendshapeIndex, item.Value*100);
        }
    }
}
