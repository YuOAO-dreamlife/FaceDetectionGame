using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    public GameObject destroyedVer;
    public bool destroy = false;

    void Update()
    {
        if (destroy)
        {
            Instantiate(destroyedVer, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
