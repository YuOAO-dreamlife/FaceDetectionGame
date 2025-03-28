using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public ISceneLoader SceneLoader;
    [SerializeField] private BasicData _currentData;
    private UIManager _uIManager;

#region 變數 & 狀態宣告
    private int _passedMissionCount;
    public int PassedMissionCount
    {
        get { return _passedMissionCount;}
        private set
        {
            if (_passedMissionCount != value)
            {
                _passedMissionCount = value;
            }
        }
    }

    [SerializeField] private int _lifeCount;
    public int OriginalLifeCount = 4;
    public int LifeCount
    {
        get { return _lifeCount;}
        private set
        {
            if (_lifeCount != value)
            {
                _lifeCount = value;
            }
        }
    }

    private int _currentTime;
    public event Action<int> OnCurrentTime;
    public int CurrentTime
    {
        get { return _currentTime;}
        private set
        {
            if (_currentTime != value)
            {
                _currentTime = value;
                OnCurrentTime?.Invoke(_currentTime);
            }
        }
    }

    private int _preBackToUIDelay;
    public int PreBackToUIDelay
    {
        get { return _preBackToUIDelay;}
        private set
        {
            if (_preBackToUIDelay != value)
            {
                _preBackToUIDelay = value;
            }
        }
    }

    private bool _missionStart;
    public event Action OnMissionStart; // 當任務開始，呼叫HandleCountdown方法
    public bool MissionStart
    {
        get { return _missionStart; }
        private set
        {
            if (_missionStart != value)
            {
                _missionStart = value;
                if (_missionStart)
                {
                    OnMissionStart?.Invoke();
                }
            }
        }
    }

    private bool _missionSuccess;
    public event Action OnMissionSuccess; // 當任務成功，轉換到UIZoomOutState
    public bool MissionSuccess
    {
        get { return _missionSuccess; }
        private set
        {
            if (_missionSuccess != value)
            {
                _missionSuccess = value;
                if (_missionSuccess)
                {
                    OnMissionSuccess?.Invoke();
                }
            }
        }
    }

    private bool _missionFailure;
    public event Action OnMissionFailure; // 當任務失敗，轉換到UIZoomOutState
    public bool MissionFailure
    {
        get { return _missionFailure; }
        private set
        {
            if (_missionFailure != value)
            {
                _missionFailure = value;
                if (_missionFailure)
                {
                    OnMissionFailure?.Invoke();
                }
            }
        }
    }

    private bool _missionEnd;
    public event Action OnMissionEnd; // 當任務結束，設置返回UI的緩衝時間+螢幕(UI)表情改變
    public bool MissionEnd
    {
        get { return _missionEnd; }
        private set
        {
            if (_missionEnd != value)
            {
                _missionEnd = value;
                if (_missionEnd)
                {
                    OnMissionEnd?.Invoke();
                }
            }
        }
    }

    private bool _changeScene;
    public event Action OnChangeScene; // 變更場景
    public bool ChangeScene
    {
        get { return _changeScene; }
        private set
        {
            if (_changeScene != value)
            {
                _changeScene = value;
                if (_changeScene)
                {
                    OnChangeScene?.Invoke();
                }
            }
        }
    }

    private bool _creditsButtonPress;
    public bool CreditsButtonPress
    {
        get { return _creditsButtonPress; }
        private set
        {
            if (_creditsButtonPress != value)
            {
                _creditsButtonPress = value;
            }
        }
    }
#endregion

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

    void ResetGameState()
    {
        _currentData = Resources.Load<BasicData>("Database/BasicData/" + SceneManager.GetActiveScene().name);

        if (SceneManager.GetActiveScene().name == "TitleScreen")
        {
            PassedMissionCount = 0;
            _uIManager = GameObject.Find("UI Objects").GetComponent<UIManager>();
            OriginalLifeCount = _uIManager.LifeUI.Length;
            LifeCount = OriginalLifeCount;
        }
        MissionStart = false;
        MissionSuccess = false;
        MissionFailure = false;
        MissionEnd = false;
        ChangeScene = false;
        CreditsButtonPress = false;
    }

