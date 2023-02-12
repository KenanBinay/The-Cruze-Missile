using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class MissionWaypoint : MonoBehaviour
{
    public TextMeshProUGUI meterText;
    public Vector3 offset;

    public static float targetMeter;

    private void Start()
    {

    }

    private void Update()
    {
        if (gameController.startDelay)
        {
            Vector3 intoPlane = Vector3.up;

            // Calculate a vector pointing to the target.
            if (targetController.target_type == 0)
            {
                meterText.text = ((int)Vector3.Distance(targetController.staticTarget.transform.position, transform.position)).ToString() + "m";
                targetMeter = (float)Vector3.Distance(targetController.staticTarget.transform.position, transform.position);

                Vector3 toTarget = targetController.staticTarget.transform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(intoPlane, -toTarget);
            }
            if (targetController.target_type == 1)
            {
                meterText.text = ((int)Vector3.Distance(propCarController.vehicle.transform.position, transform.position)).ToString() + "m";
                targetMeter = (float)Vector3.Distance(targetController.staticTarget.transform.position, transform.position);

                Vector3 toVehicleTarget = propCarController.vehicle.transform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(intoPlane, -toVehicleTarget);
            }
            if (targetController.target_type == 2)
            {
                meterText.text = ((int)Vector3.Distance(propAircraftController.aircraft.transform.position, transform.position)).ToString() + "m";
                targetMeter = (float)Vector3.Distance(targetController.staticTarget.transform.position, transform.position);

                Vector3 toVehicleTarget = propAircraftController.aircraft.transform.position - transform.position;
                transform.rotation = Quaternion.LookRotation(intoPlane, -toVehicleTarget);
            }
        }
    }
}
