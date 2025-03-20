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
    [SerializeField] private float _moveSmoothTime = 0.1f;
    [SerializeField] private float _rotationSmoothSpeed = 30f;

    void Start()
    {
        CameraToUIOffset = (int)Mathf.Abs(transform.position.z - _mainCamera.transform.position.z);
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

    protected bool eyeBlink()
    {
        return _landmarkInfo.eyeBlink;
    }

    protected bool eyeLookInLeft()
    {
        return _landmarkInfo.eyeLookInLeft;
    }

    protected bool eyeLookInRight()
    {
        return _landmarkInfo.eyeLookInRight;
    }
}
