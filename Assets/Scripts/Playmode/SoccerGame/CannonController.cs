using System;
using System.Collections;
using LazySquirrelLabs.MinMaxRangeAttribute;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private GameObject _rotateStage;
    [SerializeField] private GameObject _generatePos;
    [SerializeField] private GameObject _shootEffectPos;
    [SerializeField] private float _rotateSpeed = 10;
    [SerializeField, MinMaxRange(0, 20)] private Vector2Int _fireForceRange;
    [SerializeField, MinMaxRange(0.5f, 2.0f)] private Vector2 _durationRange;

    private int _fireForce;
    private float _elapsedTime = 0;
    private float _duration;
    private float _originalRotation;
    private float _currentRotateSpeed;

    private event Action OnCannonFire;

    void Start()
    {
        ResetFireStatus();
        _originalRotation = _rotateStage.transform.eulerAngles.y;
        _currentRotateSpeed = UnityEngine.Random.Range(0, 2) == 0 ? _rotateSpeed : -_rotateSpeed;
        Debug.Log(_originalRotation);
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.MissionStart && !GameManager.Instance.MissionEnd)
        {
            if (!Physics.Raycast(_generatePos.transform.position, _generatePos.transform.forward))
            {
                if (_rotateStage.transform.eulerAngles.y > _originalRotation)
                {
                    _currentRotateSpeed = -_rotateSpeed;
                }
                else
                {
                    _currentRotateSpeed = _rotateSpeed;
                }
            }
            _rotateStage.transform.Rotate(_rotateStage.transform.up, _currentRotateSpeed * Time.fixedDeltaTime);

            if (_elapsedTime < _duration)
            {
                _elapsedTime += Time.deltaTime;
            }
            else
            {
                OnCannonFire?.Invoke();
                _elapsedTime = 0;
                ResetFireStatus();
            }
            Debug.DrawLine(_generatePos.transform.position, _generatePos.transform.position + _generatePos.transform.forward * 100f);
        }
    }

    void ResetFireStatus()
    {
        _fireForce = UnityEngine.Random.Range(_fireForceRange.x, _fireForceRange.y);
        _duration = UnityEngine.Random.Range(_durationRange.x, _durationRange.y);
    }

    void FireTheSoccer()
    {
        GameObject soccerClone = ObjectPooler.Instance.SpawnFromPool("Soccer", _generatePos.transform.position, Quaternion.identity);
        soccerClone.GetComponent<Rigidbody>().AddForce(_shootEffectPos.transform.forward * _fireForce, ForceMode.Impulse);
        GameObject effectGenerate = ObjectPooler.Instance.SpawnFromPool("ShootEffect", _shootEffectPos.transform.position, Quaternion.LookRotation(_shootEffectPos.transform.forward));
        StartCoroutine(DeactivateAfterDelay(effectGenerate, 1.5f));
    }

    IEnumerator DeactivateAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        obj.SetActive(false);
    }

    void OnEnable()
    {
        OnCannonFire += FireTheSoccer;
    }

    void OnDisable()
    {
        OnCannonFire -= FireTheSoccer;
    }
}
