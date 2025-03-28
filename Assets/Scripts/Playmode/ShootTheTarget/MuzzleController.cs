using System;
using System.Collections;
using UnityEngine;

public class MuzzleController : HeadTransformController
{
    [SerializeField] private float _fireRate;
    private float _nextFire = 0;

    public event Action<GameObject> ShootTheTarget;

    protected override void PlayerController()
    {
        MoveHeadInXY();
        CheckEyeBlinkOrNot();
        if (GameManager.Instance.MissionStart && EyeBlink && Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate; // 下次可發射的門檻時間(_nextFire) = 現在經過時間 + 發射緩衝時間
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.forward, out hitInfo))
        {
            GameObject impactGenerate = ObjectPooler.Instance.SpawnFromPool("Effect", hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            if (hitInfo.collider.CompareTag("Target"))
            {
                ShootTheTarget?.Invoke(hitInfo.collider.gameObject);
            }

            StartCoroutine(FadingAndDeactive(impactGenerate));
        }
    }

    IEnumerator FadingAndDeactive(GameObject _gameObject)
    {
        yield return new WaitForSeconds(1.5f);
        _gameObject.SetActive(false);
    }
}
