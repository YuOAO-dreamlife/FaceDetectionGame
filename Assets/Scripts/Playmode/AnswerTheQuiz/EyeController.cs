using System;
using UnityEngine;

public class EyeController : HeadTransformController
{
    public event Action OnAnswerRight;
    public event Action OnAnswerLeft;

    protected override void PlayerController()
    {
        if (GameManager.Instance.MissionStart && !GameManager.Instance.MissionEnd)
        {
            if (eyeLookInRight())
            {
                OnAnswerRight?.Invoke();
            }
            else if (eyeLookInLeft())
            {
                OnAnswerLeft?.Invoke();
            }
        }
    }
}
