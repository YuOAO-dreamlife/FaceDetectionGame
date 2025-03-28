using System.Collections;
using UnityEngine;

public class SoccerController : MonoBehaviour
{
    void FixedUpdate()
    {
        if (gameObject.transform.position.y < -1)
        {
            SoccerDisappear();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(FadingAndDeactive());
        }
    }

    IEnumerator FadingAndDeactive()
    {
        yield return StartCoroutine(MaterialUtil.FadeOut(gameObject, 0.5f));
        SoccerDisappear();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            GameManager.Instance.MissionFailed();
        }
    }

    void SoccerDisappear()
    {
        gameObject.SetActive(false);
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        MaterialUtil.SetToFullOpacity(gameObject);
    }

    void OnEnable()
    {
        GameManager.Instance.OnMissionSuccess += SoccerDisappear;
    }

    void OnDisable()
    {
        GameManager.Instance.OnMissionSuccess -= SoccerDisappear;
    }
}