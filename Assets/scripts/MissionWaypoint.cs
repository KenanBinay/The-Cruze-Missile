using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MissionWaypoint : MonoBehaviour
{
    public Transform target;
    public TextMeshProUGUI meter;
    public Vector3 offset;

    private void Update()
    {
      //  float dot = Vector3.Dot(transform.forward, (target.position - transform.position).normalized);    

        meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
    }
}
