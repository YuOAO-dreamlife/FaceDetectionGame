using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using UnityEngine;

public abstract class HeadTransformController : MonoBehaviour
{
    [Header("Necessary Components")]
    public FaceLandmarkerRunner LandmarkInfo;
    public Camera MainCamera;
    public GameManager manager;

    protected int cameraToUI_offset;
    protected int UI_width;
    protected int UI_height;
    
    private Vector3 smoothVelocity;
    private float moveSmoothTime = 0.1f;
    private float rotationSmoothTime = 0.1f;

    protected abstract void SetTheNecessaryElements();
    protected abstract void PlayerController();

    protected void MoveHeadInXY()
    {
        Vector3 leftMost_2D = new Vector3((LandmarkInfo.LeftEarLandmark.x - 0.5f) * UI_width, (1 - LandmarkInfo.LeftEarLandmark.y - 0.5f) * UI_height, cameraToUI_offset) + MainCamera.transform.position;
        Vector3 rightMost_2D = new Vector3((LandmarkInfo.RightEarLandmark.x - 0.5f) * UI_width, (1 - LandmarkInfo.RightEarLandmark.y - 0.5f) * UI_height, cameraToUI_offset) + MainCamera.transform.position;
        Vector3 currentHeadPosition = (leftMost_2D + rightMost_2D) / 2;
        transform.position = Vector3.SmoothDamp(transform.position, currentHeadPosition, ref smoothVelocity, moveSmoothTime);
    }

    protected void RotateHead()
    {
        Vector3 leftEar = new Vector3(LandmarkInfo.LeftEarLandmark.x, 1 - LandmarkInfo.LeftEarLandmark.y, LandmarkInfo.LeftEarLandmark.z);
        Vector3 rightEar = new Vector3(LandmarkInfo.RightEarLandmark.x, 1 - LandmarkInfo.RightEarLandmark.y, LandmarkInfo.RightEarLandmark.z);
        Vector3 noseTip = new Vector3(LandmarkInfo.NoseTipLandmark.x, 1 - LandmarkInfo.NoseTipLandmark.y, LandmarkInfo.NoseTipLandmark.z);
        Vector3 headPos = (leftEar + rightEar) / 2;
        Quaternion currentHeadRot = Quaternion.LookRotation(noseTip - headPos, Vector3.Cross(noseTip - rightEar, noseTip - leftEar));
        transform.rotation = Quaternion.Slerp(transform.rotation, currentHeadRot, rotationSmoothTime);
    }

    protected void HeadFaceTheCamera()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), rotationSmoothTime);
    }

    protected bool eyeBlink()
    {
        return LandmarkInfo.eyeBlink;
    }

    protected bool eyeLookInLeft()
    {
        return LandmarkInfo.eyeLookInLeft;
    }

    protected bool eyeLookInRight()
    {
        return LandmarkInfo.eyeLookInRight;
    }
}
