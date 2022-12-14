using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetController : MonoBehaviour
{
    public Transform[] spawnPoints;
    public static GameObject staticTarget;

    int carSpawnPoint;
    public static int target_type;

    void Start()
    {
        staticTarget = GameObject.Find("targetObject_static");

        int[] carPoints = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        carSpawnPoint = carPoints[Random.Range(0, carPoints.Length)];

        int[] targetNumbs = { 0, 1, 2 };
        target_type = targetNumbs[Random.Range(0, targetNumbs.Length)];

        if (target_type == 0)
        {
            if (carSpawnPoint == 0) { staticTarget.transform.position = spawnPoints[0].position; }
            if (carSpawnPoint == 1) { staticTarget.transform.position = spawnPoints[1].position; }
            if (carSpawnPoint == 2) { staticTarget.transform.position = spawnPoints[2].position; }
            if (carSpawnPoint == 3) { staticTarget.transform.position = spawnPoints[3].position; }
            if (carSpawnPoint == 4) { staticTarget.transform.position = spawnPoints[4].position; }
            if (carSpawnPoint == 5) { staticTarget.transform.position = spawnPoints[5].position; }
            if (carSpawnPoint == 6) { staticTarget.transform.position = spawnPoints[6].position; }
            if (carSpawnPoint == 7) { staticTarget.transform.position = spawnPoints[7].position; }
            if (carSpawnPoint == 8) { staticTarget.transform.position = spawnPoints[8].position; }
            if (carSpawnPoint == 9) { staticTarget.transform.position = spawnPoints[9].position; }

            CamController.static_targetVector = staticTarget.transform.position;
            Debug.Log("staticTargetSelected point " + carSpawnPoint);
        }
        if (target_type == 1)
        {
            staticTarget.SetActive(false);
            Debug.Log("vehicleTargetSelected");
        }
        if (target_type == 2)
        {
            staticTarget.SetActive(false);
            Debug.Log("airCraftTargetSelected");
        }
    }
}
