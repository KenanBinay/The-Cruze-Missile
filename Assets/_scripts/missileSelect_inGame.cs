using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileSelect_inGame : MonoBehaviour
{
    GameObject missile;
    void Start()
    {
        if (missile != null) { missile.SetActive(false); }

        int lastSelected = PlayerPrefs.GetInt("lastSelectedMissile");

        missile = gameObject.transform.GetChild(lastSelected).gameObject;

        missile.SetActive(true);

        Debug.Log("selected missile : " + lastSelected);
    }
}
