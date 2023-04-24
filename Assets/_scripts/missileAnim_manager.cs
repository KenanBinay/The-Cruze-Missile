using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileAnim_manager : MonoBehaviour
{
    [SerializeField] Animator missileAnim;

    bool specialAnimPlayed;
    void Start()
    {
        missileAnim = GetComponent<Animator>();
        missileAnim.SetTrigger("close");
    }

    void Update()
    {
        if (missileController.speedUp && !specialAnimPlayed)
        {
            missileAnim.SetTrigger("open");
            specialAnimPlayed = true;
        }
        if (!missileController.speedUp && specialAnimPlayed)
        {
            missileAnim.SetTrigger("close");
            specialAnimPlayed = false;
        }
    }
}
