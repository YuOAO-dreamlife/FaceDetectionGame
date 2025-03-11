using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public ISceneLoader SceneLoader;

    [Header("System Status")]
    public bool gameStart;

    

    void Awake()
    {
        // 建立GameManager的單一Instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 根據不同的執行環境，建立對應的場景載入器
        #if UNITY_EDITOR
            SceneLoader = new EditorSceneLoader();
        #else
            SceneLoader = new RuntimeSceneLoader();
        #endif
    }





    
    public bool countDownFinish;
    public bool success;
    public bool failed;
    public bool gameEnd;
    public bool changeScene;
    public int countOfScenesHasLoaded = 0;
    public int lifeCount = 4;
    public int originalLifeCount = 4;

    public GameObject countDownType;
    private float gameTime;
    public int leftTime;
    private float PreBackToUIDelay;


    

    void Start()
    {
        lifeCount = originalLifeCount;
    }

    void Update()
    {
        if (countDownType != null)
        {
            if ((countDownType.tag == "KeepAliveInTime" && failed) || (countDownType.tag == "TimeLimitMission" && success))
            {
                PreBackToUIDelay = 2.0f;
            }
            else
            {
                PreBackToUIDelay = 0;
            }
        }
        
        if (gameStart && !countDownFinish) 
        {
            GameCountDown();
        }

        if (changeScene) 
        {
            LoadNextScene();
        }
    }

    void ResetGameState()
    {
        gameStart = false;
        countDownFinish = false;
        success = false;
        failed = false;
        gameEnd = false;
        changeScene = false;
        
        // SettingGameTime();
        leftTime = (int)gameTime;
    }

    // void SettingGameTime()
    // {
    //     switch (SceneManager.GetActiveScene().name)
    //     {
    //         case "GhostAvoidLight":
    //             gameTime = manager.GhostAvoidLight_Database.gameTime;
    //             break;

    //         case "ShootTheTarget":
    //             gameTime = manager.ShootTheTarget_Database.gameTime;
    //             break;
    //     }
    // }

    void GameCountDown()
    {
        if (countDownType.tag == "KeepAliveInTime")
        {
            if (failed)
            {
                PreBackToUIDelay -= Time.deltaTime;
                leftTime = (int)Mathf.Ceil(PreBackToUIDelay);
                if (PreBackToUIDelay < 0)
                {
                    countDownFinish = true;
                }
            }
            else
            {
                gameTime -= Time.deltaTime;
                leftTime = (int)Mathf.Ceil(gameTime);
                if (gameTime < 0)
                {
                    countDownFinish = true;
                    success = true;
                }
            }
        }
        else if (countDownType.tag == "TimeLimitMission")
        {
            if (success)
            {
                PreBackToUIDelay -= Time.deltaTime;
                leftTime = (int)Mathf.Ceil(PreBackToUIDelay);
                if (PreBackToUIDelay < 0)
                {
                    countDownFinish = true;
                }
            }
            else
            {
                gameTime -= Time.deltaTime;
                leftTime = (int)Mathf.Ceil(gameTime);
                if (gameTime < 0)
                {
                    countDownFinish = true;
                    failed = true;
                }
            }
        }        
    }

    void LoadNextScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScreen":
                SceneLoader.SwitchToNextScene();
                break;

            case "GameOver":
                SceneLoader.SwitchToTitleScreen();
                break;

            default:
                if (lifeCount == 0) 
                {
                    SceneLoader.SwitchToGameOver();
                }
                else
                {
                    SceneLoader.SwitchToNextScene();
                }
                break;
        }
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        countDownType = GameObject.Find("CountDownType");
        ResetGameState();
        if (SceneManager.GetActiveScene().name != "GameOver" && SceneManager.GetActiveScene().name != "TitleScreen")
        {
            countOfScenesHasLoaded++;
        }
    }
}
