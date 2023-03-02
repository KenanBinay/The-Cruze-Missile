using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backShineRotate : MonoBehaviour
{
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (Time.frameCount % 3 == 0) transform.Rotate(new Vector3(0, 0, Time.deltaTime * 9f));
    }
}
