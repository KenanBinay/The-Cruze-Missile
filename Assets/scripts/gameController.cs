using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameController : MonoBehaviour
{
    public Camera mainCam;
    public Animator startAnim;
    public GameObject missileHud, missileBody, warningUi_parent, arrrowIndicator;
    public TextMeshProUGUI altitute, countdownTxt;

    public static bool startDelay;

    bool countdownBool;
    float alt, countdownVal;
    RaycastHit hit;
    void Start()
    {
        alt = 400;
   
        missileHud.SetActive(false);
        warningUi_parent.SetActive(false);
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
        if (missileController.outside) { giveWarning(); }
        else { warningUi_parent.SetActive(false); arrrowIndicator.SetActive(true); countdownBool = false; }

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
    void giveWarning()
    {
        if (countdownBool == false)
        {
            warningUi_parent.SetActive(true);
            arrrowIndicator.SetActive(false);

            countdownVal = 6; 
            countdownBool = true; 
        }
        if (countdownBool) 
        {            
            countdownVal -= Time.deltaTime;
            int seconds = ((int)countdownVal);
            countdownTxt.text = "00:0" + seconds;
            if (seconds == 0) { missileController.crashed = true; missileHud.SetActive(false); arrrowIndicator.SetActive(false); }
        }   
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
