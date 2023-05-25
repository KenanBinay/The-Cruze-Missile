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

    int selectedMissileNumb, levelVal, unlockLevel, missileSave, lastSelectedMissileNumb, targetVal,
        soundEffect_value, tokenVal, tokenCostUnlock, isVipMissile;
    bool waitForReload, missileMenuOpened, settingsOpened;
    string selectedMissileName;

    [Header("Slider")]
    [SerializeField] private Slider levelSlider, loadingSlider;
    [Header("")]

    public Animator bottomStart, settings_anim;
    public TextMeshProUGUI levelBase_text, targetVal_text, tokenVal_text, tokenCost_text;

    [SerializeField]
    public GameObject startMenu_canvas, missileMenu_canvas, missileSelectionBoxes
        , unlock_missileMenu_nonVip, unlock_missileMenu_vip, top_mainCanvas, middile_mainCanvas, bottom_mainCanvas,
        loadingUi, sfx_off, settings_values, IAPAds, tokenShop_canvas, startMenuMissileSlot;

    private GameObject selectedMissile_missileInfo;

    public Sprite[] missileSprites;

    [Header("Audio")]
    [SerializeField] AudioSource[] menuSources;
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
        soundEffect_value = PlayerPrefs.GetInt("sfx", 0);
        tokenVal = PlayerPrefs.GetInt("tokens", 0);
        selectedMissileNumb = lastSelectedMissileNumb;

        if(soundEffect_value == 0) { sfx_off.SetActive(true); Debug.Log("sound effects off"); }
        else { sfx_off.SetActive(false); Debug.Log("sound effects on"); }

        GameObject missiles = startMenuMissileSlot.transform.Find("missiles").gameObject;
        missiles.GetComponent<Image>().sprite = missileSprites[selectedMissileNumb];

        levelSlider.value = scoreBar;
        levelBase_text.text = levelVal.ToString();
        targetVal_text.text = "TARGET " + targetVal.ToString();
        tokenVal_text.text = tokenVal.ToString();

        Debug.Log("last selected missile: " + lastSelectedMissileNumb);     
    }

    private void Update()
    {
        if (tokenVal != PlayerPrefs.GetInt("tokens", 0))
        {
            tokenVal = PlayerPrefs.GetInt("tokens", 0);
            tokenVal_text.text = tokenVal.ToString();

            Debug.Log("TOTAL TOKENS: " + tokenVal.ToString());
        }
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
        unlock_missileMenu_nonVip.SetActive(false);
        unlock_missileMenu_vip.SetActive(false);

        for (int a = 0; a < 12; a++)
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

       if(settingsOpened) loadSettings();
    }

    public void loadTokenShop()
    {
        startMenu_canvas.SetActive(false);
        IAPAds.SetActive(false);
        missileMenu_canvas.SetActive(false);
        unlock_missileMenu_nonVip.SetActive(false);
        unlock_missileMenu_vip.SetActive(false);

        tokenShop_canvas.SetActive(true);

        missileMenuOpened = true;
        if (settingsOpened) loadSettings();
    }

    public void returnHome()
    {
        missileMenuOpened = false;

        tokenShop_canvas.SetActive(false);
        missileMenu_canvas.SetActive(false);
        unlock_missileMenu_nonVip.SetActive(false);
        unlock_missileMenu_vip.SetActive(false);

        if (PlayerPrefs.GetInt("adsRemoved", 0) == 0) IAPAds.SetActive(true);

        startMenu_canvas.SetActive(true);
    }

    public void selectMissile(string numbers = "0,0,0,0")
    {
        //getting missile number & levelCost
        string[] split = numbers.Split(","[0]);
        int selectedMissile = int.Parse(split[0]);
        int levelCost = int.Parse(split[1]);
        int tokenCost = int.Parse(split[2]);
        int isVip = int.Parse(split[3]);

        tokenCost_text.text = tokenCost.ToString();

        Debug.Log("missile: " + selectedMissile.ToString()
            + " || required level: " + levelCost.ToString() + " || required token: " + tokenCost);

        for (int a = 0; a < 12; a++)
        {
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
        tokenCostUnlock = tokenCost;
        isVipMissile = isVip;

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
            Debug.Log("missile is not unlocked");

            unlock_missileMenu_nonVip.SetActive(false);
            unlock_missileMenu_vip.SetActive(false);

            if (selectedMissile_missileInfo != null) selectedMissile_missileInfo.SetActive(false);

            if (isVip == 0)
            {
                GameObject missiles = unlock_missileMenu_nonVip.transform.Find("missiles").gameObject;
                missiles.GetComponent<Image>().sprite = missileSprites[selectedMissileNumb];

                //   selectedMissile_missileInfo = missiles.transform.GetChild(selectedMissileNumb).gameObject;

                unlock_missileMenu_nonVip.SetActive(true);
            }
            if (isVip == 1)
            {
                GameObject missiles = unlock_missileMenu_vip.transform.Find("missiles").gameObject;
                missiles.GetComponent<Image>().sprite = missileSprites[selectedMissileNumb];

                //   selectedMissile_missileInfo = missiles.transform.GetChild(selectedMissileNumb).gameObject;

                TextMeshProUGUI tokenCostButton_text = unlock_missileMenu_vip.GetComponentInChildren<TextMeshProUGUI>();
                tokenCostButton_text.text = tokenCost.ToString();

                unlock_missileMenu_vip.SetActive(true);
            }

           // selectedMissile_missileInfo.SetActive(true);
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

    public void unlockMissile_byToken()
    {
        int x = PlayerPrefs.GetInt("tokens", 0);

        if (x >= tokenCostUnlock)
        {
            Debug.Log("unlock via token " + tokenCostUnlock);
            x -= tokenCostUnlock;
            PlayerPrefs.SetInt("tokens", x);

            GameObject missileSelected = missileSelectionBoxes.transform.Find("missileBox_" +
        selectedMissileNumb).gameObject;
            GameObject unlockedBox = missileSelected.transform.GetChild(4).gameObject;
            GameObject lockedBox = missileSelected.transform.GetChild(1).gameObject;

            unlockedBox.SetActive(true);
            lockedBox.SetActive(false);

            PlayerPrefs.SetInt(selectedMissileName, 1);

            Debug.Log("UNLOCKED MISSILE: " + selectedMissileNumb);

            StartCoroutine(onMissileSelected());
        }
        else Debug.Log("need more token to unlock");
    }

    public void loadSettings()
    {
        if (!settingsOpened && !missileMenuOpened)
        {
            settings_values.SetActive(true);
            settings_anim.enabled = true;
            settingsOpened = true;
        }
        else
        {
            settings_anim.SetTrigger("retract");
            StartCoroutine(settingsDelay());
        }
    }

    public void soundEffectState()
    {
        if (soundEffect_value == 0)
        {
            soundEffect_value = 1;
            PlayerPrefs.SetInt("sfx", soundEffect_value);
            sfx_off.SetActive(false);

            Debug.Log("sfx on");
        }
        else
        {
            soundEffect_value = 0;
            PlayerPrefs.SetInt("sfx", soundEffect_value);
            sfx_off.SetActive(true);

            Debug.Log("sfx off");
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

        unlock_missileMenu_nonVip.SetActive(false);
        unlock_missileMenu_vip.SetActive(false);

        if (selectedMissile_missileInfo != null) selectedMissile_missileInfo.SetActive(false);

        missileMenuOpened = false;
        Debug.Log("selected missile: " + selectedMissileNumb);

        yield return new WaitForSeconds(0.3f);

        startMenu_canvas.SetActive(true);
        missileMenu_canvas.SetActive(false);
        startMenuMissileSlot.SetActive(true);

        GameObject missiles = startMenuMissileSlot.transform.Find("missiles").gameObject;
        missiles.GetComponent<Image>().sprite = missileSprites[selectedMissileNumb];
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

    IEnumerator settingsDelay()
    {
        yield return new WaitForSeconds(0.3f);
        settings_values.SetActive(false);
        settingsOpened = false;
    }
}
