using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class menuManager : MonoBehaviour
{
    public int sceneId;
    float scoreBar;

    int selectedMissileNumb, levelVal, unlockLevel;
    bool waitForReload, missileMenuOpened;

    [Header("Slider")]
    [SerializeField] private Slider levelSlider, loadingSlider;
    [Header("")]

    public Animator bottomStart;
    public TextMeshProUGUI levelBase_text;

    [SerializeField]
    public GameObject startMenu_canvas, missileMenu_canvas, missileSelectionBoxes
        , bar_missileMenu, barOpened_missileMenu, top_mainCanvas, middile_mainCanvas, bottom_mainCanvas,
        loadingUi;

    [SerializeField] GameObject[] menuSelectedMissiles;

    private void Awake()
    {
        DOTween.KillAll();
    }

    void Start()
    {
        sceneId = 1;

        if (selectedMissileNumb == 0) menuSelectedMissiles[selectedMissileNumb].SetActive(true);
        if (selectedMissileNumb == 1) menuSelectedMissiles[selectedMissileNumb].SetActive(true);
        if (selectedMissileNumb == 2) menuSelectedMissiles[selectedMissileNumb].SetActive(true);
        if (selectedMissileNumb == 3) menuSelectedMissiles[selectedMissileNumb].SetActive(true);
        if (selectedMissileNumb == 4) menuSelectedMissiles[selectedMissileNumb].SetActive(true);
        if (selectedMissileNumb == 5) menuSelectedMissiles[selectedMissileNumb].SetActive(true);

        scoreBar = PlayerPrefs.GetFloat("sliderScore", 0);
        levelVal = PlayerPrefs.GetInt("level", 0);

        levelSlider.value = scoreBar;
        levelBase_text.text = levelVal.ToString();
    }

    public void loadMission()
    {
        if (!waitForReload)
        {
            bottomStart.SetTrigger("startClick");

            loadingUi.SetActive(true);
            startMenu_canvas.SetActive(false);
            missileMenu_canvas.SetActive(false);
            top_mainCanvas.SetActive(false);
            middile_mainCanvas.SetActive(false);
            bottom_mainCanvas.SetActive(false);

            StartCoroutine(loadLevelAsync(sceneId));              
        }
    }

    public void loadMissileChangeMenu()
    {
        missileMenuOpened = true;
        startMenu_canvas.SetActive(false);
        missileMenu_canvas.SetActive(true);
        bar_missileMenu.SetActive(true);
        barOpened_missileMenu.SetActive(false);

        for (int a = 0; a < 6; a++)
        {
            GameObject missileSelected = missileSelectionBoxes.transform.Find("missileBox_" + a)
                .gameObject;
            GameObject selectIndicator = missileSelected.transform.GetChild(1).gameObject;
                selectIndicator.SetActive(false);
        }
    }

    public void selectMissile(string numbers = "0,0")
    {
        string[] split = numbers.Split(","[0]);
        int selectedMissile = int.Parse(split[0]);
        int levelCost = int.Parse(split[1]);

        Debug.Log("missile: " + selectedMissile.ToString() + " || needed level: " + levelCost.ToString());

        for (int a = 0; a < 6; a++)
        {
            if (menuSelectedMissiles[a] != null) menuSelectedMissiles[a].SetActive(false);

            GameObject missileSelected = missileSelectionBoxes.transform.Find("missileBox_" + a)
                .gameObject;
            GameObject selectIndicator = missileSelected.transform.GetChild(1).gameObject;

            if (a == selectedMissile)
                selectIndicator.SetActive(true);

            if (a != selectedMissile && selectIndicator.activeSelf)
                selectIndicator.SetActive(false);
        }

        unlockLevel = levelCost;
        selectedMissileNumb = selectedMissile;

        StartCoroutine(onMissileSelected());
    }

    public void unlockMissile_byLevel()
    {      
        if (unlockLevel <= levelVal)
        {
            GameObject missileSelected = missileSelectionBoxes.transform.Find("missileBox_" +
          selectedMissileNumb).gameObject;

            GameObject unlockedBox = missileSelected.transform.GetChild(3).gameObject;
            GameObject lockedBox = missileSelected.transform.GetChild(0).gameObject;
            GameObject levelTxt = missileSelected.transform.GetChild(4).gameObject;

            unlockedBox.SetActive(true);

            lockedBox.SetActive(false);
            levelTxt.SetActive(false);

            Debug.Log("UNLOCKED MISSILE: " + selectedMissileNumb);
        }
    }

    IEnumerator onMissileSelected()
    {
        if (selectedMissileNumb == 0)
        {
            missileMenuOpened = false;

            yield return new WaitForSeconds(0.3f);

            startMenu_canvas.SetActive(true);
            missileMenu_canvas.SetActive(false);

            menuSelectedMissiles[selectedMissileNumb].SetActive(true);
        }

        if (selectedMissileNumb != 0)
        {
            bar_missileMenu.SetActive(false);
            barOpened_missileMenu.SetActive(true);
        }
    }

    IEnumerator loadLevelAsync(int sceneId)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneId);
        if (!loadOperation.isDone) { waitForReload = true; }

        while (!loadOperation.isDone)
        {
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingSlider.value = progressValue;
            yield return null;
        }
    }
}
