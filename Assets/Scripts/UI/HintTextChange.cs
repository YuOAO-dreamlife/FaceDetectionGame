using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HintTextChange : MonoBehaviour
{
    private UIManager manager;
    private TMP_Text hintTextComponent;

    void Start()
    {
        manager = GameObject.Find("UI Objects").GetComponent<UIManager>();
        hintTextComponent = GetComponent<TMP_Text>();
    }
    
    void Update()
    {
        
        hintTextComponent.text = manager.currentHint;
    }
}
