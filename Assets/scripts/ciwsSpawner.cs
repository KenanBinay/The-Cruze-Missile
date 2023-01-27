using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ciwsSpawner : MonoBehaviour
{
    public GameObject ciwsPrefab;
    public Transform[] Points;

    int spawnDensity;
    private HashSet<int> _selectedValues;

    public void ciwsSpawn()
    {
        _selectedValues = new HashSet<int>();

        int[] density = { 4, 5, 6, 7, 8, 9, 10 };
        spawnDensity = density[Random.Range(0, density.Length)];

        int[] lineVal = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        Debug.Log("ciwsSpawnDensity: " + spawnDensity);

        for (int i = 0; i < spawnDensity; i++)
        {
            int randomSpawnIndex = lineVal[Random.Range(0, lineVal.Length)];
            int randomValue = lineVal[randomSpawnIndex];

            while (_selectedValues.Contains(randomValue))
            {
                randomSpawnIndex = lineVal[Random.Range(0, lineVal.Length)];
                randomValue = lineVal[randomSpawnIndex];
            }

            _selectedValues.Add(randomValue);

            GameObject ciwsSpawned = Instantiate(ciwsPrefab, Points[randomValue].position, Quaternion.identity);
            ciwsSpawned.transform.parent = GameObject.Find("ciws").transform;
            ciwsSpawned.name = "ciws_" + i;
        }
    }
}
