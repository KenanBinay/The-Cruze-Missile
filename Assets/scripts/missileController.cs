using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class missileController : MonoBehaviour
{
    public static Vector2 handleInput;
   
    public float flySpeed, yawAmount;
    float yaw, pitch;

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

            //apply rotation
            transform.localRotation = Quaternion.Euler(Vector3.up * yaw + Vector3.right * pitch);
        }
    }
}
