using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Essential elements")]
    public AudioSource audioSource;
    public FaceLandmarkerRunner LandmarkInfo;

    [Header("UI Status")]
    public UIStateBase currentState;
    [SerializeField] private UIData currentData;

    [Header("UI Elements")]
    public GameObject instruction;
    public GameObject hintText;
    public GameObject levelText;


    public GameObject radialProgressBar;
    public GameObject blackUI;
    public GameObject[] lifeUI;
    public Sprite lifeUIDead;

    void Start()
    {
        LandmarkInfo = GameObject.Find("Solution").GetComponent<FaceLandmarkerRunner>();
        currentData = Resources.Load<UIData>("Database/UIData/" + SceneManager.GetActiveScene().name);

        for (int index = 0; index < GameManager.Instance.originalLifeCount - GameManager.Instance.lifeCount; index++)
        {
            lifeUI[index].GetComponent<Image>().sprite = lifeUIDead;
        }

        InitUIElements();
        SettingUIState();
    }

    void InitUIElements()
    {
        instruction.GetComponent<Image>().sprite = currentData.InstructionImage;
        instruction.GetComponentInChildren<TMP_Text>().text = currentData.InstructionText;
        hintText.GetComponent<TMP_Text>().text = currentData.HintText;
        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScreen":
                levelText.GetComponent<TMP_Text>().text = "Face the camera";
                break;

            case "GameOver":
                levelText.GetComponent<TMP_Text>().text = "Result\nLevel " + GameManager.Instance.countOfScenesHasLoaded;
                break;

            default:
                levelText.GetComponent<TMP_Text>().text = "Level " + GameManager.Instance.countOfScenesHasLoaded;
                break;
        }
    }

    public void ChangeState(UIStateBase newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    void SettingUIState()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScreen":
                ChangeState(new TitleScreenState(this));
                break;

            case "GameOver":
                ChangeState(new GameOverState(this));
                break;

            default:
                ChangeState(new InstructionState(this));
                break;
        }
    }
}
