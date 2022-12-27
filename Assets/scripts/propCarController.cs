using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class propCarController : MonoBehaviour
{
    public GameObject[] sets_R, sets_L;

    int setNumberR, setNumberL, vehicleNumb, parentPropNumb;

    public static GameObject vehicle;
    void Start()
    {
        int[] setLenghtR = { 0 };
        setNumberR = setLenghtR[Random.Range(0, setLenghtR.Length)];

        int[] setLenghtL = { 0 };
        setNumberL = setLenghtL[Random.Range(0, setLenghtL.Length)];

        int[] vehicleNumbs = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
        vehicleNumb = vehicleNumbs[Random.Range(0, vehicleNumbs.Length)];

        int[] parentProps = { 0, 1 };
        parentPropNumb = parentProps[Random.Range(0, parentProps.Length)];

        if (setNumberR == 0)
        {
            GameObject setObject = sets_R[0];
            setObject = Instantiate(setObject, setObject.transform.position, setObject.transform.rotation);
            setObject.transform.parent = gameObject.transform;
        }
        if (setNumberL == 0)
        {
            GameObject setObject = sets_L[0];
            setObject = Instantiate(setObject, setObject.transform.position, setObject.transform.rotation);
            setObject.transform.parent = gameObject.transform;
        }

    }
    private void Update()
    {
        if (targetController.target_type == 1 && vehicle == null)
        {
            if (parentPropNumb == 0)
            {
                GameObject parentLane = gameObject.transform.Find("set1_Rlane(Clone)").gameObject;
                vehicle = parentLane.transform.GetChild(vehicleNumb).gameObject;
                vehicle.GetComponent<Target>().enabled = true;
            }
            if (parentPropNumb == 1)
            {
                GameObject parentLane = gameObject.transform.Find("set1_Llane(Clone)").gameObject;
                vehicle = parentLane.transform.GetChild(vehicleNumb).gameObject;
                vehicle.GetComponent<Target>().enabled = true;
            }
            vehicle.tag = "vehicleTarget";
        }

        if (!missileController.targetHit && targetController.target_type == 1) CamController.car_targetVector = vehicle.transform.position;
    }
}
