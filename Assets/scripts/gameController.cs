using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public Animator startAnim;
    public GameObject missileHud;

    public static bool startDelayed;
    void Start()
    {
        missileHud.SetActive(false);
        startDelayed = false;

        StartCoroutine(startDelay());
    }

    void Update()
    {
        
    }

    IEnumerator startDelay()
    {
        yield return new WaitForSeconds(3f);
        startDelayed = true;
        startAnim.enabled = false;
        missileHud.SetActive(true);
        Debug.Log("missileCam");
    }
}
