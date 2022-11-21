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

    void Update()
    {
        Ray ray = new Ray(missileBody.transform.position, -Vector3.up);
        Debug.DrawRay(missileBody.transform.position, Vector3.down * alt, Color.red);
        if(Physics.Raycast(ray,out hit))
        {
          //  float heightUpGround = hit.distance + 2.5f;
            altitute.text = hit.distance.ToString("#");
        }
        else { altitute.text = "!!!"; }
        
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
