using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleController : HeadTransformController
{
    public GameObject impactEffect;
    private float fireRate = 0.5f;
    private float nextFire = 0;
    
    void Start()
    {
        SetTheNecessaryElements();
    }

    void Update()
    {
        PlayerController();
    }

    protected override void SetTheNecessaryElements()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraToUI_offset = 1;
        UI_width = 50;
        UI_height = 25;
    }

    protected override void PlayerController()
    {
        MoveHeadInXY();
        if (manager.gameStart && eyeBlink() && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, Vector3.forward, out hitInfo))
        {
            GameObject impactGenerate = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

            if (hitInfo.collider.CompareTag("Target"))
            {
                hitInfo.transform.GetComponent<TargetController>().destroy = true;
            }

            Destroy(impactGenerate, 1.5f);
        }
    }
}
