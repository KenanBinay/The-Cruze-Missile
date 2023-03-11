using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class menuManager : MonoBehaviour
{
    public int sceneId;
    float scoreBar;

    int selectedMissileNumb, levelVal, unlockLevel, savedMissile, lastSelectedMissileNumb;
    bool waitForReload, missileMenuOpened;
    string selectedMissileName;

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

        scoreBar = PlayerPrefs.GetFloat("sliderScore", 0);
        levelVal = PlayerPrefs.GetInt("level", 0);
        lastSelectedMissileNumb = PlayerPrefs.GetInt("lastSelectedMissile");

        menuSelectedMissiles[lastSelectedMissileNumb].SetActive(true);

        levelSlider.value = scoreBar;
        levelBase_text.text = levelVal.ToString();

        Debug.Log("last selected missile: " + lastSelectedMissileNumb);
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

            selectedMissileName = Enum.GetName(typeof(missileList), a);
            savedMissile = PlayerPrefs.GetInt(selectedMissileName);

            if (a != 0 && savedMissile == 1)
            {
                GameObject unlockedBox = missileSelected.transform.GetChild(3).gameObject;
                GameObject lockedBox = missileSelected.transform.GetChild(0).gameObject;
                GameObject levelTxt = missileSelected.transform.GetChild(4).gameObject;

                unlockedBox.SetActive(true);
                lockedBox.SetActive(false);
                levelTxt.SetActive(false);
            }
        }
    }

    public void selectMissile(string numbers = "0,0")
    {
        string[] split = numbers.Split(","[0]);
        int selectedMissile = int.Parse(split[0]);
        int levelCost = int.Parse(split[1]);

        Debug.Log("missile: " + selectedMissile.ToString() + " || required level: " + levelCost.ToString());

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

        //getting name from missileList by enumGetName
        if (selectedMissileNumb == 0) { savedMissile = 1; }
        else
        {
            selectedMissileName = Enum.GetName(typeof(missileList), selectedMissileNumb);
            savedMissile = PlayerPrefs.GetInt(selectedMissileName);
        }

        Debug.Log("Save: " + savedMissile);

        if (unlockLevel <= levelVal && savedMissile == 1) StartCoroutine(onMissileSelected());
        else
        {
            barOpened_missileMenu.SetActive(true);
            bar_missileMenu.SetActive(false);
        }
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

            PlayerPrefs.SetInt(selectedMissileName, 1);

            Debug.Log("UNLOCKED MISSILE: " + selectedMissileNumb);

            StartCoroutine(onMissileSelected());
        }
    }

    IEnumerator onMissileSelected()
    {
        PlayerPrefs.SetInt("lastSelectedMissile", selectedMissileNumb);

        barOpened_missileMenu.SetActive(false);
        bar_missileMenu.SetActive(true);

        missileMenuOpened = false;
        Debug.Log("selected missile: " + selectedMissileNumb);

        yield return new WaitForSeconds(0.3f);

        startMenu_canvas.SetActive(true);
        missileMenu_canvas.SetActive(false);

        for (int a = 0; a < 6; a++)
        {
            menuSelectedMissiles[a].SetActive(false);
        }

        menuSelectedMissiles[selectedMissileNumb].SetActive(true);
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
