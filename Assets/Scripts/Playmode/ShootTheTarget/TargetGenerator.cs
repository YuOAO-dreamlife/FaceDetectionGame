using System.Collections.Generic;
using UnityEngine;

public class TargetGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _targets;
    [SerializeField] private int _targetsCount = 3;
    private HashSet<int> _randomIndexSet = new HashSet<int>();

    void Start()
    {
        while (_randomIndexSet.Count < _targetsCount)
        {
            _randomIndexSet.Add(Random.Range(0, _targets.Length));
        }

        foreach (int index in _randomIndexSet)
        {
            _targets[index].SetActive(true);
        }

        foreach (Transform child in gameObject.transform)
        {
            if (!child.gameObject.activeSelf)
            {
                Destroy(child.gameObject);
            }
        }
    }

    void OnTransformChildrenChanged()
    {
        if (gameObject.transform.childCount == 0)
        {
            GameManager.Instance.MissionComplete();
        }
    }
}
