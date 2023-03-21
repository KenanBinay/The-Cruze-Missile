using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menuManager : MonoBehaviour
{
    public int sceneId;
    float scoreBar;

    int selectedMissileNumb, levelVal, unlockLevel, missileSave, lastSelectedMissileNumb, targetVal;
    bool waitForReload, missileMenuOpened;
    string selectedMissileName;

    [Header("Slider")]
    [SerializeField] private Slider levelSlider, loadingSlider;
    [Header("")]

    public Animator bottomStart;
    public TextMeshProUGUI levelBase_text, targetVal_text;

    [SerializeField]
    public GameObject startMenu_canvas, missileMenu_canvas, missileSelectionBoxes
        , bar_missileMenu, barOpened_missileMenu, top_mainCanvas, middile_mainCanvas, bottom_mainCanvas,
        loadingUi, missileMenu_infoUi;

    private GameObject selectedMissile_missileInfo;

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
        targetVal = PlayerPrefs.GetInt("mission", 0);
        selectedMissileNumb = lastSelectedMissileNumb;

        for (int a = 0; a < 6; a++)
        { menuSelectedMissiles[a].SetActive(false); }

        menuSelectedMissiles[lastSelectedMissileNumb].SetActive(true);

        levelSlider.value = scoreBar;
        levelBase_text.text = levelVal.ToString();
        targetVal_text.text = "TARGET " + targetVal.ToString();

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
        lastSelectedMissileNumb = PlayerPrefs.GetInt("lastSelectedMissile");

        missileMenuOpened = true;
        startMenu_canvas.SetActive(false);
        missileMenu_canvas.SetActive(true);
        bar_missileMenu.SetActive(true);
        barOpened_missileMenu.SetActive(false);

        for (int a = 0; a < 6; a++)
        {
            GameObject missileSelected = missileSelectionBoxes.transform.Find("missileBox_" + a)
                .gameObject;
            GameObject selectIndicator = missileSelected.transform.GetChild(2).gameObject;
            selectIndicator.SetActive(false);         

            GameObject selectedBox = missileSelected.transform.GetChild(3).gameObject;
            GameObject unlockedBox = missileSelected.transform.GetChild(4).gameObject;
            GameObject lockedBox = missileSelected.transform.GetChild(1).gameObject;
            GameObject levelTxt = missileSelected.transform.GetChild(5).gameObject;

            selectedMissileName = Enum.GetName(typeof(missileList), a);
            missileSave = PlayerPrefs.GetInt(selectedMissileName);

            if (missileSave == 1 && a != lastSelectedMissileNumb)
            {
                unlockedBox.SetActive(true);
                selectedBox.SetActive(false);
                lockedBox.SetActive(false);
                levelTxt.SetActive(false);
            }
            if (a == lastSelectedMissileNumb)
            {
                selectedBox.SetActive(true);
                unlockedBox.SetActive(false);
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
            GameObject selectIndicator = missileSelected.transform.GetChild(2).gameObject;

            if (a == selectedMissile)
                selectIndicator.SetActive(true); 

            if (a != selectedMissile && selectIndicator.activeSelf)
                selectIndicator.SetActive(false); 
        }

        unlockLevel = levelCost;
        selectedMissileNumb = selectedMissile;

        //getting name from missileList by enumGetName
        if (selectedMissileNumb == 0) missileSave = 1;
        else
        {
            selectedMissileName = Enum.GetName(typeof(missileList), selectedMissileNumb);
            missileSave = PlayerPrefs.GetInt(selectedMissileName);
        }

        Debug.Log("Save: " + missileSave);

        if (missileSave == 1) StartCoroutine(onMissileSelected());
        else
        {
            barOpened_missileMenu.SetActive(true);
            bar_missileMenu.SetActive(false);

            if (selectedMissile_missileInfo != null) selectedMissile_missileInfo.SetActive(false);

            GameObject missiles = missileMenu_infoUi.transform.Find("missiles").gameObject;
            selectedMissile_missileInfo = missiles.transform.GetChild(selectedMissileNumb).gameObject;

            selectedMissile_missileInfo.SetActive(true);
        }
    }

    public void unlockMissile_byLevel()
    {      
        if (unlockLevel <= levelVal)
        {
            GameObject missileSelected = missileSelectionBoxes.transform.Find("missileBox_" +
          selectedMissileNumb).gameObject;
            GameObject unlockedBox = missileSelected.transform.GetChild(4).gameObject;
            GameObject lockedBox = missileSelected.transform.GetChild(1).gameObject;
            GameObject levelTxt = missileSelected.transform.GetChild(5).gameObject;

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
        int lastSelected = PlayerPrefs.GetInt("lastSelectedMissile");

        for (int a = 0; a < 2; a++)
        {
            GameObject missileSelected = missileSelectionBoxes.transform.Find("missileBox_" + lastSelected)
          .gameObject;          

            GameObject selectedBox = missileSelected.transform.GetChild(3).gameObject;
            GameObject unlockedBox = missileSelected.transform.GetChild(4).gameObject;

            if (lastSelected != selectedMissileNumb)
            {
                selectedBox.SetActive(false);
                unlockedBox.SetActive(true);
            }
            else
            {
                selectedBox.SetActive(true);
                unlockedBox.SetActive(false);
            }

            lastSelected = selectedMissileNumb;
        }    

        PlayerPrefs.SetInt("lastSelectedMissile", selectedMissileNumb);

        barOpened_missileMenu.SetActive(false);
        bar_missileMenu.SetActive(true);

        if (selectedMissile_missileInfo != null) selectedMissile_missileInfo.SetActive(false);

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
