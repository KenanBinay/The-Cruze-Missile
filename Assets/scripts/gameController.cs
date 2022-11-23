using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameController : MonoBehaviour
{
    public Camera mainCam;
    public Animator startAnim;
    public GameObject missileHud, missileBody;
    public TextMeshProUGUI altitute;

    public static bool startDelay;
    Vector3 crashAngle;

    float alt;
    RaycastHit hit;
    void Start()
    {
        crashAngle = new Vector3(0, 0, 0);
        alt = 3;
        missileHud.SetActive(false);
        startDelay = false;

        StartCoroutine(delayForStart());
    }

    void Update()
    {

        if (missileController.crashed == false && missileController.targetHit == false)
        {
            Ray rayFront = new Ray(missileBody.transform.position, Vector3.forward);
            Ray rayBack = new Ray(missileBody.transform.position, Vector3.back);
            Ray rayUp = new Ray(missileBody.transform.position, Vector3.up);
            Ray rayDown = new Ray(missileBody.transform.position, Vector3.down);
            Ray rayRight = new Ray(missileBody.transform.position, Vector3.right);
            Ray rayLeft = new Ray(missileBody.transform.position, Vector3.left);

            if (Physics.Raycast(rayFront, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance < 2) { crashAngle = Vector3.forward; missileController.crashed = true; }
            }
            if (Physics.Raycast(rayBack, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance < 2) { crashAngle = Vector3.back; missileController.crashed = true; }
            }
            if (Physics.Raycast(rayUp, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance < 2) { crashAngle = Vector3.up; missileController.crashed = true; }
            }
            if (Physics.Raycast(rayDown, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance < 2) { crashAngle = Vector3.down; missileController.crashed = true; }
            }
            if (Physics.Raycast(rayRight, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance < 2) { crashAngle = Vector3.right; missileController.crashed = true; }
            }
            if (Physics.Raycast(rayLeft, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance < 2) { crashAngle = Vector3.left; missileController.crashed = true; }
            }
        }

        //calculate main height to ground by using missiles position in unity
        altitute.text = missileBody.transform.position.y.ToString("#");
    }

    public static void targetHit(GameObject hudUi, GameObject controllerJoystick)
    {
        hudUi.SetActive(false);
        controllerJoystick.SetActive(false);
    }
    public static void crash(GameObject hudUi, GameObject controllerJoystick)
    {
        hudUi.SetActive(false);
        controllerJoystick.SetActive(false);
    }

    IEnumerator delayForStart()
    {
        yield return new WaitForSeconds(3f);
        mainCam.cullingMask = ~(1 << LayerMask.NameToLayer("missile"));
        startDelay = true;
        startAnim.enabled = false;
        missileHud.SetActive(true);
        Debug.Log("missileCam");
    }
}
