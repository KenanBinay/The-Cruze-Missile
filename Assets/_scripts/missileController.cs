using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class missileController : MonoBehaviour
{
    public FxController fx;

    public static Vector3 normal;
    public static Vector2 handleInput;
    public Transform hudYawUi, cam;
    [SerializeField] public GameObject mainHudUi, warningUi, hitFlash_image, hitEffect_particle;
    [SerializeField] public GameObject[] fullJet_missileParticles, outOfFuel_missileParticles;
    Rigidbody rigidM;

    public float flySpeed, yawAmount;
    public static float yaw, pitch, yawHudHorizontal, yawHudVertical;
    bool missileYawPitchSet, clickCheckMissileRot;

    public static bool crashed, targetHit, outside, ciwsHit;
    public static int hitVal;

    Vector3 missileLastRotation;

    void Start()
    {
        hitVal = 0;
        pitch = yaw = 0f;
        handleInput = Vector2.zero;

        outside = ciwsHit = crashed = targetHit = missileYawPitchSet = clickCheckMissileRot = false;
        rigidM = gameObject.GetComponent<Rigidbody>();

        for (int i = 0; i < fullJet_missileParticles.Length; i++)
        {
            fullJet_missileParticles[i].SetActive(true);
            outOfFuel_missileParticles[i].SetActive(false);
        }
    }

    void FixedUpdate()
    {
        if (gameController.startDelay && !crashed && !targetHit)
        {
            moveForward();

            //input controlling yaw & pitch 
            yaw += handleInput.x * yawAmount * Time.deltaTime / 1.2f;
            pitch += handleInput.y * yawAmount * Time.deltaTime / 1.2f;

            if (yawHudHorizontal <= -90 && handleInput.x < 0 || yawHudHorizontal >= 90 && handleInput.x > 0) { }
            else { yawHudHorizontal += handleInput.x * yawAmount * Time.deltaTime; }

            yawHudVertical += handleInput.y * yawAmount * Time.deltaTime;

            //apply rotation
            if (handleInput.x != 0 || handleInput.y != 0)
            {
                transform.Rotate(-1 * handleInput.y / 1.5f, handleInput.x / 1.5f, 0f, Space.Self);
                missileLastRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                hudYawUi.localRotation = Quaternion.Euler(Vector3.back * yawHudHorizontal + Vector3.right * yawHudVertical);
            }
            if (Input.GetMouseButtonUp(0))
            {
                transform.DORotate(missileLastRotation, 0.5f).SetEase(Ease.InOutQuad);
                hudYawUi.DOLocalRotate(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InOutQuad).onComplete = hudYawRotateOnComplete;
            }

            // setting missile nozzle particles when outOfFuel is true
            if (fuelManager.outOfFuel && Time.frameCount % 3 == 0)
            {
                fullJet_missileParticles[0].SetActive(false); outOfFuel_missileParticles[0].SetActive(true);
            }
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

    void moveForward() 
    {
         transform.position += transform.forward * flySpeed * Time.deltaTime; 
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
