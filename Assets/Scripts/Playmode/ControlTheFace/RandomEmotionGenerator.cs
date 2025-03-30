using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomEmotionGenerator : MonoBehaviour
{
    [SerializeField] private FaceController _faceController;
    [SerializeField] private Image _currentEmotion;
    [SerializeField] private Sprite _slightlySmiling;
    [SerializeField] private Sprite _slightlyFrowning;
    [SerializeField] private Sprite _kissing;
    [SerializeField] private Sprite _grinning;
    [SerializeField] private Sprite _frowningOpenMouth;
    [SerializeField] private Sprite _flushed;
    [SerializeField] private Sprite _openMouth;
    [SerializeField] private Sprite _expressionless;
    [SerializeField] private Sprite _angry;
    [SerializeField] private Sprite _successSprite;
    [SerializeField] private GameObject _rate;
    [SerializeField] private Sprite _ok;
    [SerializeField] private Sprite _good;
    [SerializeField] private Sprite _great;
    [SerializeField] private Sprite _excellent;
    public Dictionary<string, Sprite> Emotions;
    private List<string> _keys;
    private string _currentEmotionString;
    public string FinalEmotionString = null;
    [SerializeField] private int _clearTimes = 5;
    private int _currentTime = 1;

    private float _startTime;

    void Awake()
    {
        Emotions = new Dictionary<string, Sprite>()
        {
            ["Slightly Smiling"] = _slightlySmiling,
            ["Slightly Frowning"] = _slightlyFrowning,
            ["Kissing"] = _kissing,
            ["Grinning"] = _grinning,
            ["Frowning Open Mouth"] = _frowningOpenMouth,
            ["Flushed"] = _flushed,
            ["OpenMouth"] = _openMouth,
            ["Expressionless"] = _expressionless,
            ["Angry"] = _angry,
        };
        _keys = new List<string>(Emotions.Keys);
    }

    void StartRandomGenerate()
    {
        if (_currentTime <= _clearTimes)
        {
            GameManager.Instance.SetCurrentTime(99);
            if (FinalEmotionString != null)
            {
                _keys.Remove(FinalEmotionString);
            }
            FinalEmotionString = null;
            StartCoroutine(RandomGenerate(3));
        }
        else
        {
            FinalEmotionString = null;
            _currentEmotion.sprite = _successSprite;
            GameManager.Instance.MissionComplete();
        }
    }

    IEnumerator RandomGenerate(int duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            _currentEmotionString = _keys[Random.Range(0, _keys.Count)];
            _currentEmotion.sprite = Emotions[_currentEmotionString];
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        FinalEmotionString = _currentEmotionString;

        GameManager.Instance.SetCurrentTimeToUIData();

        _startTime = Time.time;
    }

    void StartAppearRate()
    {
        StartCoroutine(AppearRate());
    }

    IEnumerator AppearRate()
    {
        switch (Time.time - _startTime)
        {
            case float n when n < 2:
                _rate.GetComponent<Image>().sprite = _excellent;
                break;

            case float n when n < 3 && n > 2:
                _rate.GetComponent<Image>().sprite = _great;
                break;

            case float n when n < 4 && n > 3:
                _rate.GetComponent<Image>().sprite = _good;
                break;

            case float n when n > 4:
                _rate.GetComponent<Image>().sprite = _ok;
                break;

            default:
                break;
        }

        yield return TransformUtil.ScaleObject(_rate, 0, 1, 0.2f);
        yield return new WaitForSeconds(0.5f);
        yield return TransformUtil.ScaleObject(_rate, 1, 0, 0.2f);
    }

    void AddCurrentTime()
    {
        _currentTime++;
    }

    void OnEnable()
    {
        GameManager.Instance.OnMissionStart += StartRandomGenerate;
        _faceController.OnCorrectEmotion += StartAppearRate;
        _faceController.OnCorrectEmotion += AddCurrentTime;
        _faceController.OnCorrectEmotion += StartRandomGenerate;
    }

    void OnDisable()
    {
        GameManager.Instance.OnMissionStart -= StartRandomGenerate;
        _faceController.OnCorrectEmotion -= StartAppearRate;
        _faceController.OnCorrectEmotion -= AddCurrentTime;
        _faceController.OnCorrectEmotion -= StartRandomGenerate;
    }
}
