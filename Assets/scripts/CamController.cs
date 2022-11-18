using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public GameObject targetMissile;
    public Animator startAnim;

    public float smoothSpeed;

    bool startDelayed;
    void Start()
    {     
        startDelayed = false;

        StartCoroutine(startDelay());
    }

    void FixedUpdate()
    {
        if (startDelayed)
        {
            Vector3 desiredPosition = new Vector3(targetMissile.transform.position.x, targetMissile.transform.position.y, targetMissile.transform.position.z);
            Vector3 SmoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = new Vector3(targetMissile.transform.position.x, targetMissile.transform.position.y, targetMissile.transform.position.z);
            transform.Rotate(new Vector3(targetMissile.transform.rotation.x, targetMissile.transform.rotation.y, targetMissile.transform.rotation.z));
        }
    }

    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(3f);
        startDelayed = true;
        startAnim.enabled = false;
        Debug.Log("missileCam");
    }
}
