using UnityEngine;

public class SoccerController : MonoBehaviour
{
    private bool _isFading = false;

    void FixedUpdate()
    {
        if (gameObject.transform.position.y < -1)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!_isFading && collision.gameObject.CompareTag("Player"))
        {
            _isFading = true;
            StartCoroutine(MaterialUtil.FadeOutAndDestroy(gameObject, 0.5f));
        }
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
        Destroy(gameObject);
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