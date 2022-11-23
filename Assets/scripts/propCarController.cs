using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class propCarController : MonoBehaviour
{
    public GameObject[] sets_R , sets_L;

    int setNumberR, setNumberL;

    void Start()
    {
        int[] setLenghtR = { 0 };
        setNumberR = setLenghtR[Random.Range(0, setLenghtR.Length)];

        int[] setLenghtL = { 0 };
        setNumberL = setLenghtL[Random.Range(0, setLenghtL.Length)];

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
}
