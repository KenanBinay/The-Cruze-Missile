using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class missileController : MonoBehaviour
{
    public scoreManager_inGame scoreManager;
    public FxController fx;

    public static Vector3 normal;
    public static Vector2 handleInput;
    public Transform hudYawUi, cam;
    [SerializeField] public GameObject mainHudUi, warningUi, hitFlash_image, hitEffect_particle,
        missilesParent;
    [SerializeField] public GameObject fullJet_missileParticle, outOfFuel_missileParticle;
 
    Rigidbody rigidM;

    public float flySpeed, yawAmount;
    public static float yaw, pitch, yawHudHorizontal, yawHudVertical, exSpeed;

    public static bool crashed, targetHit, outside, ciwsHit, speedUp;
    public static int hitVal;

    bool explosion;

    Vector3 missileLastRotation;

    [Header("Audio")]
    [SerializeField] AudioSource[] missileSources, explosionSources;
    void Start()
    {
        hitVal = 0;
        pitch = yaw = 0f;
        handleInput = Vector2.zero;

        outside = ciwsHit = crashed = targetHit = false;
        rigidM = gameObject.GetComponent<Rigidbody>();

        //getting last selected missile number 
        int selectedMissileNumb = PlayerPrefs.GetInt("lastSelectedMissile");
        GameObject selectedMissileParent = missilesParent.transform.GetChild(0).gameObject;

        fullJet_missileParticle = selectedMissileParent.transform.GetChild(1).gameObject;
        outOfFuel_missileParticle = selectedMissileParent.transform.GetChild(2).gameObject;

        if (fullJet_missileParticle == null || outOfFuel_missileParticle == null) return;
    }

    void FixedUpdate()
    {
        if (gameController.startDelay && !crashed && !targetHit)
        {
            moveForward();

            //input controlling yaw & pitch 
            if (gameController.screenClickedOnPlay)
            {
                yaw += handleInput.x * yawAmount * Time.deltaTime / 1.2f;
                pitch += handleInput.y * yawAmount * Time.deltaTime / 1.2f;

                if (yawHudHorizontal > -90 && handleInput.x > 0 || yawHudHorizontal < 90 && handleInput.x < 0)
                { yawHudHorizontal += handleInput.x * yawAmount * Time.deltaTime; }

                yawHudVertical += handleInput.y * yawAmount * Time.deltaTime;

                //apply rotation
                if (handleInput.x != 0 || handleInput.y != 0)
                {
                    transform.Rotate(-1 * handleInput.y / 1.5f, handleInput.x / 1.5f, 0f, Space.Self);
                    missileLastRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                    hudYawUi.localRotation = Quaternion.Euler(Vector3.back * yawHudHorizontal 
                        + Vector3.right * yawHudVertical);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    transform.DORotate(missileLastRotation, 0.5f).SetEase(Ease.InOutQuad);
                    hudYawUi.DOLocalRotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InOutQuad)
                        .onComplete = hudYawRotateOnComplete;
                }
            }

            // setting missile nozzle particles when outOfFuel is true
            if (fuelManager.outOfFuel && Time.frameCount % 3 == 0)
            {
                fullJet_missileParticle.SetActive(false); 
                outOfFuel_missileParticle.SetActive(true);
            }
            if (!missileSources[0].isPlaying)
            {
                if (PlayerPrefs.GetInt("sfx") == 1) missileSources[0].Play();
            }
        }
        if (crashed || targetHit) // exploding effect call
        {
            if (!FxController.fxExplode)
            {
                if (crashed) Debug.Log("crashed"); fx.crashFx();
                if (targetHit) Debug.Log("targetHit"); fx.targetHitFx();
            }
            if (!explosion)
            {
                int[] explosions = { 0, 1, 2, 3 };
                int explode = explosions[Random.Range(0, explosions.Length)];
                if (PlayerPrefs.GetInt("sfx") == 1) explosionSources[explode].Play();

                explosion = true;
            }
            for (int i = 0; i < missileSources.Length; i++)
            {
                missileSources[i].Stop();
            }
        }
    }

    void moveForward()
    {
        float speedM = flySpeed + exSpeed;
        transform.position += transform.forward * speedM * Time.deltaTime;

        if (exSpeed != 0 && !speedUp)
        {
            if (PlayerPrefs.GetInt("sfx") == 1) missileSources[1].Play();
            speedUp = true;
        }

        if (exSpeed == 0 && speedUp) speedUp = false;
    }
    
    void hudYawRotateOnComplete()
    {
        yawHudHorizontal = yawHudVertical = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("crashColl"))
        {
            if (!crashed && !targetHit)
            {
                Debug.Log("crashed");
                crashed = true;

                scoreManager_inGame.addScore(110);
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

                scoreManager_inGame.addScore(340);
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

                scoreManager_inGame.addScore(370);
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

                AudioSource audioSource = collision.gameObject.GetComponent<AudioSource>();
                if (audioSource.isPlaying) audioSource.Stop();
                else return;

                scoreManager_inGame.addScore(480);
            }
        }
        rigidM.constraints = RigidbodyConstraints.FreezeAll;
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

                if (hitVal >= 3) { crashed = true; }
                scoreManager_inGame.addScore(130);
                StartCoroutine(roundHit());
            }
        }

        if (other.gameObject.CompareTag("ciwsRadar"))
        {
            if (PlayerPrefs.GetInt("sfx") == 1)
            {
                missileSources[2].Play();
                missileSources[3].Play();
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

            scoreManager_inGame.addScore(50);
            Debug.Log("returned");
        }
        if (other.gameObject.CompareTag("ciwsRadar"))
        {
            hitVal = 0;
            scoreManager_inGame.addScore(50);

            missileSources[2].Stop();
            missileSources[3].Stop();
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
