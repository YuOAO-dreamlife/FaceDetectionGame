using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTextChange : MonoBehaviour
{
    private GameManager manager;
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SettingText();
    }

    void SettingText()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Title_Screen":
                GetComponent<TMP_Text>().text = "Face the camera";
                break;

            case "Game_Over":
                GetComponent<TMP_Text>().text = "Result\nLevel " + manager.countOfScenesHasLoaded;
                break;

            default:
                GetComponent<TMP_Text>().text = "Level " + manager.countOfScenesHasLoaded;
                break;
        }
    }
}
