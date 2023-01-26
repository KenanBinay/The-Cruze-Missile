using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    public FxController fx;

    public Camera mainCam;
    public Animator startAnim;

    public GameObject missileHud, missileBody, warningUi, arrrowIndicator, tutoUi, joystickMain, ciwslockedUi, missionComplete_Ui, missionFailed_Ui, jet_main, onPlay_Ui, onPauseSlide_Ui;
    public Sprite iconPause, iconPlay;
    public Image iconPausePlay;

    public TextMeshProUGUI altitute, countdownTxt, missionTxt, missionDoneTxt, missionFailedTxt;

    public static bool startDelay, firstScreenTouch;

    bool countdownBool, startClick, gameover, paused;
    float rayLenght, countdownVal;
    RaycastHit hit;

    int missionCurrentVal;

    void Start()
    {
        rayLenght = 400;

        onPauseSlide_Ui.SetActive(false);
        missileHud.SetActive(false);
        warningUi.SetActive(false);
        joystickMain.SetActive(false);
        missionComplete_Ui.SetActive(false);
        missionFailed_Ui.SetActive(false);

        startDelay = startClick = gameover = firstScreenTouch = false;

        if (PlayerPrefs.GetInt("mission", 0) == 0) { PlayerPrefs.SetInt("mission", 1); }

        missionTxt.text = "MISSION " + PlayerPrefs.GetInt("mission", 0);
        missionCurrentVal = PlayerPrefs.GetInt("mission", 0);

        Debug.Log("mission: " + PlayerPrefs.GetInt("mission", 0));     

        StartCoroutine(delayForStart());
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !firstScreenTouch) { firstScreenTouch = true; }

        if (!missileController.crashed && !missileController.targetHit && startDelay)
        {
            if (!startClick & Input.GetMouseButtonDown(0)) { tutoUi.SetActive(false); }

            Debug.DrawRay(missileBody.transform.position, Vector3.down * rayLenght, Color.red);
            Ray rayDown = new Ray(missileBody.transform.position, Vector3.down);

            if (Physics.Raycast(rayDown, out hit))
            {
                if (hit.collider.tag == "crashColl")
                    if (hit.distance < 2) 
                    { 
                        missileController.crashed = true; 
                        if (!FxController.fxExplode) fx.crashFx(); 
                    }
                if (hit.collider.tag == "plane") 
                { 
                    missileController.crashed = true; 
                    if (!FxController.fxExplode) fx.crashFx(); Debug.Log("plane ray hit"); 
                }
            }
        }

        if (missileController.outside && !missileController.crashed && !missileController.targetHit) { giveWarning(); }
        else { warningUi.SetActive(false); countdownBool = false; }

        if (ciwsController.targetDetected) { ciwslockedUi.transform.DOScale(new Vector3(1, 1, 1), 0.2f); }
        else { ciwslockedUi.transform.DOScale(new Vector3(0, 2.5f, 1), 0.2f); }

        //on missile collide with target or crash
        if (missileController.targetHit && !gameover)
        {
            arrrowIndicator.SetActive(false);
            missileHud.SetActive(false);
            joystickMain.SetActive(false);

            missionComplete_Ui.SetActive(true);
            missionDoneTxt.text = "MISSION " + missionCurrentVal + " COMPLETE";
            gameover = true;
        }
        if (missileController.crashed && !gameover)
        {
            arrrowIndicator.SetActive(false);
            missileHud.SetActive(false);
            joystickMain.SetActive(false);
            onPlay_Ui.SetActive(false);

            missionFailed_Ui.SetActive(true);
            missionFailedTxt.text = "MISSION " + missionCurrentVal + " FAILED";
            gameover = true;
        }

        //calculate main height to ground by using missiles position in unity
        altitute.text = missileBody.transform.position.y.ToString("#");
    }
    void tweenCiwsUi() { ciwslockedUi.SetActive(false); }

    void giveWarning()
    {
        if (!countdownBool)
        {
            warningUi.SetActive(true);

            countdownVal = 6;
            countdownBool = true;
        }
        if (countdownBool)
        {
            countdownVal -= Time.deltaTime;
            int seconds = ((int)countdownVal);
            countdownTxt.text = "00:0" + seconds;
            if (seconds == 0) { missileController.crashed = true; fx.crashFx(); missileHud.SetActive(false); arrrowIndicator.SetActive(false); }
        }
    }

    IEnumerator delayForStart()
    {
        yield return new WaitForSeconds(3.5f);
      //  mainCam.cullingMask = ~(1 << LayerMask.NameToLayer("missile"));
        startDelay = true;
        startAnim.enabled = false;
        missileHud.SetActive(true);
        joystickMain.SetActive(true);
        Debug.Log("missileCam");

        yield return new WaitForSeconds(2f);
        Destroy(jet_main);
    }

    public void loadMission(int sceneId)
    {
        StartCoroutine(loadSceneAsync(sceneId));
    }

    public void pausePlay()
    {
        if (!missileController.crashed && !missileController.targetHit)
        {
            if (!paused)
            {
                onPauseSlide_Ui.SetActive(true);
                joystickMain.SetActive(false);
                paused = true;
                iconPausePlay.sprite = iconPlay;

                Debug.Log("pause");
                Time.timeScale = 0;
            }
            else if (paused)
            {
                onPauseSlide_Ui.SetActive(false);
                joystickMain.SetActive(true);
                paused = false;
                iconPausePlay.sprite = iconPause;

                Debug.Log("play");
                Time.timeScale = 1;
            }
        }
    }

    IEnumerator loadSceneAsync(int sceneId)
    {
        yield return new WaitForSeconds(0.2f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
