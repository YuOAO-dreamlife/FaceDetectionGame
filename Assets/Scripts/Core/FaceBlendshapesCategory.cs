using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using TMPro;
using UnityEngine;

public class FaceBlendshapesCategory : MonoBehaviour
{
    [SerializeField] private FaceLandmarkerRunner _landmarkInfo;

    void Update()
    {
        _landmarkInfo = GameObject.Find("Solution").GetComponent<FaceLandmarkerRunner>();
        gameObject.GetComponent<TMP_Text>().text = _landmarkInfo.categoryDetails;
    }
}
