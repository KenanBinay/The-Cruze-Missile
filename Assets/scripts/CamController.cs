using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamController : MonoBehaviour
{
    public GameObject Missile, diveEffect;
    private Camera cam;

    private Vector3 _currentVelocity = Vector3.one;
    public float smoothSpeed;
    public Vector3 _offset;

    RaycastHit hit;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.back * 5, Color.blue);
        Ray ray = new Ray(transform.position, Vector3.back);

        if (!missileController.crashed && !missileController.targetHit && gameController.startDelay)
        {
            smoothFollow();

            float missileDegree = Missile.transform.eulerAngles.x;
            if (missileDegree > 40 && missileDegree < 90)
            { 
                if (!diveEffect.activeSelf) diveEffect.SetActive(true);
            }
            else
            { 
                if (diveEffect.activeSelf) diveEffect.SetActive(false); 
            }
        }
     
        if (missileController.crashed || missileController.targetHit)
        {
            if (diveEffect.activeSelf) diveEffect.SetActive(false);

            if (cam.fieldOfView < 120) { cam.fieldOfView += 6 * Time.deltaTime; }

            transform.DOLookAt(Missile.transform.position, 0.5f);
            transform.DOMoveY(Missile.transform.position.y + 140, 2f);

            if (Physics.Raycast(ray, out hit))
            { if (hit.distance < 2) { transform.position += new Vector3(0, 0, 5); } }
        }
    }

    void smoothFollow()
    {
        Vector3 missilePosition = Missile.transform.position + (Missile.transform.rotation * _offset);
        transform.position = Vector3.SmoothDamp(transform.position, missilePosition, ref _currentVelocity, smoothSpeed);
        transform.DOLookAt(new Vector3(Missile.transform.position.x, Missile.transform.position.y + 2, Missile.transform.position.z), 1f);
    }
}
