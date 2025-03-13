using UnityEngine;

public class UIPulseEffect : MonoBehaviour
{
    private float _minScale = 1.0f;
    private float _maxScale = 1.025f;
    private float _pulseSpeed = 12.0f;
    private RectTransform _rectTransform;
    private float _timer;
    private Vector3 _originalScale;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalScale = _rectTransform.localScale;
    }

    void Update()
    {
        _timer += Time.deltaTime * _pulseSpeed;
        float scale = Mathf.Lerp(_minScale, _maxScale, (Mathf.Sin(_timer) + 1.0f) / 2.0f);
        _rectTransform.localScale = _originalScale * scale;
    }
}
