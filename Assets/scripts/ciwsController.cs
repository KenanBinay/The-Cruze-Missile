using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ciwsController : MonoBehaviour
{
    public GameObject gunM, gunUp, roundEffect;
    GameObject missile;

    public static bool targetDetected;

    void Start()
    {
        missile = GameObject.Find("Missile_main").gameObject;
        targetDetected = false;
        roundEffect.SetActive(false);

        DOTween.SetTweensCapacity(3200, 60);
    }

    private void Update()
    {
        if (ciwsSpawner.ciwsSpawned)
        {
            if (missileController.crashed || missileController.targetHit || !targetDetected)
            {
                gunM.transform.DORotate(new Vector3(0, 0, 0), 3);
                gunUp.transform.DORotate(new Vector3(0, 0, 0), 3);

                roundEffect.SetActive(false);
                targetDetected = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("missileM") && ciwsSpawner.ciwsSpawned) { targetDetected = true; Debug.Log("lockedOn"); }      
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("missileM") && !missileController.crashed && !missileController.targetHit && ciwsSpawner.ciwsSpawned)
        {
            roundEffect.SetActive(true);

            gunM.transform.DOLookAt(new Vector3(0, missile.transform.position.y, 0), 2);
            gunUp.transform.DOLookAt(missile.transform.position, 2);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("missileM")) 
        {
            Debug.Log("lockedOff");
            targetDetected = false;    
        }
    }
}
