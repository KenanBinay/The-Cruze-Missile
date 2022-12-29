using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fuelManager : MonoBehaviour
{
    Rigidbody missileRb;
    GameObject missileObj;
    public Animator anim_outOfFuel;
    public Slider fuelSlide;

    public float fuelCountdown;
    public static bool outOfFuel;

    void Start()
    {
        fuelSlide.maxValue = fuelCountdown;

        missileObj = GameObject.Find("Missile_main");
        missileRb = missileObj.GetComponent<Rigidbody>();

        anim_outOfFuel.enabled = false;
    }

    void Update()
    {
        if (gameController.startDelay)
        {
            if (fuelCountdown > 0)
            {
                fuelCountdown -= Time.deltaTime;
                fuelSlide.value = fuelCountdown;
            }
            else { missileRb.useGravity = true; outOfFuel = true; anim_outOfFuel.enabled = true; }
        }
    }
}
