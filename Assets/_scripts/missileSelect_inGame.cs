using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileSelect_inGame : MonoBehaviour
{
    GameObject missile;
    [SerializeField] GameObject spawnedMissile;
    [SerializeField] GameObject[] missilePrefabs;
    [SerializeField] GameObject[] premium_missilePrefabs;

    void Start()
    {
        if (missile != null) { missile.SetActive(false); }

        int lastSelected = PlayerPrefs.GetInt("lastSelectedMissile");

        //     missile = gameObject.transform.GetChild(lastSelected).gameObject;
        //     missile.SetActive(true);

        int layerNumb = gameObject.layer;
        string nameLayer = LayerMask.LayerToName(layerNumb);

        GameObject spawnedMissile = Instantiate(missilePrefabs[lastSelected], transform);

        spawnedMissile.layer = LayerMask.NameToLayer(nameLayer);
        Debug.Log(spawnedMissile.layer);

        foreach (Transform child in spawnedMissile.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(nameLayer); ;        
        }       

        Debug.Log("selected missile : " + lastSelected);
    }
}
