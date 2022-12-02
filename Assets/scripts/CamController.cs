using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamController : MonoBehaviour
{
    public GameObject Missile, endPos, target;

    public float smoothSpeed;

    void FixedUpdate()
    {
        if (!missileController.crashed && !missileController.targetHit && gameController.startDelay)
        {        
            transform.DOMove(Missile.transform.position, 0);
            transform.DORotate(Missile.transform.eulerAngles, 0);
        }
        if (!missileController.crashed && missileController.targetHit)
        {
            if (targetController.target_type == 0)
            {
                transform.DOLookAt(target.transform.position, 0.5f);
                transform.DOMove(endPos.transform.position, 2f);
            }
            if (targetController.target_type == 1)
            {
                transform.DOLookAt(propCarController.vehicle.transform.position, 0.5f);
                transform.DOMove(new Vector3(propCarController.vehicle.transform.position.x, propCarController.vehicle.transform.position.y + 100, propCarController.vehicle.transform.position.z - 10), 2f);
            }
        }
        if (missileController.crashed && !missileController.targetHit)
        {
            if (missileController.normal.x < -0.5f) { transform.DOMove(new Vector3(Missile.transform.position.x, Missile.transform.position.y + 150, Missile.transform.position.z + 150), 2f); }
            else { transform.DOMove(new Vector3(Missile.transform.position.x, Missile.transform.position.y + 150, Missile.transform.position.z - 150), 2f); }
            transform.DOLookAt(Missile.transform.position, 0.5f);
        }
    }
}
