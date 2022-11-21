using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CamController : MonoBehaviour
{
    public GameObject Missile, endPos, target;

    public float smoothSpeed;

    void Start()
    {     
   
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
    }   
}
