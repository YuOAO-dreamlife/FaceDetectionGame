using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("System Status")]
    public bool gameStart;
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

    private AsyncOperation asyncOperation_nextScene;
    private AsyncOperation asyncOperation_gameOver;
    private AsyncOperation asyncOperation_titleScreen;

    private UIManager manager;

    void Start()
    {
        lifeCount = originalLifeCount;
    }

    void Update()
    {
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

        countDownType = GameObject.Find("CountDownType");
        manager = GameObject.Find("UI Objects").GetComponent<UIManager>();
        
        SettingGameTime();
        PreBackToUIDelay = manager.PreBackToUIDelay;
        leftTime = (int)gameTime;
    }

    void SettingGameTime()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Ghost_Avoid_Light":
                gameTime = manager.Ghost_Avoid_Light_Database.gameTime;
                break;

            case "Shoot_The_Target":
                gameTime = manager.Shoot_The_Target_Database.gameTime;
                break;
        }
    }

#if !UNITY_EDITOR

    public void PreloadNextScene()
    {
        int index;
        if (SceneManager.GetActiveScene().buildIndex + 1 > 2)
        {
            index = 1;
        }
        else
        {
            index = SceneManager.GetActiveScene().buildIndex + 1;
        }
        asyncOperation_nextScene = SceneManager.LoadSceneAsync(index);
        asyncOperation_nextScene.allowSceneActivation = false;
    }

    void SwitchToNextScene()
    {
        if (asyncOperation_nextScene != null)
        {
            asyncOperation_nextScene.allowSceneActivation = true;
        }
    }

    public void PreloadGameOver()
    {
        asyncOperation_gameOver = SceneManager.LoadSceneAsync("Game_Over");
        asyncOperation_gameOver.allowSceneActivation = false;
    }

    void SwitchToGameOver()
    {
        if (asyncOperation_gameOver != null)
        {
            asyncOperation_gameOver.allowSceneActivation = true;
        }
    }

    public void PreloadTitleScreen()
    {
        asyncOperation_titleScreen = SceneManager.LoadSceneAsync("Title_Screen");
        asyncOperation_titleScreen.allowSceneActivation = false;
    }

    void SwitchToTitleScreen()
    {
        if (asyncOperation_titleScreen != null)
        {
            asyncOperation_titleScreen.allowSceneActivation = true;
        }
    }

#endif

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
        #if UNITY_EDITOR
            switch (SceneManager.GetActiveScene().name)
            {
                case "Title_Screen":
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    break;

                case "Game_Over":
                    SceneManager.LoadScene("Title_Screen");
                    Destroy(gameObject);
                    break;

                default:
                    if (lifeCount == 0) 
                    {
                        SceneManager.LoadScene("Game_Over");
                    }
                    else
                    {
                        if ((SceneManager.GetActiveScene().buildIndex + 1) > (SceneManager.sceneCountInBuildSettings - 2))
                        {
                            SceneManager.LoadScene(1);
                        }
                        else
                        {
                            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                        }
                    }
                    break;
            }
        #else
            switch (SceneManager.GetActiveScene().name)
            {
                case "Title_Screen":
                    SwitchToNextScene();
                    break;

                case "Game_Over":
                    SwitchToTitleScreen();
                    Destroy(gameObject);
                    break;

                default:
                    if (lifeCount == 0) 
                    {
                        SwitchToGameOver();
                    }
                    else
                    {
                        SwitchToNextScene();
                    }
                    break;
            }
        #endif
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
        ResetGameState();
        if (SceneManager.GetActiveScene().name != "Game_Over" && SceneManager.GetActiveScene().name != "Title_Screen")
        {
            countOfScenesHasLoaded++;
        }
    }
}
