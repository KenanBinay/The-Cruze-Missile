using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fuelManager : MonoBehaviour
{
    Rigidbody missileRb;
    GameObject missileObj;

    public GameObject fuelBar;
    public Animator anim_outOfFuel, fuelOffer_anim;
    public Slider fuelSlide;
    private Image fillArea;

    public Color a, b, c, d;
    Color targetColor;

    public float fuelCountdownVal, countdown;
    public static float fuelValue;
    public static bool outOfFuel, refuel, refuelAnimPlayed;

    void Start()
    {
        countdown = fuelCountdownVal;
        fuelSlide.maxValue = fuelCountdownVal;     

        missileObj = GameObject.Find("Missile");
        missileRb = missileObj.GetComponent<Rigidbody>();

        anim_outOfFuel.enabled = outOfFuel = refuelAnimPlayed = false;

        fuelBar.SetActive(false);

        fillArea = fuelSlide.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
    }

    void Update()
    {
        if (gameController.startDelay && !gameController.gameover)
        {
            if (refuel)
            {
                fuelOffer_anim.SetTrigger("offerClose");

                fuelCountdownVal = countdown;
                fuelSlide.maxValue = fuelCountdownVal;
                refuel = refuelAnimPlayed = false;
            }

            if (!fuelBar.activeSelf) { fuelBar.SetActive(true); }

            if (fuelCountdownVal > 0)
            {
                fuelCountdownVal -= Time.deltaTime;
                fuelSlide.value = fuelCountdownVal;
                fuelValue = fuelCountdownVal;

                fillArea.color = Color.Lerp(fillArea.color, targetColor, 0.1f);

                if (fuelCountdownVal > 20) targetColor = a;
                if (fuelCountdownVal > 15 && fuelCountdownVal < 20) targetColor = b;
                if (fuelCountdownVal > 8 && fuelCountdownVal < 15)
                {
                    targetColor = c;
                   if(!refuelAnimPlayed) fuelOffer_anim.SetTrigger("offerOpen"); refuelAnimPlayed = true;
                } 
                if (fuelCountdownVal < 8) targetColor = d;
            }
            else { missileRb.useGravity = true; outOfFuel = true; anim_outOfFuel.enabled = true; }
        }
        if (gameController.gameover && refuelAnimPlayed)
        {
            fuelOffer_anim.SetTrigger("offerClose");
            refuelAnimPlayed = false;
        }
    }
}
