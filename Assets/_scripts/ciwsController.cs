using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ciwsController : MonoBehaviour
{
    public GameObject gunM, gunUp, roundEffect;
    GameObject missile;

    public static bool targetDetected;

    [SerializeField] AudioSource ciwsSource;

    void Start()
    {
        missile = GameObject.Find("Missile").gameObject;
        targetDetected = false;
        roundEffect.SetActive(false);

        DOTween.SetTweensCapacity(3200, 60);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("missileM") && ciwsSpawner.ciwsSpawned)
            targetDetected = true; Debug.Log("lockedOn");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("missileM") && !missileController.crashed
            && !missileController.targetHit && ciwsSpawner.ciwsSpawned)
        {
            gunM.transform.DOLookAt(new Vector3(0, missile.transform.position.y, 0), 3);
            gunUp.transform.DOLookAt(missile.transform.position, 3);

            if (!roundEffect.activeSelf) roundEffect.SetActive(true);
            if (!ciwsSource.isPlaying)
            {
                if (PlayerPrefs.GetInt("sfx") == 1) ciwsSource.Play();
            } 
        }
        if (other.gameObject.CompareTag("missileM")
            && missileController.crashed || missileController.targetHit)
        {
            StartCoroutine(returnStatic());
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("missileM"))
        {
            Debug.Log("lockedOff");
            targetDetected = false;        

            StartCoroutine(returnStatic());
        }
    }

    IEnumerator returnStatic()
    {
        gunM.transform.DORotate(new Vector3(0, 0, 0), 0.7f);
        gunUp.transform.DORotate(new Vector3(0, 0, 0), 0.7f);

        if (roundEffect.activeSelf) roundEffect.SetActive(false);

        yield return new WaitForSeconds(1);

        ciwsSource.Stop();

        DOTween.Complete(gunUp, gunM);
    }
}
