using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class missileController : MonoBehaviour
{
    Vector3 normal;
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
        if (gameController.startDelay && crashed == false && targetHit == false)
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
            if (crashed == false && targetHit == false)
            {
                crashed = true;
                gameController.crash(mainHudUi, controllerJoystick);
                Debug.Log("crashed");
            }
        }
        if (collision.gameObject.CompareTag("target"))
        {
            if (targetHit == false && crashed == false)
            {
                targetHit = true;
                gameController.targetHit(mainHudUi, controllerJoystick);
                Debug.Log("targetHit");
            }
        }
        if (collision.gameObject.CompareTag("vehicleTarget"))
        {
            if (targetHit == false && crashed == false)
            {
                targetHit = true;
                gameController.targetHit(mainHudUi, controllerJoystick);
                collision.gameObject.transform.DOPause();
                Debug.Log("vehicleTargetHit");
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
        if (other.gameObject.CompareTag("outside") && crashed == false)
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
