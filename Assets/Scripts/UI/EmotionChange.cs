using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionChange : MonoBehaviour
{
    private GameManager manager;
    public UIManager uIManager;
    [SerializeField] private GameObject[] defaultEmotions;
    [SerializeField] private GameObject[] failedEmotions;
    [SerializeField] private GameObject[] successEmotions;
    private int defaultRandom;
    private int failedRandom;
    private int successRandom;

    // Start is called before the first frame update
    void Start()
    {
        defaultRandom = Random.Range(0, defaultEmotions.Length);
        failedRandom = Random.Range(0, failedEmotions.Length);
        successRandom = Random.Range(0, successEmotions.Length);
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.gameEnd && !(uIManager.currentState == UIManager.UIState.UIResult))
        {
            defaultEmotions[defaultRandom].SetActive(true);
        }
        else if (uIManager.currentState == UIManager.UIState.UIResult)
        {
            defaultEmotions[defaultRandom].SetActive(false);
            if (manager.failed) 
            {
                failedEmotions[failedRandom].SetActive(true);
            }
            else 
            {
                successEmotions[successRandom].SetActive(true);
            }
        }  
    }
}
