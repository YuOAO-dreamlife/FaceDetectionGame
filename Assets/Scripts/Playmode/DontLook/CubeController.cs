using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeController : MonoBehaviour
{
    [SerializeField] private float _amplitude = 0.1f;
    [SerializeField] private float _frequency = 1f;
    [SerializeField] private float _rotateTime;
    private Vector3 _initialPosition;
    [SerializeField] private int _clearTimes = 3;
    private int _currentTime = 1;

    [SerializeField] private Transform[] _raycastOrigins;
    [SerializeField] private LayerMask _uiLayerMask;
    [SerializeField] private LayerMask _objLayerMask;
    [SerializeField] private List<string> _detectedDirections = new List<string>();
    [SerializeField] private List<GameObject> _detectedFaces = new List<GameObject>();
    [SerializeField] private List<GameObject> _detectedFaceObjects = new List<GameObject>();
    [SerializeField] private List<Image> _directionImages = new List<Image>();
    [SerializeField] private Image _successImg;
    [SerializeField] private Image _failedImg;
    private float _rayDistance = 3.5f;

    [SerializeField] private HeadRotateController _headRotateController;
    [SerializeField] private GameObject _front;
    [SerializeField] private GameObject _back;
    [SerializeField] private GameObject _top;
    [SerializeField] private GameObject _bottom;
    [SerializeField] private GameObject _left;
    [SerializeField] private GameObject _right;
    [SerializeField] private Transform[] _raycastFrontOrigins;
    [SerializeField] private Transform[] _raycastBackOrigins;
    [SerializeField] private Transform[] _raycastTopOrigins;
    [SerializeField] private Transform[] _raycastBottomOrigins;
    [SerializeField] private Transform[] _raycastLeftOrigins;
    [SerializeField] private Transform[] _raycastRightOrigins;

    private bool _rotating = true;
    
    void Start()
    {
        _initialPosition = transform.position;
        // StartRotateCube();
    }

    void Update()
    {
        ShakingEffect();
    }

    void ShakingEffect()
    {
        // 使用sin和cos在XZ平面上產生平滑的晃動效果
        float offsetX = _amplitude * Mathf.Sin(Time.time * _frequency);
        float offsetZ = _amplitude * Mathf.Cos(Time.time * _frequency);
        transform.position = _initialPosition + new Vector3(offsetX, 0, offsetZ);
    }

    void StartRotateCube()
    {
        StartCoroutine(RotateCube());
    }

    IEnumerator RotateCube()
    {
        _rotating = true;
        // 嘗試旋轉到含有指定的方向數量
        GameManager.Instance.SetCurrentTime(99);
        do
        {
            // 隨機旋轉
            DetectRotateCubes(_raycastFrontOrigins, _front);
            yield return StartCoroutine(RandomRotateZ(_front));
            DetectRotateCubes(_raycastBackOrigins, _back);
            yield return StartCoroutine(RandomRotateZ(_back));
            DetectRotateCubes(_raycastTopOrigins, _top);
            yield return StartCoroutine(RandomRotateY(_top));
            DetectRotateCubes(_raycastBottomOrigins, _bottom);
            yield return StartCoroutine(RandomRotateY(_bottom));
            DetectRotateCubes(_raycastLeftOrigins, _left);
            yield return StartCoroutine(RandomRotateX(_left));
            DetectRotateCubes(_raycastRightOrigins, _right);
            yield return StartCoroutine(RandomRotateX(_right));

            DetectDirection();
        } while (_detectedDirections.Count == 4);

        AppearDirections();

        // 已完成旋轉，開始正式計時
        GameManager.Instance.SetCurrentTimeToUIData();
        _rotating = false;
    }

    void DetectRotateCubes(Transform[] raycastOrigins, GameObject pivot)
    {
        for (int i = 0; i < raycastOrigins.Length; i++)
        {
            Ray ray = new Ray(raycastOrigins[i].position, raycastOrigins[i].forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _rayDistance, _objLayerMask))
            {
                hit.collider.transform.SetParent(pivot.transform);
            }
            else
            {
                i--; // 未偵測成功，重新偵測一次
            }
        }
    }

    IEnumerator RandomRotateZ(GameObject pivot)
    {
        int rotateCounts = Random.Range(1, 4);
        for (int count = 1; count <= rotateCounts; count++)
        {
            Quaternion startQuat = pivot.transform.rotation;
            Quaternion endQuat = startQuat * Quaternion.Euler(0, 0, 90);
            yield return StartCoroutine(TransformUtil.RotateToQuat(pivot.transform, startQuat, endQuat, _rotateTime));
        }
    }

    IEnumerator RandomRotateY(GameObject pivot)
    {
        int rotateCounts = Random.Range(1, 4);
        for (int count = 1; count <= rotateCounts; count++)
        {
            Quaternion startQuat = pivot.transform.rotation;
            Quaternion endQuat = startQuat * Quaternion.Euler(0, 90, 0);
            yield return StartCoroutine(TransformUtil.RotateToQuat(pivot.transform, startQuat, endQuat, _rotateTime));
        }
    }

    IEnumerator RandomRotateX(GameObject pivot)
    {
        int rotateCounts = Random.Range(1, 4);
        for (int count = 1; count <= rotateCounts; count++)
        {
            Quaternion startQuat = pivot.transform.rotation;
            Quaternion endQuat = startQuat * Quaternion.Euler(90, 0, 0);
            yield return StartCoroutine(TransformUtil.RotateToQuat(pivot.transform, startQuat, endQuat, _rotateTime));
        }
    }

    void DetectDirection()
    {
        _detectedDirections.Clear();
        _detectedFaces.Clear();
        foreach (Transform origin in _raycastOrigins)
        {
            Ray ray = new Ray(origin.position, origin.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _rayDistance, _uiLayerMask))
            {
                GameObject direction = hit.collider.gameObject;
                if (!_detectedDirections.Contains(direction.name) && direction.name != "None")
                {
                    _detectedDirections.Add(direction.name);
                    Debug.Log("新增方向：" + direction.name);
                }
                _detectedFaces.Add(direction);
            }

            if (Physics.Raycast(ray, out hit, _rayDistance, _objLayerMask))
            {
                GameObject cube = hit.collider.gameObject;
                _detectedFaceObjects.Add(cube);
            }
        }
    }

    void AppearDirections()
    {
        for (int index = 0; index < _directionImages.Count; index++)
        {
            if (_detectedFaces[index].name != "None")
            {
                _directionImages[index].sprite = Resources.Load<Sprite>("Sprites/Gameplay/" + _detectedFaces[index].name);
                _directionImages[index].color = new Color(_directionImages[index].color.r, _directionImages[index].color.g, _directionImages[index].color.b, 255);
            }
        }
    }

    void ChangeDetectedFacesColor(Color color)
    {
        foreach (GameObject cube in _detectedFaceObjects)
        {
            Material[] materials = cube.GetComponent<MeshRenderer>().materials;
            foreach (Material mat in materials)
            {
                if (mat.color != Color.black)
                {
                    mat.color = color;
                }
            }
        }
    }

    void DisappearDirections()
    {
        for (int index = 0; index < _directionImages.Count; index++)
        {
            if (_detectedFaces[index].name != "None")
            {
                _directionImages[index].color = new Color(_directionImages[index].color.r, _directionImages[index].color.g, _directionImages[index].color.b, 0);
            }
        }
    }

    void CheckAnswer(string answer)
    {
        if (!_rotating)
        {
            Debug.Log("Check");
            DisappearDirections();
            if (_detectedDirections.Contains(answer))
            {
                ChangeDetectedFacesColor(Color.red);
                _failedImg.color = new Color(_failedImg.color.r, _failedImg.color.g, _failedImg.color.b, 255);
                GameManager.Instance.MissionFailed();
            }
            else
            {
                if (_currentTime < _clearTimes)
                {
                    _currentTime++;    
                    StartRotateCube();
                }
                else
                {
                    ChangeDetectedFacesColor(Color.green);
                    _successImg.color = new Color(_successImg.color.r, _successImg.color.g, _successImg.color.b, 255);
                    GameManager.Instance.MissionComplete();
                }
            }
        }
    }

    void OnEnable()
    {
        GameManager.Instance.OnMissionStart += StartRotateCube;
        _headRotateController.OnHeadRotate += CheckAnswer;
    }

    void OnDisable()
    {
        GameManager.Instance.OnMissionStart -= StartRotateCube;
        _headRotateController.OnHeadRotate -= CheckAnswer;
    }
}
