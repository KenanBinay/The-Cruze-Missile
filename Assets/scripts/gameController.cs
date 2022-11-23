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
        alt = 3;
        missileHud.SetActive(false);
        startDelay = false;

        StartCoroutine(delayForStart());
    }

    void Update()
    {
        //calculating distance to closest collision by using raycast physics
        if (missileController.crashed == false && missileController.targetHit == false)
        {
            Ray ray = new Ray(missileBody.transform.position, Vector3.up + Vector3.down + Vector3.forward + Vector3.right + Vector3.left + Vector3.back);
            Debug.DrawRay(missileBody.transform.position, Vector3.up * alt, Color.red);
            Debug.DrawRay(missileBody.transform.position, Vector3.down * alt, Color.red);
            Debug.DrawRay(missileBody.transform.position, Vector3.forward * alt, Color.red);
            Debug.DrawRay(missileBody.transform.position, Vector3.right * alt, Color.red);
            Debug.DrawRay(missileBody.transform.position, Vector3.left * alt, Color.red);
            Debug.DrawRay(missileBody.transform.position, Vector3.back * alt, Color.red);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "crashColl")
                {
                    if (hit.distance < 1) { missileController.crashed = true; Debug.Log("rayCollide"); }
                }
            }
        }
         

        //calculate main height to ground by using missiles position in unity
        altitute.text = missileBody.transform.position.y.ToString("#");
    }

    public static void targetHit(GameObject hudUi)
    {
        hudUi.SetActive(false);
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
