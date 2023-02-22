using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundManager : MonoBehaviour
{
    public GameObject[] mapBackgrounds_Desert;

    int selectedMapCount;
    void Start()
    {
        for(int a = 0; a < 3; a++)
        {
            mapBackgrounds_Desert[a].SetActive(false);
        }

        int[] mapCount = { 0, 1, 2 };
        selectedMapCount = mapCount[Random.Range(0, mapCount.Length)];

        mapBackgrounds_Desert[selectedMapCount].SetActive(true);
    }
}
