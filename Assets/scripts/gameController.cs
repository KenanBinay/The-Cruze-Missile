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

    float alt;
    RaycastHit hit;
    void Start()
    {
        alt = 400;
        missileHud.SetActive(false);
        startDelay = false;

        StartCoroutine(delayForStart());
    }

    void FixedUpdate()
    {
        if (missileController.crashed == false && missileController.targetHit == false)
        {
            Debug.DrawRay(missileBody.transform.position, Vector3.down * alt, Color.red);
            Ray rayDown = new Ray(missileBody.transform.position, Vector3.down);

            if (Physics.Raycast(rayDown, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance < 2) { missileController.crashed = true; }
                if (hit.collider.tag == "plane")
                    missileController.crashed = true;
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
