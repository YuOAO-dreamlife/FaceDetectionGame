using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InstructionChange : MonoBehaviour
{
    private UIManager manager;
    [SerializeField] private TMP_Text instructionText;

    void Update()
    {
        manager = GameObject.Find("UI Objects").GetComponent<UIManager>();
        GetComponent<Image>().sprite = manager.currentInstructionImage;
        instructionText.text = manager.currentInstruction;
    }
}
