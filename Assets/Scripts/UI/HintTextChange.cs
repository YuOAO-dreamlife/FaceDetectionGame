using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HintTextChange : MonoBehaviour
{
    private UIManager manager;
    void Update()
    {
        manager = GameObject.Find("UI Objects").GetComponent<UIManager>();
        GetComponent<TMP_Text>().text = manager.currentHint;
    }
}
