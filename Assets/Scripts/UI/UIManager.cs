using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : UIAnimator
{
    public enum UIState
    {
        TitleScreen, WaitCircleBar, GameOver, BlackUI, Instruction,
        Hint, UIZoomIn, Gaming, UIZoomOut, UIResult, NULL,
    }

    [Header("UI State")]
    public UIState currentState;
    public bool startAnimation;

    [Header("Database")]
    public Database Title_Screen_Database;
    public Database Ghost_Avoid_Light_Database;
    public Database Shoot_The_Target_Database;
    public Database Game_Over_Database;

    [Header("UI Elements")]
    public Sprite currentInstructionImage;
    public string currentInstruction;
    public string currentHint;
    [SerializeField] private GameObject instruction;
    [SerializeField] private GameObject radialProgressBar;
    [SerializeField] private GameObject hintText;
    [SerializeField] private GameObject[] lifeUI;
    [SerializeField] private GameObject blackUI;
    [SerializeField] private Sprite lifeUIAlive;
    [SerializeField] private Sprite lifeUIDead;

    [Header("Animation Settings")]
    [SerializeField] private float instructionScaleDuration = 0.2f;
    [SerializeField] private float instructionDelay = 1.5f;
    [SerializeField] private float hintScaleDuration = 0.2f;
    [SerializeField] private float hintDelay = 0.5f;
    [SerializeField] private float UIScaleDuration = 0.25f;
    [SerializeField] private float UITransparentDuration = 0.25f;
    [SerializeField] public float PreBackToUIDelay;
    [SerializeField] private float PostBackToUIDelay = 3.5f;
    [SerializeField] private float BlackUITransparentDuration = 1.0f;

    [Header("UI Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip IntermissionSoundtrack;
    [SerializeField] private AudioClip successSoundtrack;
    [SerializeField] private AudioClip failedSoundtrack;
    [SerializeField] private AudioClip completeSoundtrack;
    [SerializeField] private AudioClip gameOverSoundtrack;

    private GameManager manager;
    private FaceLandmarkerRunner LandmarkInfo;

    void Start()
    {
        startAnimation = false;

        audioSource = GetComponent<AudioSource>();

        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        LandmarkInfo = GameObject.Find("Solution").GetComponent<FaceLandmarkerRunner>();

        for (int index = 0; index < manager.originalLifeCount - manager.lifeCount; index++)
        {
            lifeUI[index].GetComponent<Image>().sprite = lifeUIDead;
        }

        SettingCurrentDatabaseAndUIState();
    }

    void Update()
    {
        if (!startAnimation)
        {
            switch (currentState)
            {
                case UIState.TitleScreen:
                    StartCoroutine(TitleScreenShowInstruction());
                    break;

                case UIState.WaitCircleBar:
                    TitleScreenAnimation();
                    break;

                case UIState.GameOver:
                    StartCoroutine(ShowGameOverHint());
                    break;

                case UIState.BlackUI:
                    StartCoroutine(BlackOutScreen());
                    break;

                case UIState.Instruction:
                    StartCoroutine(ShowInstruction());
                    break;

                case UIState.Hint:
                    StartCoroutine(ShowHint());
                    break;

                case UIState.UIZoomIn:
                    StartCoroutine(UIZoomIn());
                    break;

                case UIState.Gaming:
                    if (manager.success || manager.failed)
                    {
                        currentState = UIState.UIZoomOut;
                    }
                    break;

                case UIState.UIZoomOut:
                    StartCoroutine(UIZoomOut());
                    break;

                case UIState.UIResult:
                    StartCoroutine(UIResult());
                    break;
            }
        }
    }

    void LoadDatabase(Database database)
    {
        currentInstructionImage = database.instructionImage;
        currentInstruction = database.instruction;
        currentHint = database.hint;
    }

    void SettingCurrentDatabaseAndUIState()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Title_Screen":
                LoadDatabase(Title_Screen_Database);
                currentState = UIState.TitleScreen;
                break;

            case "Ghost_Avoid_Light":
                LoadDatabase(Ghost_Avoid_Light_Database);
                currentState = UIState.Instruction;
                break;

            case "Shoot_The_Target":
                LoadDatabase(Shoot_The_Target_Database);
                currentState = UIState.Instruction;
                break;

            case "Game_Over":
                LoadDatabase(Game_Over_Database);
                currentState = UIState.GameOver;
                break;

            default:
                currentState = UIState.NULL;
                break;
        }
    }

    public void ChangeState(UIState newState)
    {
        currentState = newState;
        startAnimation = false;
    }

    IEnumerator TitleScreenShowInstruction()
    {
        startAnimation = true;

        #if !UNITY_EDITOR
            manager.PreloadNextScene();
        #endif

        instruction.GetComponent<RectTransform>().localPosition = new Vector3(0, 12, 0);

        ScaleObject(instruction, 0.0f, 0.3f, instructionScaleDuration);
        yield return new WaitForSeconds(instructionScaleDuration);

        ChangeState(UIState.WaitCircleBar);
    }

    void TitleScreenAnimation()
    {
        if (LandmarkInfo.noFaceExist) 
        {
            radialProgressBar.GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        else 
        {
            radialProgressBar.GetComponent<RectTransform>().localScale = Vector3.one * 1.5f;
        }
    }

    IEnumerator ShowGameOverHint()
    {
        startAnimation = true;

        #if !UNITY_EDITOR
            manager.PreloadTitleScreen();
        #endif

        audioSource.PlayOneShot(gameOverSoundtrack);

        ScaleObject(hintText, 0.0f, 0.15f, hintScaleDuration);
        yield return new WaitForSeconds(hintScaleDuration + gameOverSoundtrack.length - 3);

        ChangeState(UIState.BlackUI);
    }

    IEnumerator BlackOutScreen()
    {
        startAnimation = true;

        FadeObject(blackUI, 0.0f, 1.0f, BlackUITransparentDuration);
        yield return new WaitForSeconds(BlackUITransparentDuration + 1);

        manager.changeScene = true;
    }

    IEnumerator ShowInstruction()
    {
        startAnimation = true;

        audioSource.PlayOneShot(IntermissionSoundtrack);

        ScaleObject(instruction, 0.0f, 0.4f, instructionScaleDuration);
        yield return new WaitForSeconds(instructionDelay + instructionScaleDuration);

        ScaleObject(instruction, 0.4f, 0.0f, instructionScaleDuration);
        yield return new WaitForSeconds(instructionScaleDuration);

        instruction.SetActive(false);

        ChangeState(UIState.Hint);
    }

    IEnumerator ShowHint()
    {
        startAnimation = true;

        ScaleObject(hintText, 0.0f, 0.15f, hintScaleDuration);
        yield return new WaitForSeconds(hintDelay + hintScaleDuration);

        hintText.GetComponent<RectTransform>().localScale = Vector3.zero;

        ChangeState(UIState.UIZoomIn);
    }

    IEnumerator UIZoomIn()
    {
        startAnimation = true;

        ScaleObject(gameObject, 1.0f, 4.0f, UIScaleDuration);
        yield return new WaitForSeconds(UIScaleDuration);

        FadeObject(gameObject, 1.0f, 0.0f, UITransparentDuration);
        yield return new WaitForSeconds(UITransparentDuration);

        manager.gameStart = true;

        ChangeState(UIState.Gaming);
    }

    IEnumerator UIZoomOut()
    {
        startAnimation = true;

        manager.gameEnd = true;

        if (
            (manager.countDownType.tag == "KeepAliveInTime" && manager.failed) 
            || (manager.countDownType.tag == "TimeLimitMission" && manager.success)
        )
        {
            PreBackToUIDelay = 2.0f;
        }
        else
        {
            PreBackToUIDelay = 0;
        }

        yield return new WaitForSeconds(PreBackToUIDelay);

        if (manager.failed) 
        {
            audioSource.PlayOneShot(failedSoundtrack);
        }
        else 
        {
            audioSource.PlayOneShot(successSoundtrack);
        }

        FadeObject(gameObject, 0.0f, 1.0f, UITransparentDuration);
        yield return new WaitForSeconds(UITransparentDuration);

        ScaleObject(gameObject, 4.0f, 1.0f, UIScaleDuration);
        yield return new WaitForSeconds(UIScaleDuration);

        ChangeState(UIState.UIResult);
    }

    IEnumerator UIResult()
    {
        startAnimation = true;

        if (manager.failed)
        {
            lifeUI[manager.originalLifeCount - manager.lifeCount].GetComponent<Image>().sprite = lifeUIDead;
            ScaleObject(lifeUI[manager.originalLifeCount - manager.lifeCount], 3.0f, 1.5f, 0.2f);
            manager.lifeCount--;
        }
        
        #if !UNITY_EDITOR
            if (manager.lifeCount > 0)
            {
                manager.PreloadNextScene();
            }
            else
            {
                manager.PreloadGameOver();
            }
        #endif

        yield return new WaitForSeconds(PostBackToUIDelay);

        manager.changeScene = true;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}