#region 直接改變狀態的方法
    void AddMissionCount()
    {
        if (
            SceneManager.GetActiveScene().name != "GameOver" 
            && SceneManager.GetActiveScene().name != "TitleScreen"
            && SceneManager.GetActiveScene().name != "Credits"
        )
        {
            PassedMissionCount++;
        }
    }

    public void SetCurrentTime(int time)
    {
        CurrentTime = time;
    }

    public void SetCurrentTimeToUIData()
    {
        CurrentTime = _currentData.GameTime;
    }

    public void LoseALife()
    {
        LifeCount--;
    }

    public void StartTheMission()
    {
        MissionStart = true;
    }

    public void MissionComplete()
    {
        MissionSuccess = true;
    }

    public void MissionFailed()
    {
        MissionFailure = true;
    }

    public void EndTheMission()
    {
        MissionEnd = true;
    }

    public void ChangeTheScene()
    {
        ChangeScene = true;
    }

    public void PressTheCreditsButton()
    {
        CreditsButtonPress = true;
    }
#endregion

#region 改變時呼叫的方法
    void HandleCountdown()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        SetCurrentTimeToUIData();
        while (CurrentTime > 0)
        {
            yield return new WaitForSeconds(1);
            CurrentTime--;
        }
        if (_currentData.CountdownType == CountdownType.KeepAliveInTime && !MissionFailure)
        {
            MissionComplete();
        }
        else if (_currentData.CountdownType == CountdownType.TimeLimitMission && !MissionSuccess)
        {
            MissionFailed();
        }
        else if(_currentData.CountdownType == CountdownType.TimeLimitButMaybeEarlyEnd && !MissionSuccess && !MissionFailure)
        {
            MissionFailed();
        }
    }

    void SetPreBackToUIDelayTime()
    {
        if (_currentData.CountdownType == CountdownType.KeepAliveInTime)
        {
            if (MissionSuccess)
            {
                PreBackToUIDelay = 0;
            }
            else if (MissionFailure)
            {
                PreBackToUIDelay = 2;
                SetCurrentTime(PreBackToUIDelay);
            }
        }
        else if (_currentData.CountdownType == CountdownType.TimeLimitMission)
        {
            if (MissionSuccess)
            {
                PreBackToUIDelay = 2;
                SetCurrentTime(PreBackToUIDelay);
            }
            else if (MissionFailure)
            {
                PreBackToUIDelay = 0;
            }
        }
        else if (_currentData.CountdownType == CountdownType.TimeLimitButMaybeEarlyEnd)
        {
            if (CurrentTime > 0)
            {
                PreBackToUIDelay = 2;
                SetCurrentTime(PreBackToUIDelay);
            }
            else
            {
                PreBackToUIDelay = 0;
            }
        }
    }

    void LoadNextScene()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScreen":
                if (CreditsButtonPress)
                {
                    SceneLoader.SwitchToCredits();
                }
                else
                {
                    SceneLoader.SwitchToNextScene();
                }
                break;

            case "GameOver":
                SceneLoader.SwitchToTitleScreen();
                break;

            case "Credits":
                SceneLoader.SwitchToTitleScreen();
                break;

            default:
                if (LifeCount == 0) 
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
#endregion
    
#region 變換場景時呼叫的方法
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResetGameState();
        AddMissionCount();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnMissionStart += HandleCountdown;
        OnMissionEnd += SetPreBackToUIDelayTime;
        OnChangeScene += LoadNextScene;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        OnMissionStart -= HandleCountdown;
        OnMissionEnd -= SetPreBackToUIDelayTime;
        OnChangeScene -= LoadNextScene;
    }
#endregion
}
