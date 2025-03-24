using System.Collections;
using UnityEngine;

public abstract class UIStateBase
{
    protected UIManager _manager;
    private Coroutine _currentCoroutine;

    protected float _instructionScaleDuration = 0.15f;
    protected float _instructionDelay = 1.5f;
    protected float _hintScaleDuration = 0.15f;
    protected float _hintDelay = 1.0f;
    protected float _uIScaleDuration = 0.2f;
    protected float _uITransparentDuration = 0.2f;
    protected float _preBackToUIDelay;
    protected float _postBackToUIDelay = 3.5f;
    protected float _blackUITransparentDuration = 2.0f;

    protected AudioClip _intermissionSoundtrack = Resources.Load<AudioClip>("Soundtracks/Default");
    protected AudioClip _successSoundtrack = Resources.Load<AudioClip>("Soundtracks/Success");
    protected AudioClip _failedSoundtrack = Resources.Load<AudioClip>("Soundtracks/Failed");
    protected AudioClip _gameOverSoundtrack = Resources.Load<AudioClip>("Soundtracks/GameOver");

    public UIStateBase(UIManager manager)
    {
        this._manager = manager;
    }

    public abstract void Enter();
    public virtual void Exit()
    {
        StopTrackedCoroutine();
    }

    protected void StartTrackedCoroutine(IEnumerator routine)
    {
        _currentCoroutine = _manager.StartCoroutine(routine);
    }

    protected void StopTrackedCoroutine()
    {
        if (_currentCoroutine != null)
        {
            _manager.StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }
}