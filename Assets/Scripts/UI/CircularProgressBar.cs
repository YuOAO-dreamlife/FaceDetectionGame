using System.Collections;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircularProgressBar : MonoBehaviour
{
    private FaceLandmarkerRunner LandmarkInfo;
    private GameManager manager;
    private Image progressImage;
    private bool isActive = false;
    [SerializeField] private float indicatorTimer;
    private float maxIndicatorTimer = 3;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "TitleScreen") 
        {
            gameObject.SetActive(false);
        }

        LandmarkInfo = GameObject.Find("Solution").GetComponent<FaceLandmarkerRunner>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        progressImage = GetComponent<Image>();

        LandmarkInfo.OnFaceAppear += HandleFaceDetectionChanged;
    }

    void OnDestroy()
    {
        if (LandmarkInfo != null)
        {
            LandmarkInfo.OnFaceAppear -= HandleFaceDetectionChanged;
        }
    }

    void HandleFaceDetectionChanged(bool noFaceExist)
    {
        if (noFaceExist)
        {
            indicatorTimer = 0;
            isActive = false;
            progressImage.fillAmount = 0;
        }
        else
        {
            if (!isActive)
            {
                isActive = true;
                StartCoroutine(UpdateProgress());
            }
        }
    }

    IEnumerator UpdateProgress()
    {
        while (isActive)
        {
            indicatorTimer += Time.deltaTime;
            progressImage.fillAmount = indicatorTimer / maxIndicatorTimer;

            if (indicatorTimer >= maxIndicatorTimer)
            {
                manager.changeScene = true;
                yield break;
            }

            yield return null;
        }
    }
}
