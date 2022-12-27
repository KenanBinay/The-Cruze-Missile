using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class helicopterController : MonoBehaviour
{
    public GameObject rotorBack, rotorM;
    Rigidbody rbHeli;
    bool crashedGround, missileHit;
    void Start()
    {
        rbHeli = gameObject.GetComponent<Rigidbody>();
        crashedGround = missileHit = false;
    }

    void Update()
    {
        if (!crashedGround && !missileHit)
        {
            CamController.aircraft_targetVector = transform.position;

            rotorBack.transform.Rotate(new Vector3(1000 * Time.deltaTime, 0, 0));
            rotorM.transform.Rotate(new Vector3(0, 500 * Time.deltaTime, 0));
        }
        if (missileHit && !crashedGround)
        {
            rotorBack.transform.Rotate(new Vector3(700 * Time.deltaTime, 0, 0));
            rotorM.transform.Rotate(new Vector3(0, 400 * Time.deltaTime, 0));

            transform.Rotate(new Vector3(0, 300 * Time.deltaTime, 0));
            transform.DORotate(new Vector3(25, transform.position.y, transform.position.z), 3f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("missileM"))
        {
            rbHeli.useGravity = true;
            missileHit = true;
            transform.DOPause();
        }
        if (collision.gameObject.CompareTag("crashColl")) { crashedGround = true; }
    }
}
