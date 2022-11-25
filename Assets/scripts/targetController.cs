using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetController : MonoBehaviour
{
    public Vector3[] spawnPoints, endCamPoses;
    public GameObject endPos, staticTarget;

    int carSpawnPoint;
    public static int target_type;
    void Start()
    {
        int[] carPoints = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        carSpawnPoint = carPoints[Random.Range(0, carPoints.Length)];

        int[] targetNumbs = { 0, 1 };
        target_type = targetNumbs[Random.Range(0, targetNumbs.Length)];

        if (target_type == 0)
        {
            if (carSpawnPoint == 0) { staticTarget.transform.position = spawnPoints[0]; endPos.transform.position = endCamPoses[0]; Debug.Log("staticTarget point 0"); }
            if (carSpawnPoint == 1) { staticTarget.transform.position = spawnPoints[1]; endPos.transform.position = endCamPoses[1]; Debug.Log("staticTarget point 1"); }
            if (carSpawnPoint == 2) { staticTarget.transform.position = spawnPoints[2]; endPos.transform.position = endCamPoses[2]; Debug.Log("staticTarget point 2"); }
            if (carSpawnPoint == 3) { staticTarget.transform.position = spawnPoints[3]; endPos.transform.position = endCamPoses[3]; Debug.Log("staticTarget point 3"); }
            if (carSpawnPoint == 4) { staticTarget.transform.position = spawnPoints[4]; endPos.transform.position = endCamPoses[4]; Debug.Log("staticTarget point 4"); }
            if (carSpawnPoint == 5) { staticTarget.transform.position = spawnPoints[5]; endPos.transform.position = endCamPoses[5]; Debug.Log("staticTarget point 5"); }
            if (carSpawnPoint == 6) { staticTarget.transform.position = spawnPoints[6]; endPos.transform.position = endCamPoses[6]; Debug.Log("staticTarget point 6"); }
            if (carSpawnPoint == 7) { staticTarget.transform.position = spawnPoints[7]; endPos.transform.position = endCamPoses[7]; Debug.Log("staticTarget point 7"); }
            if (carSpawnPoint == 8) { staticTarget.transform.position = spawnPoints[8]; endPos.transform.position = endCamPoses[8]; Debug.Log("staticTarget point 8"); }
            if (carSpawnPoint == 9) { staticTarget.transform.position = spawnPoints[9]; endPos.transform.position = endCamPoses[9]; Debug.Log("staticTarget point 9"); }
        }
        if (target_type == 1)
        {
            staticTarget.SetActive(false);
          
        }
    }

    void Update()
    {
        
    }
}
