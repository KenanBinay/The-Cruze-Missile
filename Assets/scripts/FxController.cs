using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxController : MonoBehaviour
{
    public GameObject[] explosionParticle_crash, explosionParticle_hit;

    bool fxExplode;

    void Start()
    {
        fxExplode = false;
    }

    private void Update()
    {
        if (missileController.crashed || missileController.targetHit && !fxExplode)
        {
            int[] Fx_targetHit = { 0, 1, 2, 3, 4 };
            int selectedTargetHit_Fx = Fx_targetHit[Random.Range(0, Fx_targetHit.Length)];

            int[] Fx_crash = { 1, 2 };
            int selectedCrash_Fx = Fx_crash[Random.Range(0, Fx_crash.Length)];

            if (missileController.crashed && !fxExplode)
            {
                explosionParticle_crash[selectedCrash_Fx].SetActive(true);
                explosionParticle_crash[0].SetActive(true);

                fxExplode = true;
            }
            if (missileController.targetHit && !fxExplode)
            {
                explosionParticle_hit[selectedTargetHit_Fx].SetActive(true);

                fxExplode = true;
            }
        }
    }
}
