using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : HeadTransformController
{
    private float _duration = 1;

    protected override void PlayerController()
    {
        if (!GameManager.Instance.MissionFailure)
        {
            MoveHeadInXY();
            RotateHead();
        }
    }

    void GhostFailedAction()
    {
        StartCoroutine(HeadFaceTheCamera());
    }
    
    IEnumerator HeadFaceTheCamera()
    {
        float elapsedTime = 0;

        while (elapsedTime < _duration)
        {
            float t = elapsedTime / _duration;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 180, 0), t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    void OnEnable()
    {
        GameManager.Instance.OnMissionFailure += GhostFailedAction;
    }

    void OnDisable()
    {
        GameManager.Instance.OnMissionFailure -= GhostFailedAction;
    }
}
