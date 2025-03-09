using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeftTimeChange : MonoBehaviour
{
    private GameManager manager;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (manager.leftTime <= 3)
        {
            GetComponent<TMP_Text>().text = "The left time of the mission... " + manager.leftTime.ToString() + "...";
        }
        else
        {
            GetComponent<TMP_Text>().text = "";
        }
    }
}
