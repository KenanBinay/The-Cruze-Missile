using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class helicopterController : MonoBehaviour
{
    public GameObject rotorBack, rotorM;
    Rigidbody rbHeli, rb_backRotor, rb_mainRotor;
    bool crashedGround, missileHit;
    void Start()
    {
        rbHeli = gameObject.GetComponent<Rigidbody>();

        crashedGround = missileHit = false;

        if (PlayerPrefs.GetInt("sfx") == 1)
            gameObject.GetComponentInChildren<AudioSource>().playOnAwake = true;
    }

    void Update()
    {
        if (Time.frameCount % 2 == 0 && rotorBack != null && rotorM != null)
        {
            if (!crashedGround && !missileHit)
            {
                rotorBack.transform.Rotate(new Vector3(1000 * Time.deltaTime, 0, 0));
                rotorM.transform.Rotate(new Vector3(0, 500 * Time.deltaTime, 0));
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("missileM"))
        {
            rbHeli.useGravity = true;
            if (rb_backRotor != null)
            {
                rb_backRotor.useGravity = true;
                rotorBack.GetComponent<Collider>().enabled = true;
            }         
            if (rb_mainRotor != null) 
            {
                rb_mainRotor.useGravity = true;
                rotorM.GetComponent<Collider>().enabled = true;
            }

            missileHit = true;
            if (transform != null) transform.DOPause();
        }
        if (collision.gameObject.CompareTag("crashColl")) { crashedGround = true; }
    }
}
