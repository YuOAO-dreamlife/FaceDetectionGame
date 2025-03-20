using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] private GameObject _destroyedVer;
    private MuzzleController _muzzleController;

    void DestroyTarget(GameObject target)
    {
        if (target == gameObject)
        {
            Instantiate(_destroyedVer, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        _muzzleController = GameObject.Find("Muzzle").GetComponent<MuzzleController>();
        _muzzleController.ShootTheTarget += DestroyTarget;
    }

    void OnDisable()
    {
        _muzzleController.ShootTheTarget -= DestroyTarget;
    }
}
