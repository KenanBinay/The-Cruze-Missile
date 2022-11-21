using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class missileController : MonoBehaviour
{
    Vector3 normal;
    public static Vector2 handleInput;
    public Transform hudYawUi;
    public GameObject mainHudUi;
    Rigidbody rigidM;

    public float flySpeed, yawAmount;
    float yaw, pitch, yawHudHorizontal, yawHudVertical;

    public static bool crashed, targetHit;

    void Start()
    {
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
            crashed = true;
            rigidM.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log("crashed");
        }
        if (collision.gameObject.CompareTag("target"))
        {
            targetHit = true;
            rigidM.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log("targetHit");

            gameController.targetHit(mainHudUi);
        }

        normal = collision.contacts[0].normal;

        Debug.Log(normal);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("outside"))
        {
            Debug.Log("outOfTheMap");
        }
    }
}
