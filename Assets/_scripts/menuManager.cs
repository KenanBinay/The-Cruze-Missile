using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuManager : MonoBehaviour
{
    public int sceneId;

    int selectedMissileNumb, levelBase;
    bool waitForReload, missileMenuOpened;

    public Slider levelSlider;
    public Animator bottomStart;
    public TextMeshProUGUI levelBase_text;
    [SerializeField] public GameObject startMenu_canvas, missileMenu_canvas, missileSelectionBoxes
        , bar_missileMenu, barOpened_missileMenu;

    [SerializeField] GameObject[] menuSelectedMissiles;

    void Start()
    {
        sceneId = 1;

        if (selectedMissileNumb == 0) menuSelectedMissiles[selectedMissileNumb].SetActive(true);
        if (selectedMissileNumb == 1) menuSelectedMissiles[selectedMissileNumb].SetActive(true);
        if (selectedMissileNumb == 2) menuSelectedMissiles[selectedMissileNumb].SetActive(true);
        if (selectedMissileNumb == 3) menuSelectedMissiles[selectedMissileNumb].SetActive(true);
        if (selectedMissileNumb == 4) menuSelectedMissiles[selectedMissileNumb].SetActive(true);
        if (selectedMissileNumb == 5) menuSelectedMissiles[selectedMissileNumb].SetActive(true);

    }

    void Update()
    {
        if (levelSlider.value == levelSlider.maxValue)
        {
            levelBase += 1;
            levelSlider.value = 0;
            levelBase_text.text = levelBase.ToString();
        }
    }

    public void loadMission()
    {
        if (!waitForReload)
        {
            bottomStart.SetTrigger("startClick");
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
            if (!operation.isDone) { waitForReload = true; }            
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

    public void selectMissile(int missileNumb)
    {
        for (int a = 0; a < 6; a++)
        {
            if (menuSelectedMissiles[a] != null) menuSelectedMissiles[a].SetActive(false);

            GameObject missileSelected = missileSelectionBoxes.transform.Find("missileBox_" + a)
                .gameObject;
            GameObject selectIndicator = missileSelected.transform.GetChild(1).gameObject;

            if (a == missileNumb)
                selectIndicator.SetActive(true);

            if (a != missileNumb && selectIndicator.activeSelf)
                selectIndicator.SetActive(false);
        }

        selectedMissileNumb = missileNumb;
        Debug.Log("missile selected " + missileNumb);

        StartCoroutine(onMissileSelected());
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
}
