using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class MissionWaypoint : MonoBehaviour
{
    public Transform target;
    public TextMeshProUGUI meterText;
    public Vector3 offset;

    private void Start()
    {
     
    }

    private void Update()
    {
        meterText.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
        //  transform.LookAt(new Vector3(0,0, target.transform.position.z));
        Vector3 intoPlane = Vector3.up;

        // Calculate a vector pointing to the target.
        Vector3 toTarget = target.transform.position - transform.position;

        // Point our Z+ into the gameplay plane, 
        // and our Y+ away from the target.
        // (Since our "front" is at the bottom/Y- extreme)
        transform.rotation = Quaternion.LookRotation(intoPlane, -toTarget);
    }
}
