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
        missile = GameObject.Find("Missile").gameObject;
        targetDetected = false;
        roundEffect.SetActive(false);

        DOTween.SetTweensCapacity(3200, 60);
    }

    private void LateUpdate()
    {
        if (missileController.crashed || missileController.targetHit || !targetDetected)
        {
            if (gunM != null) gunM.transform.DORotate(new Vector3(0, 0, 0), 3);
            if (gunUp != null) gunUp.transform.DORotate(new Vector3(0, 0, 0), 3);

            roundEffect.SetActive(false);
            targetDetected = false;
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

            if (gunM != null) gunM.transform.DOLookAt(new Vector3(0, missile.transform.position.y, 0), 4);
            if (gunUp != null) gunUp.transform.DOLookAt(missile.transform.position, 4);
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
