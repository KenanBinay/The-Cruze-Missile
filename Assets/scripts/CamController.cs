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
            Ray rayFront = new Ray(transform.position, Vector3.forward);
            Ray rayBack = new Ray(transform.position, Vector3.back);
            Ray rayUp = new Ray(transform.position, Vector3.up);
            Ray rayDown = new Ray(transform.position, Vector3.down);
            Ray rayRight = new Ray(transform.position, Vector3.right);
            Ray rayLeft = new Ray(transform.position, Vector3.left);

            if (Physics.Raycast(rayFront, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance > 100) 
                    {
                        transform.DOMove(new Vector3(Missile.transform.position.x, Missile.transform.position.y + 150, Missile.transform.position.z - 150), 2f);
                        transform.DOLookAt(Missile.transform.position, 0.5f);
                    }
            }
            if (Physics.Raycast(rayBack, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance > 100)
                    {
                        transform.DOMove(new Vector3(Missile.transform.position.x, Missile.transform.position.y + 150, Missile.transform.position.z - 150), 2f);
                        transform.DOLookAt(Missile.transform.position, 0.5f);
                    }
            }
            if (Physics.Raycast(rayUp, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance > 100)
                    {
                        transform.DOMove(new Vector3(Missile.transform.position.x, Missile.transform.position.y + 150, Missile.transform.position.z - 150), 2f);
                        transform.DOLookAt(Missile.transform.position, 0.5f);
                    }
            }
            if (Physics.Raycast(rayDown, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance > 100)
                    {
                        transform.DOMove(new Vector3(Missile.transform.position.x, Missile.transform.position.y + 150, Missile.transform.position.z - 150), 2f);
                        transform.DOLookAt(Missile.transform.position, 0.5f);
                    }
            }
            if (Physics.Raycast(rayRight, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance > 100)
                    {
                        transform.DOMove(new Vector3(Missile.transform.position.x, Missile.transform.position.y + 150, Missile.transform.position.z - 150), 2f);
                        transform.DOLookAt(Missile.transform.position, 0.5f);
                    }
            }
            if (Physics.Raycast(rayLeft, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance > 100)
                    {
                        transform.DOMove(new Vector3(Missile.transform.position.x, Missile.transform.position.y + 150, Missile.transform.position.z - 150), 2f);
                        transform.DOLookAt(Missile.transform.position, 0.5f);
                    }
            }
           
        }
    }   
}
