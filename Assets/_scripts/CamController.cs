using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamController : MonoBehaviour
{
    public GameObject Missile, diveEffect, speedEffect;
    private Camera cam;

    private Vector3 _currentVelocity = Vector3.one;
    public Vector3 _offset;

    public float smoothSpeed, smoothLook;

    RaycastHit hit;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        //  Debug.DrawRay(transform.position, Vector3.back * 5, Color.blue);
        // Ray ray = new Ray(transform.position, Vector3.back);

        if (!missileController.crashed && !missileController.targetHit && gameController.startDelay)
        {
            smoothFollow();

            float missileDegree = Missile.transform.eulerAngles.x;

            if (missileDegree > 40 && missileDegree < 90)
            {
                if (!diveEffect.activeSelf) diveEffect.SetActive(true); missileController.exSpeed = 10;
            }
            else
            { 
                if (diveEffect.activeSelf) diveEffect.SetActive(false);

                if (!gameController.speedUp_missile)
                    missileController.exSpeed = 0;
            }

            if (gameController.speedUp_missile) speedEffect.SetActive(true);
            else speedEffect.SetActive(false);
        }
        if (missileController.crashed || missileController.targetHit)
        {
            if (diveEffect.activeSelf) diveEffect.SetActive(false);
            if (speedEffect.activeSelf) speedEffect.SetActive(false);

            if (cam.fieldOfView < 120) { cam.fieldOfView += 6 * Time.deltaTime; }

            transform.DOLookAt(Missile.transform.position, 0.5f);
            transform.DOMoveY(Missile.transform.position.y + 140, 2f);

            //     if (Physics.Raycast(ray, out hit))
            //     { if (hit.distance < 2) { transform.position += new Vector3(0, 0, 5); } }
        }
    }

    void smoothFollow()
    {
        Vector3 missilePosition = Missile.transform.position + (Missile.transform.rotation * _offset);
        transform.position = Vector3.SmoothDamp(transform.position, missilePosition, ref _currentVelocity, smoothSpeed);
        //   transform.LookAt(new Vector3(Missile.transform.position.x, Missile.transform.position.y + 2, Missile.transform.position.z), Missile.transform.up);

        var targetRotation = Quaternion.LookRotation(new Vector3(Missile.transform.position.x, Missile.transform.position.y + 2, Missile.transform.position.z) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothLook * Time.deltaTime);
    }
}
