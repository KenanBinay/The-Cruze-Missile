using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    public FxController script_fx;
    private scoreManager_inGame scoreManager;
    [SerializeField] public ciwsSpawner script_ciwsSpawner;

    [SerializeField] public Camera mainCam;
    [SerializeField] public Animator startAnim, levelEndUiAnim;

    [SerializeField]
    public GameObject missileHud, missileBody, warningUi, tutoUi,
        joystickMain, ciwslockedUi, missionComplete_Ui, missionFailed_Ui, jet_main, pauseButton_Ui,
        fuelBar_Ui, missionInfo_Ui, onPauseSlide_Ui;

    [SerializeField] public Sprite iconPause, iconPlay;
    [SerializeField] public Image iconPausePlay;
    [SerializeField] public Color safe, mid, critical;
    [SerializeField] public Slider levelSlider;
    Color targetColor;


    [SerializeField]
    public TextMeshProUGUI altitute, countdownTxt, missionTxt, missionDoneTxt,
        missionFailedTxt, level_txt, scoreVal_txt;

    [SerializeField] public TextMeshPro alt_txt, time_txt;

    public static bool startDelay, screenClickedOnPlay, timeScoreGiven, speedUp_missile;
    public static float missionTime, gainedScoreInLevel;

    bool countdownBool, gameover, paused, waitForReload, compeleted_endUiAnim;
    float rayLenght, countdownVal;

    RaycastHit hit;

    int missionCurrentVal, levelValue;

    Vector3 targetLine;
    private void Awake()
    {
        if (Application.isMobilePlatform)
        {
            Application.targetFrameRate = -1;
            Debug.unityLogger.logEnabled = false;
            QualitySettings.vSyncCount = 0;
        }

        DOTween.KillAll();
    }

    void Start()
    {
        scoreManager = GetComponent<scoreManager_inGame>();

        rayLenght = 600;
        gainedScoreInLevel = missionTime = 0;

        missileHud.SetActive(false);
        joystickMain.SetActive(false);

        startDelay = screenClickedOnPlay = gameover = false;

        if (PlayerPrefs.GetInt("mission", 0) == 0) { PlayerPrefs.SetInt("mission", 1); }

        missionTxt.text = "TARGET " + PlayerPrefs.GetInt("mission", 0);
        missionCurrentVal = PlayerPrefs.GetInt("mission", 0);
        levelValue = PlayerPrefs.GetInt("level", 0);
        level_txt.text = levelValue.ToString();
        levelSlider.value = PlayerPrefs.GetFloat("sliderScore", 0);

        Debug.Log("mission: " + PlayerPrefs.GetInt("mission", 0));     

        StartCoroutine(delayForStart());
    }

    private void loadMapAssets()
    {  
        script_ciwsSpawner.ciwsSpawn();
    }

    void Update()
    {
        if (!missileController.crashed && !missileController.targetHit && startDelay)
        {
            if (!screenClickedOnPlay & Input.GetMouseButtonDown(0))
            { tutoUi.SetActive(false); screenClickedOnPlay = true; }

            Debug.DrawRay(missileBody.transform.position, Vector3.down * rayLenght, Color.red);
            Ray rayDown = new Ray(missileBody.transform.position, Vector3.down);

            if (Physics.Raycast(rayDown, out hit))
            {
                if (hit.collider.tag == "crashColl")
                {
                    if (hit.distance < 2)
                    {
                        missileController.crashed = true;
                        if (!FxController.fxExplode) script_fx.crashFx();
                    }
                    if (hit.distance < 50)
                    {
                        missileController.exSpeed = 15;
                        gameController.speedUp_missile = true;
                    }
                    else if (hit.distance > 50) gameController.speedUp_missile = false;
                }
                if (hit.collider.tag == "plane")
                {
                    missileController.crashed = true;
                    if (!FxController.fxExplode) script_fx.crashFx(); Debug.Log("plane ray hit");
                }
            }

            missionTime += Time.deltaTime;
            time_txt.text = missionTime.ToString("0.00");
        }

        if (missileController.outside && !missileController.crashed && !missileController.targetHit) { giveWarning(); }
        else { warningUi.SetActive(false); countdownBool = false; }

        if (ciwsController.targetDetected) { ciwslockedUi.transform.DOLocalMoveY(520, 0.25f); }
        else { ciwslockedUi.transform.DOLocalMoveY(1200, 0.25f); }

        //on missile collide with target or crash
        if (missileController.targetHit && !gameover)
        {
            missileHud.SetActive(false);
            joystickMain.SetActive(false);
            pauseButton_Ui.SetActive(false);
            fuelBar_Ui.SetActive(false);
            missionInfo_Ui.SetActive(false);

            missionComplete_Ui.SetActive(true);
            missionDoneTxt.text = "TARGET " + missionCurrentVal + " DESTROYED";
            gameover = true;

            mainCam.cullingMask -= (1 << LayerMask.NameToLayer("missile"));

            //giving time score
            if (!timeScoreGiven)
            {
                if (missionTime >= 40) scoreManager_inGame.addScore(70);
                if (missionTime >= 30 && missionTime < 40) scoreManager_inGame.addScore(170);
                if (missionTime >= 25 && missionTime < 30) scoreManager_inGame.addScore(200);
                if (missionTime >= 15 && missionTime < 25) scoreManager_inGame.addScore(360);
                if (missionTime < 15) scoreManager_inGame.addScore(410);

                timeScoreGiven = true;
            }

            StartCoroutine(levelEndScoreValueSmoothSet());
        }
        if (missileController.crashed && !gameover)
        {
            missileHud.SetActive(false);
            joystickMain.SetActive(false);
            pauseButton_Ui.SetActive(false);
            fuelBar_Ui.SetActive(false);
            missionInfo_Ui.SetActive(false);

            missionFailed_Ui.SetActive(true);
            missionFailedTxt.text = "TARGET " + missionCurrentVal + " FAILED";

            gameover = true;

            mainCam.cullingMask -= (1 << LayerMask.NameToLayer("missile"));
        }

        if (Time.frameCount % 3 == 0)
        {
            //calculate main height to ground by using missiles position in unity & color change
            alt_txt.text = missileBody.transform.position.y.ToString("ALT " + "#");
            alt_txt.color = Color.Lerp(alt_txt.color, targetColor, 0.1f);
            if (missileBody.transform.position.y <= 230 && targetColor != safe) targetColor = safe;
            if (missileBody.transform.position.y > 230 && missileBody.transform.position.y <= 350 && targetColor != mid) targetColor = mid;
            if (missileBody.transform.position.y > 400 && targetColor != critical) targetColor = critical;
        }

        if (compeleted_endUiAnim)
        {
            if (levelSlider.value == levelSlider.maxValue)
            {
                levelEndUiAnim.SetTrigger("levelUp");

                levelValue++;
                level_txt.text = levelValue.ToString();
                levelSlider.value = 0;

                PlayerPrefs.SetInt("level", levelValue);
            }
            if (levelSlider.value != levelSlider.maxValue && scoreManager_inGame.sliderScore > 0)
            {
                levelSlider.value += 20;
                scoreManager_inGame.sliderScore -= 20;
                PlayerPrefs.SetFloat("sliderScore", levelSlider.value);
            }
        }
    }

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
            if (seconds == 0) 
            { missileController.crashed = true; script_fx.crashFx(); missileHud.SetActive(false); }
        }
    }

    IEnumerator delayForStart()
    {
        yield return new WaitForSeconds(3.5f);

        mainCam.cullingMask += (1 << LayerMask.NameToLayer("missile"));
        loadMapAssets();
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
        if (!waitForReload)
        {
            if (Time.timeScale != 1) Time.timeScale = 1;          
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
            if (!operation.isDone) { waitForReload = true;}
        }  
    }
    public void loadMenu(int sceneId)
    {
        if (!waitForReload)
        {
            if (Time.timeScale != 1) Time.timeScale = 1;
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
            if (!operation.isDone) { waitForReload = true; }
        }
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

  
    IEnumerator levelEndScoreValueSmoothSet()
    {
        scoreVal_txt.text = "+" + gainedScoreInLevel;
        PlayerPrefs.SetFloat("sliderScore", scoreManager_inGame.sliderScore);

        yield return new WaitForSeconds(2.30f);
        compeleted_endUiAnim = true;
    }
}
