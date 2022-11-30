using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ciwsController : MonoBehaviour
{
    public GameObject gunM, gunUp, missile;

    bool targetDetected;

    void Start()
    {
        
    }

    private void Update()
    {
        if (!targetDetected)
        {
            gunM.transform.DORotate(new Vector3(0, 0, 0), 1);
            gunUp.transform.DORotate(new Vector3(0, 0, 0), 1);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("missileM")) { targetDetected = true; }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("missileM"))
        {
            var lookPos = missile.transform.position - gunM.transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            gunM.transform.rotation = Quaternion.Slerp(gunM.transform.rotation, rotation, Time.deltaTime * 3f);

            var lookPosTurret = missile.transform.position - gunUp.transform.position;
            lookPosTurret.x = 0;
            var rotationturret = Quaternion.LookRotation(lookPosTurret);
            gunUp.transform.rotation = Quaternion.Slerp(gunUp.transform.rotation, rotationturret, Time.deltaTime * 3f);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("missileM")) { targetDetected = false; }
    }
}
