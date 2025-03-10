using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightGenerator : MonoBehaviour
{
    public GameObject spotLight, parent, ghost;
    public GameManager manager;
    public int spotLightAmount;
    private float x_max = 100, x_min = -100, y_max = 50, y_min = -50;
    private bool lightHasGenerated = false;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (manager.gameStart && !lightHasGenerated)
        {
            for (int count = 1; count <= spotLightAmount; count++)
            {
                while(true)
                {
                    Vector3 lightPos = new Vector3(Random.Range(x_min, x_max), Random.Range(y_min, y_max), 0);
                    if (Vector2.Distance(ghost.transform.position, lightPos) > 40)
                    {
                        Instantiate(spotLight, lightPos, Quaternion.identity, parent.transform);
                        break;
                    }
                }
            }
            lightHasGenerated = true;
        }
    }
}
