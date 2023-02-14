using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class fuelManager : MonoBehaviour
{
    Rigidbody missileRb;
    GameObject missileObj;

    public GameObject fuelBar;
    public Animator anim_outOfFuel;
    public Slider fuelSlide;
    private Image fillArea;

    public Color a, b, c, d;
    Color targetColor;

    public float fuelCountdown;
    public static float fuelValue;
    public static bool outOfFuel;

    void Start()
    {
        fuelSlide.maxValue = fuelCountdown;     

        missileObj = GameObject.Find("Missile");
        missileRb = missileObj.GetComponent<Rigidbody>();

        anim_outOfFuel.enabled = outOfFuel = false;

        fuelBar.SetActive(false);

        fillArea = fuelSlide.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
    }

    void LateUpdate()
    {
        if (gameController.startDelay && !missileController.targetHit && !missileController.crashed)
        {
            if (!fuelBar.activeSelf) { fuelBar.SetActive(true); }

            if (fuelCountdown > 0)
            {
                fuelCountdown -= Time.deltaTime;
                fuelSlide.value = fuelCountdown;
                fuelValue = fuelCountdown;

                fillArea.color = Color.Lerp(fillArea.color, targetColor, 0.1f);

                if (fuelCountdown > 20) targetColor = a;
                if (fuelCountdown > 15 && fuelCountdown < 20) targetColor = b;
                if (fuelCountdown > 8 && fuelCountdown < 15) targetColor = c;
                if (fuelCountdown < 8) targetColor = d;
            }
            else { missileRb.useGravity = true; outOfFuel = true; anim_outOfFuel.enabled = true; }          
        }   
    }
}
