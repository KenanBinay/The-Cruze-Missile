using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileSpawnManager : MonoBehaviour
{
    public GameObject missileMain, mainCam, jetStart;

    public Vector3[] top_spawnPoses, right_spawnPoses, left_spawnPoses, bottom_spawnPoses;

    int spawnPoint, spawnFace;
    public static bool spawnedTop, spawnedLeft, spawnedRight, spawnedBottom;

    public void missileSpawn()
    {
        int[] spawnFaceNumbs = { 0, 1, 2, 3 };
        spawnFace = spawnFaceNumbs[Random.Range(0, spawnFaceNumbs.Length)];

        int[] mainSpawnPoints = { 0, 1, 2, 3, 4 };
        spawnPoint = mainSpawnPoints[Random.Range(0, mainSpawnPoints.Length)];

        if (spawnFace == 0)
        {
            Debug.Log("spawned on top, yaw = 0");

            spawnedTop = true;

            missileMain.transform.position = new Vector3(top_spawnPoses[spawnPoint].x, missileMain.transform.position.y, missileMain.transform.position.z);
            jetStart.transform.position = new Vector3(missileMain.transform.position.x, jetStart.transform.position.y, jetStart.transform.position.z);
            mainCam.transform.position = new Vector3(missileMain.transform.position.x + 4.2f, missileMain.transform.position.y - 1, missileMain.transform.position.z + 9);
        }
        if (spawnFace == 1)
        {
            Debug.Log("spawned on right, yaw = 90");

            spawnedRight = true;

            missileMain.transform.Rotate(new Vector3(0, 90, 0));
            jetStart.transform.Rotate(new Vector3(0, 90, 0));
            mainCam.transform.eulerAngles = new Vector3(0, -68, 0);

            missileMain.transform.position = new Vector3(right_spawnPoses[spawnPoint].x, missileMain.transform.position.y, right_spawnPoses[spawnPoint].z);
            jetStart.transform.position = new Vector3(missileMain.transform.position.x, jetStart.transform.position.y, missileMain.transform.position.z);
            mainCam.transform.position = new Vector3(missileMain.transform.position.x + 9, missileMain.transform.position.y - 1, missileMain.transform.position.z - 4.2f);
        }
        if (spawnFace == 2)
        {
            Debug.Log("spawned on left, yaw = -90");

            spawnedLeft = true;

            missileMain.transform.Rotate(new Vector3(0, -90, 0));
            jetStart.transform.Rotate(new Vector3(0, -90, 0));
            mainCam.transform.eulerAngles = new Vector3(0, -248, 0);

            missileMain.transform.position = new Vector3(left_spawnPoses[spawnPoint].x, missileMain.transform.position.y, left_spawnPoses[spawnPoint].z);
            jetStart.transform.position = new Vector3(missileMain.transform.position.x, jetStart.transform.position.y, missileMain.transform.position.z);
            mainCam.transform.position = new Vector3(missileMain.transform.position.x - 9f, missileMain.transform.position.y - 1, missileMain.transform.position.z + 4.2f);
        }
        if (spawnFace == 3)
        {
            Debug.Log("spawned on bottom, yaw = 180");

            spawnedBottom = true;

            missileMain.transform.Rotate(new Vector3(0, 180, 0));
            jetStart.transform.Rotate(new Vector3(0, 180, 0));
            mainCam.transform.eulerAngles = new Vector3(0, 21, 0);

            missileMain.transform.position = new Vector3(bottom_spawnPoses[spawnPoint].x, missileMain.transform.position.y, bottom_spawnPoses[spawnPoint].z);
            jetStart.transform.position = new Vector3(missileMain.transform.position.x, jetStart.transform.position.y, missileMain.transform.position.z);
            mainCam.transform.position = new Vector3(missileMain.transform.position.x - 4.2f, missileMain.transform.position.y - 1, missileMain.transform.position.z - 9);
        }
    }
}