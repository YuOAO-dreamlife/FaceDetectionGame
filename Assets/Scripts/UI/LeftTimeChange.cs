using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeftTimeChange : MonoBehaviour
{
    private GameManager manager;
    private TMP_Text timeText;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        timeText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (manager.leftTime <= 3)
        {
            timeText.text = "The left time of the mission... " + manager.leftTime.ToString() + "...";
        }
        else
        {
            timeText.text = "";
        }
    }
}
