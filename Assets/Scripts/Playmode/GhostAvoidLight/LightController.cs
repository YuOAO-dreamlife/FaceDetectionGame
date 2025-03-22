using UnityEngine;

public class LightController : MonoBehaviour
{
    private GameObject _ghost;
    [SerializeField] private Light _light;
    
    [SerializeField] private float _Xmax; 
    [SerializeField] private float _Xmin;
    [SerializeField] private float _Ymax; 
    [SerializeField] private float _Ymin;
    [SerializeField] private float _speed;

    [SerializeField] private float _gatherTime = 0.1f;
    [SerializeField] private float _gatherSpotAngle;
    [SerializeField] private float _gatherIntensity;

    private Vector3 _targetPos;

    void Start()
    {
        _ghost = GameObject.Find("Ghost");
        _targetPos = PositionChange();
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _targetPos) < 1) 
        {
            _targetPos = PositionChange();
        }
        if (!GameManager.Instance.MissionEnd)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.fixedDeltaTime * _speed);
        }
    }

    Vector3 PositionChange()
    {
        return new Vector3(Random.Range(_Xmin, _Xmax), Random.Range(_Ymin, _Ymax), 0);
    }

    void GatherToPlayer()
    {
        Vector3 ghostPos = new Vector3(_ghost.transform.position.x, _ghost.transform.position.y, 0);
        StartCoroutine(TransformUtil.MoveToPos(transform, transform.position, ghostPos, _gatherTime));
        _light.spotAngle = _gatherSpotAngle;
        _light.intensity = _gatherIntensity;
    }

    void LightDisappear()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _ghost.GetComponent<Animator>().SetBool("Damage", true);
            GameManager.Instance.MissionFailed();
        }
        else if (other.CompareTag("Light"))
        {
            _targetPos = PositionChange();
        }
    }

    void OnEnable()
    {
        GameManager.Instance.OnMissionFailure += GatherToPlayer;
        GameManager.Instance.OnMissionSuccess += LightDisappear;
    }

    void OnDisable()
    {
        GameManager.Instance.OnMissionFailure -= GatherToPlayer;
        GameManager.Instance.OnMissionSuccess -= LightDisappear;
    }
}
