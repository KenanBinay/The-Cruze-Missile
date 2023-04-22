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
        GameObject particle_0 = spawnedMissile.transform.GetChild(1).gameObject;
        GameObject particle_1 = spawnedMissile.transform.GetChild(2).gameObject;

        spawnedMissile.layer = LayerMask.NameToLayer(nameLayer);
        Debug.Log(spawnedMissile.layer);

        foreach (Transform child in spawnedMissile.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(nameLayer);
        }
        foreach (Transform child in particle_0.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(nameLayer);
        }
        foreach (Transform child in particle_1.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(nameLayer);
        }

        Debug.Log("selected missile : " + lastSelected);
    }
}
