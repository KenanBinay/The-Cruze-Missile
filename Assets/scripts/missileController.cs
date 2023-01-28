using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class missileController : MonoBehaviour
{
    public FxController fx;

    public static Vector3 normal;
    public static Vector2 handleInput;
    public Transform hudYawUi;
    public GameObject mainHudUi, warningUi, hitFlash_image, hitEffect_particle;
    Rigidbody rigidM;

    public float flySpeed, yawAmount;
    float yaw, pitch, yawHudHorizontal, yawHudVertical;
    bool missileYawPitchSet;

    public static bool crashed, targetHit, outside, ciwsHit;
    public static int hitVal;

    void Start()
    {
        hitVal = 0;
        pitch = 0f;
        outside = ciwsHit = crashed = targetHit = missileYawPitchSet = false;
        rigidM = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (gameController.startDelay && !crashed && !targetHit)
        {
            //move forward
            transform.position += transform.forward * flySpeed * Time.deltaTime;

            if (!gameController.firstScreenTouch || gameController.startDelay && !missileYawPitchSet)
            {
                pitch = 0;

                if (missileSpawnManager.spawnedTop) { yaw = 0; }
                if (missileSpawnManager.spawnedRight) { yaw = 90; }
                if (missileSpawnManager.spawnedLeft) { yaw = -90; }
                if (missileSpawnManager.spawnedBottom) { yaw = 180; }

                missileYawPitchSet = true;
            }

            // controlling yaw & pitch 
            if (missileYawPitchSet)
            {
                //input
                yaw += handleInput.x * yawAmount * Time.deltaTime / 1.2f;
                pitch += handleInput.y * yawAmount * Time.deltaTime / 1.2f;

                if (yawHudHorizontal <= -90 && handleInput.x < 0 || yawHudHorizontal >= 90 && handleInput.x > 0) { }
                else { yawHudHorizontal += handleInput.x * yawAmount * Time.deltaTime; }

                yawHudVertical += handleInput.y * yawAmount * Time.deltaTime;

                //apply rotation
                transform.localRotation = Quaternion.Euler(Vector3.up * yaw + Vector3.left * pitch);

                if (handleInput == Vector2.zero) { hudYawUi.DOLocalRotate(new Vector3(yawHudVertical, 0, 0), 1); yawHudHorizontal = 0; }
                else { hudYawUi.localRotation = Quaternion.Euler(Vector3.back * yawHudHorizontal + Vector3.right * yawHudVertical); }
            }

            if (ciwsHit) mainHudUi.transform.DOShakeScale(0.4f, 0.03f).onComplete = mHudTweenDone;
        }
        if (crashed || targetHit) // exploding effect call
        {
            if (!FxController.fxExplode)
            {
                if (crashed) Debug.Log("crashed"); fx.crashFx();
                if (targetHit) Debug.Log("targetHit"); fx.targetHitFx();
            }
        }
    }

    void mHudTweenDone() { ciwsHit = false; mainHudUi.transform.localScale = new Vector3(1, 1, 1); }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("crashColl"))
        {
            if (!crashed && !targetHit)
            {
                Debug.Log("crashed");
                crashed = true;
            }           
        }
        if (collision.gameObject.CompareTag("target"))
        {
            if (!crashed && !targetHit)
            {
                Debug.Log("targetHit");

                int missionVal = PlayerPrefs.GetInt("mission", 0) + 1;
                PlayerPrefs.SetInt("mission", missionVal++);
                collision.gameObject.SetActive(false);

                targetHit = true;
            }
        }
        if (collision.gameObject.CompareTag("vehicleTarget"))
        {
            if (!crashed && !targetHit)
            {
                Debug.Log("vehicleTargetHit");

                int missionVal = PlayerPrefs.GetInt("mission", 0) + 1;
                PlayerPrefs.SetInt("mission", missionVal);
                collision.gameObject.SetActive(false);

                targetHit = true;
                collision.gameObject.transform.DOPause();
            }
        }
        if (collision.gameObject.CompareTag("airTarget"))
        {
            if (!crashed && !targetHit)
            {
                Debug.Log("aircraftTargetHit");

                int missionVal = PlayerPrefs.GetInt("mission", 0) + 1;
                PlayerPrefs.SetInt("mission", missionVal);

                targetHit = true;
                collision.gameObject.transform.DOPause();
            }
        }

        rigidM.constraints = RigidbodyConstraints.FreezeAll;
        normal = collision.contacts[0].normal;
        Debug.Log(normal);

    /*    if (normal.x > 0.5f) { Debug.Log("left"); }
        if (normal.x < -0.5f) { Debug.Log("right"); }
        if (normal.y > 0.5f) { Debug.Log("front"); }
        if (normal.y < -0.5f) { Debug.Log("back"); }
        if (normal.z > 0.5f) { Debug.Log("up"); }
        if (normal.z < -0.5f) { Debug.Log("down"); } */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("roundColl"))
        {
            if (!crashed && !targetHit && ciwsController.targetDetected)
            {           
                hitVal++;
                ciwsHit = true;
                Debug.Log("hit: " + hitVal);  

                if (hitVal >= 2) { crashed = true; }
                StartCoroutine(roundHit());
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("outside") && !crashed)
        {
            outside = true;
            Debug.Log("returnToCombat");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("outside"))
        {
            outside = false;
            Debug.Log("returned");
        }
        if (other.gameObject.CompareTag("ciwsRadar"))
        {
            hitVal = 0;
        }
    }

    IEnumerator roundHit()
    {
        hitFlash_image.SetActive(true);
        hitEffect_particle.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        hitFlash_image.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        hitEffect_particle.SetActive(false);
    }
}
