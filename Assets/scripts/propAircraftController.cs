using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class propAircraftController : MonoBehaviour
{
    public GameObject[] airCraftSets;
    GameObject parentLane;
    int setNumber, aircraftNumb, parentPropNumb;

    public static GameObject aircraft;
    void Start()
    {
        int[] setLenght = { 0, 1, 2, 3 };
        setNumber = setLenght[Random.Range(0, setLenght.Length)];

        int[] aircraftNumbs = { 0, 1, 2 };
        aircraftNumb = aircraftNumbs[Random.Range(0, aircraftNumbs.Length)];

        GameObject setObject = airCraftSets[setNumber];
        setObject = Instantiate(setObject, setObject.transform.position, setObject.transform.rotation);
        setObject.transform.parent = gameObject.transform;
    }

    private void Update()
    {
        if (targetController.target_type == 2 && aircraft == null)
        {
            if (setNumber == 0) { parentLane = gameObject.transform.Find("heliSet_1(Clone)").gameObject; }
            if (setNumber == 1) { parentLane = gameObject.transform.Find("heliSet_2(Clone)").gameObject; }
            if (setNumber == 2) { parentLane = gameObject.transform.Find("heliSet_3(Clone)").gameObject; }
            if (setNumber == 3) { parentLane = gameObject.transform.Find("heliSet_4(Clone)").gameObject; }

            aircraft = parentLane.transform.GetChild(aircraftNumb).gameObject;
            aircraft.tag = "airTarget";
        }
    }
}
