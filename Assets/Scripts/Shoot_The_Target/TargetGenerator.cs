using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGenerator : MonoBehaviour
{
    private GameManager manager;
    [SerializeField] private GameObject[] targets;
    [SerializeField] private int targetsCount = 3;
    private HashSet<int> randomIndexSet = new HashSet<int>();

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        while (randomIndexSet.Count < targetsCount)
        {
            randomIndexSet.Add(Random.Range(0, targets.Length));
        }

        foreach (int index in randomIndexSet)
        {
            targets[index].SetActive(true);
        }

        foreach (Transform child in gameObject.transform)
        {
            if (!child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }
    }

    
    void Update()
    {
        if (gameObject.transform.childCount == 0)
        {
            manager.success = true;
        }
    }
}
