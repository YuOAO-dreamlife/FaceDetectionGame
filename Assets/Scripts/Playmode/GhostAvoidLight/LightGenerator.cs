using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _spotLight;
    [SerializeField] private GameObject _parent;
    [SerializeField] private GameObject _ghost;
    [SerializeField] private int _spotLightAmount;
    [SerializeField] private float _Xmax; 
    [SerializeField] private float _Xmin;
    [SerializeField] private float _Ymax;
    [SerializeField] private float _Ymin;

    void GenerateLights()
    {
        for (int count = 1; count <= _spotLightAmount; count++)
        {
            while(true)
            {
                Vector3 lightPos = new Vector3(Random.Range(_Xmin, _Xmax), Random.Range(_Ymin, _Ymax), 0);
                if (Vector2.Distance(_ghost.transform.position, lightPos) > 40)
                {
                    Instantiate(_spotLight, lightPos, Quaternion.identity, _parent.transform);
                    break;
                }
            }
        }
    }

    void OnEnable()
    {
        GameManager.Instance.OnMissionStart += GenerateLights;
    }

    void OnDisable()
    {
        GameManager.Instance.OnMissionStart -= GenerateLights;
    }
}
