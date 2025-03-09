using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private GameObject ghost;
    private GameObject manager;
    private float x_max = 100, x_min = -100, y_max = 50, y_min = -50;
    private float speed = 30;
    private Vector3 targetPos;

    void Start()
    {
        ghost = GameObject.Find("Ghost");
        manager = GameObject.Find("GameManager");
        PositionChange();
    }

    void PositionChange()
    {
        targetPos = new Vector2(Random.Range(x_min, x_max), Random.Range(y_min, y_max));
    }

    void Update()
    {
        if (manager.GetComponent<GameManager>().failed)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(ghost.transform.position.x, ghost.transform.position.y, 0), Time.deltaTime * speed / 3);
            gameObject.GetComponent<Light>().spotAngle = 30;
            gameObject.GetComponent<Light>().intensity = 5;
        }
        else if (manager.GetComponent<GameManager>().success)
        {
            Destroy(gameObject);
        }
        else
        {
            if (Vector2.Distance(transform.position, targetPos) < 1) 
            {
                PositionChange();
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ghost.GetComponent<Animator>().SetBool("Damage", true);
            manager.GetComponent<GameManager>().failed = true;
        }
        else if (other.tag == "Light")
        {
            PositionChange();
        }
    }
}
