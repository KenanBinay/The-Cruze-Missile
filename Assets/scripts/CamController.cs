using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamController : MonoBehaviour
{
    public GameObject Missile, diveEffect;
    private Camera cam;

    public float smoothSpeed;

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
            transform.DOMove(Missile.transform.position, 0);
            transform.DORotate(Missile.transform.eulerAngles, 0);

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
}
