using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class missileController : MonoBehaviour
{
    public static Vector3 normal;
    public static Vector2 handleInput;
    public Transform hudYawUi;
    public GameObject mainHudUi, controllerJoystick, waypointArrow, warningUi;
    Rigidbody rigidM;

    public float flySpeed, yawAmount;
    float yaw, pitch, yawHudHorizontal, yawHudVertical;

    public static bool crashed, targetHit, outside;

    void Start()
    {
        outside = false;
        rigidM = gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (gameController.startDelay && !crashed && !targetHit)
        {
            //move forward
            transform.position += transform.forward * flySpeed * Time.deltaTime;

            //input
            yaw += handleInput.x * yawAmount * Time.deltaTime;
            pitch += handleInput.y * yawAmount * Time.deltaTime;

            if (yawHudHorizontal <= -90 && handleInput.x < 0 || yawHudHorizontal >= 90 && handleInput.x > 0) { }
            else { yawHudHorizontal += handleInput.x * yawAmount * Time.deltaTime; }
           
            yawHudVertical += handleInput.y * yawAmount * Time.deltaTime;

            //apply rotation
            transform.localRotation = Quaternion.Euler(Vector3.up * yaw + Vector3.right * pitch);

            if (handleInput == Vector2.zero) { hudYawUi.DOLocalRotate(new Vector3(yawHudVertical, 0, 0), 1); yawHudHorizontal = 0; }
            else { hudYawUi.localRotation = Quaternion.Euler(Vector3.back * yawHudHorizontal + Vector3.right * yawHudVertical); }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("crashColl"))
        {
            if (!crashed && !targetHit)
            {
                crashed = true;
                Debug.Log("crashed");
                gameController.crash(mainHudUi, controllerJoystick);
            }           
        }
        if (collision.gameObject.CompareTag("target"))
        {
            if (!crashed && !targetHit)
            {
                targetHit = true;
                Debug.Log("targetHit");
                gameController.targetHit(mainHudUi, controllerJoystick);
            }
        }
        if (collision.gameObject.CompareTag("vehicleTarget"))
        {
            if (!crashed && !targetHit)
            {
                targetHit = true;
                Debug.Log("vehicleTargetHit");
                collision.gameObject.transform.DOPause();
                gameController.targetHit(mainHudUi, controllerJoystick);
            }
        }

        rigidM.constraints = RigidbodyConstraints.FreezeAll;
        normal = collision.contacts[0].normal;
        Debug.Log(normal);

        if (normal.x > 0.5f) { Debug.Log("left"); }
        if (normal.x < -0.5f) { Debug.Log("right"); }
        if (normal.y > 0.5f) { Debug.Log("front"); }
        if (normal.y < -0.5f) { Debug.Log("back"); }
        if (normal.z > 0.5f) { Debug.Log("up"); }
        if (normal.z < -0.5f) { Debug.Log("down"); }
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
    }
}
