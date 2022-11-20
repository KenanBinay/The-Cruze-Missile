using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public Animator startAnim;
    public GameObject missileHud;

    public static bool startDelay;

    void Start()
    {
        missileHud.SetActive(false);
        startDelay = false;

        StartCoroutine(delayForStart());
    }

    void Update()
    {
        
    }

    IEnumerator delayForStart()
    {
        yield return new WaitForSeconds(3f);
        startDelay = true;
        startAnim.enabled = false;
        missileHud.SetActive(true);
        Debug.Log("missileCam");
    }
}
