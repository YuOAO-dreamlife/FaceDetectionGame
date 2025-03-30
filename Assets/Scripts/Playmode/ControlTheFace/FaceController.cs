using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceController : HeadTransformController
{
    [SerializeField] private SkinnedMeshRenderer _meshRenderer;
    [SerializeField] private RandomEmotionGenerator _randomEmotion;

    public event Action OnCorrectEmotion;

    protected override void PlayerController()
    {
        RotateHead();
        ChangeFaceEmotions(_meshRenderer);
        switch (_randomEmotion.FinalEmotionString)
        {
            case "Slightly Smiling":
                if (_landmarkInfo.BlendshapesBool["mouthSmileLeft"] 
                    && _landmarkInfo.BlendshapesBool["mouthSmileRight"] 
                    && !_landmarkInfo.BlendshapesBool["jawOpen"])
                {
                    Debug.Log("Slightly Smiling");
                    OnCorrectEmotion?.Invoke();
                }
                break;

            case "Slightly Frowning":
                if (_landmarkInfo.BlendshapesBool["mouthFrownLeft"]
                    && _landmarkInfo.BlendshapesBool["mouthFrownRight"]
                    && !_landmarkInfo.BlendshapesBool["jawOpen"])
                {
                    Debug.Log("Slightly Frowning");
                    OnCorrectEmotion?.Invoke();
                }
                break;
                
            case "Kissing":
                if (_landmarkInfo.BlendshapesBool["mouthPucker"]
                    && !_landmarkInfo.BlendshapesBool["jawOpen"])
                {
                    Debug.Log("Kissing");
                    OnCorrectEmotion?.Invoke();
                }
                break;
                
            case "Grinning":
                if (_landmarkInfo.BlendshapesBool["mouthSmileLeft"] 
                    && _landmarkInfo.BlendshapesBool["mouthSmileRight"] 
                    && _landmarkInfo.BlendshapesBool["jawOpen"])
                {
                    Debug.Log("Grinning");
                    OnCorrectEmotion?.Invoke();
                }
                break;
                
            case "Frowning Open Mouth":
                if (_landmarkInfo.BlendshapesBool["mouthFrownLeft"]
                    && _landmarkInfo.BlendshapesBool["mouthFrownRight"]
                    && _landmarkInfo.BlendshapesBool["jawOpen"])
                {
                    Debug.Log("Frowning Open Mouth");
                    OnCorrectEmotion?.Invoke();
                }
                break;
                
            case "Flushed":
                if (!_landmarkInfo.BlendshapesBool["jawOpen"]
                    && _landmarkInfo.BlendshapesBool["eyeWideLeft"]
                    && _landmarkInfo.BlendshapesBool["eyeWideRight"]
                    && _landmarkInfo.BlendshapesBool["browInnerUp"]
                    && _landmarkInfo.BlendshapesBool["browOuterUpLeft"]
                    && _landmarkInfo.BlendshapesBool["browOuterUpRight"])
                {
                    Debug.Log("Flushed");
                    OnCorrectEmotion?.Invoke();
                }
                break;
                
            case "OpenMouth":
                if (_landmarkInfo.BlendshapesBool["jawOpen"])
                {
                    Debug.Log("OpenMouth");
                    OnCorrectEmotion?.Invoke();
                }
                break;
                
            case "Expressionless":
                if (_landmarkInfo.BlendshapesBool["eyeBlinkLeft"]
                    && _landmarkInfo.BlendshapesBool["eyeBlinkRight"]
                    && !_landmarkInfo.BlendshapesBool["mouthSmileLeft"]
                    && !_landmarkInfo.BlendshapesBool["mouthSmileRight"]
                    && !_landmarkInfo.BlendshapesBool["mouthFrownLeft"]
                    && !_landmarkInfo.BlendshapesBool["mouthFrownRight"]
                    && !_landmarkInfo.BlendshapesBool["jawOpen"])
                {
                    Debug.Log("Expressionless");
                    OnCorrectEmotion?.Invoke();
                }
                break;
                
            case "Angry":
                if (!_landmarkInfo.BlendshapesBool["jawOpen"]
                    && _landmarkInfo.BlendshapesBool["browDownLeft"]
                    && _landmarkInfo.BlendshapesBool["browDownRight"]
                    && _landmarkInfo.BlendshapesBool["mouthFrownLeft"]
                    && _landmarkInfo.BlendshapesBool["mouthFrownRight"])
                {
                    Debug.Log("Angry");
                    OnCorrectEmotion?.Invoke();
                }
                break;
                
            default:
                break;
        }
    }
}
