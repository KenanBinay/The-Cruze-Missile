using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MissionWaypoint : MonoBehaviour
{
    public TextMeshPro meterText_;
    public Vector3 offset;

    public static float targetMeter;

    private void Update()
    {
        if (Time.frameCount % 3 == 0)
        {
            if (gameController.startDelay)
            {
                Vector3 intoPlane = Vector3.up;

                // Calculate a vector pointing to the target.
                if (targetController.target_type == 0)
                {
                    meterText_.text = ((int)Vector3.Distance(targetController.staticTarget.transform.position, transform.position)).ToString() + "m";
                    targetMeter = (float)Vector3.Distance(targetController.staticTarget.transform.position, transform.position);

                    Vector3 toTarget = targetController.staticTarget.transform.position - transform.position;
                    transform.rotation = Quaternion.LookRotation(intoPlane, toTarget);
                }
                if (targetController.target_type == 1)
                {
                    meterText_.text = ((int)Vector3.Distance(propCarController.vehicle.transform.position, transform.position)).ToString() + "m";
                    targetMeter = (float)Vector3.Distance(targetController.staticTarget.transform.position, transform.position);

                    Vector3 toTarget = propCarController.vehicle.transform.position - transform.position;
                    transform.rotation = Quaternion.LookRotation(intoPlane, toTarget);
                }
                if (targetController.target_type == 2)
                {
                    meterText_.text = ((int)Vector3.Distance(propAircraftController.aircraft.transform.position, transform.position)).ToString() + "m";
                    targetMeter = (float)Vector3.Distance(targetController.staticTarget.transform.position, transform.position);

                    Vector3 toTarget = propAircraftController.aircraft.transform.position - transform.position;
                    transform.rotation = Quaternion.LookRotation(intoPlane, toTarget);
                }
            }
        }
    }
}
