using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CamController : MonoBehaviour
{
    public GameObject Missile, endPos, target;

    public float smoothSpeed;

    float alt;
    RaycastHit hit;
    void Start()
    {
        alt = 200;
    }

    void FixedUpdate()
    {
        if (missileController.crashed == false && missileController.targetHit == false && gameController.startDelay)
        {
            transform.DOMove(Missile.transform.position, 0);
            transform.DORotate(Missile.transform.eulerAngles, 0);

        }
        if (missileController.targetHit)
        {
            transform.DOMove(endPos.transform.position, 2f);
            transform.DOLookAt(target.transform.position, 0.5f);
        }
        if (missileController.crashed)
        {
            transform.DOMove(new Vector3(Missile.transform.position.x, Missile.transform.position.y + 150, Missile.transform.position.z - 150), 2f);
            transform.DOLookAt(Missile.transform.position, 0.5f);
        }
    }   
}
