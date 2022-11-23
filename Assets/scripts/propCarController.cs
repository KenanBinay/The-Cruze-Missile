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

        if (setNumberR == 0)
        {
            GameObject setObject = sets_R[0];
            setObject = Instantiate(setObject, setObject.transform.position, setObject.transform.rotation);
            setObject.transform.parent = gameObject.transform;
        }
          
    }
}
