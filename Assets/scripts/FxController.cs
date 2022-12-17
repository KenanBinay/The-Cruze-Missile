using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxController : MonoBehaviour
{
    public GameObject[] explosionParticle_crash, explosionParticle_hit;

    bool fxExplode;

    void Start()
    {
        
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        int[] Fx_targetHit = { 0, 1, 2, 3, 4 };
        int selectedTargetHit_Fx = Fx_targetHit[Random.Range(0, Fx_targetHit.Length)];

        int[] Fx_crash = { 1, 2 };
        int selectedCrash_Fx = Fx_crash[Random.Range(0, Fx_crash.Length)];

        if (collision.gameObject.CompareTag("crashColl"))
        {
            explosionParticle_crash[selectedCrash_Fx].SetActive(true);
            explosionParticle_crash[0].SetActive(true);
        }
        if (collision.gameObject.CompareTag("target"))
        {
            explosionParticle_hit[selectedTargetHit_Fx].SetActive(true);
        }
        if (collision.gameObject.CompareTag("vehicleTarget"))
        {
            explosionParticle_hit[selectedTargetHit_Fx].SetActive(true);
        }
        if (collision.gameObject.CompareTag("airTarget"))
        {
            explosionParticle_hit[selectedTargetHit_Fx].SetActive(true);
        }
    }
}
