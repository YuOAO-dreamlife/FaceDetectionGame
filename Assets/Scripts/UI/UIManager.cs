using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Essential elements")]
    public AudioSource AudioSource;
    public FaceLandmarkerRunner LandmarkInfo;

    [Header("UI Status")]
    public UIStateBase CurrentState;
    [SerializeField] private UIData _currentData;

    [Header("UI Elements")]
    public GameObject Instruction;
    public GameObject HintText;
    public GameObject LevelText;
    [SerializeField] private GameObject _leftTimeText;
    public GameObject RadialProgressBar;
    public GameObject ScreenEmotion;
    public GameObject BlackUI;
    public GameObject[] LifeUI;
    public Sprite LifeUIDead;
    public GameObject button;

    void Start()
    {
        LandmarkInfo = GameObject.Find("Solution").GetComponent<FaceLandmarkerRunner>();
        _currentData = Resources.Load<UIData>("Database/UIData/" + SceneManager.GetActiveScene().name);

        for (int index = 0; index < LifeUI.Length - GameManager.Instance.LifeCount; index++)
        {
            LifeUI[index].GetComponent<Image>().sprite = LifeUIDead;
        }

        InitUIElements();
        SettingUIState();
    }

    void LeftTimeChange(int currentTime)
    {
        if (currentTime <= 3)
        {
            _leftTimeText.SetActive(true);
            _leftTimeText.GetComponent<TMP_Text>().text = "The left time of the mission... " + currentTime.ToString() + "...";
        }
    }

    void InitUIElements()
    {
        Instruction.GetComponent<Image>().sprite = _currentData.InstructionImage;
        Instruction.GetComponentInChildren<TMP_Text>().text = _currentData.InstructionText;
        HintText.GetComponent<TMP_Text>().text = _currentData.HintText;
        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScreen":
                LevelText.GetComponent<TMP_Text>().text = "Face the camera";
                break;

            case "GameOver":
                LevelText.GetComponent<TMP_Text>().text = "Result\nLevel " + GameManager.Instance.PassedMissionCount;
                break;

            case "Credits":
                LevelText.GetComponent<TMP_Text>().text = "Read the contents";
                break;

            default:
                LevelText.GetComponent<TMP_Text>().text = "Level " + GameManager.Instance.PassedMissionCount;
                break;
        }
    }

    public void OnClickCreditsOrBackButton()
    {
        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            GameManager.Instance.PressTheCreditsButton();
        }
        GameManager.Instance.ChangeTheScene();
    }

    public void ChangeState(UIStateBase newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
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

            case "Credits":
                ChangeState(new CreditsState(this));
                break;

            default:
                ChangeState(new InstructionState(this));
                break;
        }
    }

    void OnEnable()
    {
        GameManager.Instance.OnCurrentTime += LeftTimeChange;
    }

    void OnDisable()
    {
        GameManager.Instance.OnCurrentTime -= LeftTimeChange;
    }
}
