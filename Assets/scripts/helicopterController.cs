using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helicopterController : MonoBehaviour
{
    public GameObject rotorBack, rotorM;

    void Start()
    {
        
    }

    void Update()
    {
        rotorBack.transform.Rotate(new Vector3(1000 * Time.deltaTime, 0, 0));
        rotorM.transform.Rotate(new Vector3(0, 500 * Time.deltaTime, 0));
    }
}
