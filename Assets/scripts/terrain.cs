using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrain : MonoBehaviour
{
    public GameObject mainHudUi, controllerJoystick;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("missileM"))
        {
            if (missileController.crashed == false)
            {
                missileController.crashed = true;
                gameController.crash(mainHudUi, controllerJoystick);
                Debug.Log("crashed");
            }
        }
    }
}
