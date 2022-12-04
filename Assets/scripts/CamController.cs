using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamController : MonoBehaviour
{
    public GameObject Missile, endPos;

    public float smoothSpeed;

    RaycastHit hit;

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.back * 5, Color.blue);
        Ray ray = new Ray(transform.position, Vector3.back);

        if (!missileController.crashed && !missileController.targetHit && gameController.startDelay)
        {
            transform.DOMove(Missile.transform.position, 0);
            transform.DORotate(Missile.transform.eulerAngles, 0);
        }
        if (!missileController.crashed && missileController.targetHit)
        {
            if (targetController.target_type == 0)
            {
                transform.DOLookAt(targetController.staticTarget.transform.position, 0.5f);
                transform.DOMoveY(targetController.staticTarget.transform.position.y + 140, 2f);

                if (Physics.Raycast(ray, out hit))
                { if (hit.distance < 2) { transform.position += new Vector3(0, 0, 5); } }
            }
            if (targetController.target_type == 1)
            {
                transform.DOLookAt(propCarController.vehicle.transform.position, 0.5f);
                transform.DOMoveY(propCarController.vehicle.transform.position.y + 140, 2f);

                if (Physics.Raycast(ray, out hit))
                { if (hit.distance < 2) { transform.position += new Vector3(0, 0, 5); } }
            }
            if (targetController.target_type == 2)
            {
                transform.DOLookAt(propAircraftController.aircraft.transform.position, 0.5f);
                transform.DOMoveY(propAircraftController.aircraft.transform.position.y + 140, 2f);

                if (Physics.Raycast(ray, out hit))
                { if (hit.distance < 2) { transform.position += new Vector3(0, 0, 5); } }
            }
        }
        if (missileController.crashed && !missileController.targetHit)
        {
            transform.DOLookAt(Missile.transform.position, 0.5f);
            transform.DOMoveY(Missile.transform.position.y + 140, 2f);

            if (Physics.Raycast(ray, out hit))
            { if (hit.distance < 2) { transform.position += new Vector3(0, 0, 5); } }
        }
    }
}
