using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CircularProgressBar : MonoBehaviour
{
    private FaceLandmarkerRunner LandmarkInfo;
    private GameManager manager;
    private bool isActive = false;
    [SerializeField] private float indicatorTimer;
    private float maxIndicatorTimer = 3;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Title_Screen") 
        {
            gameObject.SetActive(false);
        }
        LandmarkInfo = GameObject.Find("Solution").GetComponent<FaceLandmarkerRunner>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (LandmarkInfo.noFaceExist) 
        {
            indicatorTimer = 0;
            isActive = false;
        }
        else 
        {
            isActive = true;
        }
        
        if (isActive)
        {
            indicatorTimer += Time.deltaTime;
            GetComponent<Image>().fillAmount = indicatorTimer / maxIndicatorTimer;

            if (indicatorTimer >= maxIndicatorTimer)
            {
                manager.changeScene = true;
            }
        }
    }
}
