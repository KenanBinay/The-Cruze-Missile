using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class missileController : MonoBehaviour
{
    public static Vector2 handleInput;
    public Transform hudYawUi;
   
    public float flySpeed, yawAmount;
    float yaw, pitch, yawHudHorizontal, yawHudVertical;

    void Start()
    {
      
    }

    void Update()
    {
        if (gameController.startDelayed)
        {
            //move forward
            transform.position += transform.forward * flySpeed * Time.deltaTime;

            //input
            yaw += handleInput.x * yawAmount * Time.deltaTime;
            pitch += handleInput.y * yawAmount * Time.deltaTime;
            yawHudHorizontal += handleInput.x * yawAmount * Time.deltaTime;
            yawHudVertical += handleInput.y * yawAmount * Time.deltaTime;

            //apply rotation
            transform.localRotation = Quaternion.Euler(Vector3.up * yaw + Vector3.right * pitch);

            if (handleInput == Vector2.zero) { hudYawUi.DOLocalRotate(new Vector3(0, 0, 0), 1); yawHudVertical = yawHudHorizontal = 0; }
            else { hudYawUi.localRotation = Quaternion.Euler(1 * yawHudVertical, 0, -1f * yawHudHorizontal); }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("crashColl"))
        {
            Debug.Log("crashed");
        }
        if (collision.gameObject.CompareTag("target"))
        {
            Debug.Log("targetHit");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("outside"))
        {
            Debug.Log("outOfTheMap");
        }
    }
}
