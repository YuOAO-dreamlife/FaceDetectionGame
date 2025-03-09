using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI State")]
    public UIStateBase currentState;

    [Header("Database")]
    public Database TitleScreen_Database;
    public Database GhostAvoidLight_Database;
    public Database ShootTheTarget_Database;
    public Database GameOver_Database;

    [Header("UI Elements")]
    public Sprite currentInstructionImage;
    public string currentInstruction;
    public string currentHint;
    public GameObject instruction;
    public GameObject radialProgressBar;
    public GameObject hintText;
    public GameObject blackUI;
    public GameObject[] lifeUI;
    public Sprite lifeUIDead;

    [Header("UI Audio")]
    public AudioSource audioSource;
    public AudioClip IntermissionSoundtrack;
    public AudioClip successSoundtrack;
    public AudioClip failedSoundtrack;
    public AudioClip completeSoundtrack;
    public AudioClip gameOverSoundtrack;

    private GameManager manager;
    public FaceLandmarkerRunner LandmarkInfo;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        LandmarkInfo = GameObject.Find("Solution").GetComponent<FaceLandmarkerRunner>();

        for (int index = 0; index < manager.originalLifeCount - manager.lifeCount; index++)
        {
            lifeUI[index].GetComponent<Image>().sprite = lifeUIDead;
        }

        SettingCurrentDatabase();
        SettingUIState();
    }

    void LoadDatabase(Database database)
    {
        currentInstructionImage = database.instructionImage;
        currentInstruction = database.instruction;
        currentHint = database.hint;
    }

    void SettingCurrentDatabase()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScreen":
                LoadDatabase(TitleScreen_Database);
                break;

            case "GhostAvoidLight":
                LoadDatabase(GhostAvoidLight_Database);
                break;

            case "ShootTheTarget":
                LoadDatabase(ShootTheTarget_Database);
                break;

            case "GameOver":
                LoadDatabase(GameOver_Database);
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
