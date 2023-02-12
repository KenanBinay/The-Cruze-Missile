using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private float counter, dist;

    public Transform missile;
    public Transform target;

    public float lineDrawSpeed = 6f;
    public float renderMeterLimit;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();      
    }

    void LateUpdate()
    {
        if (target == null)
        {
            if (targetController.target_type == 0) { target = GameObject.Find("targetObject_static").GetComponent<Transform>(); }
            if (targetController.target_type == 1) { target = propCarController.vehicle.GetComponent<Transform>(); }
            if (targetController.target_type == 2) { target = propAircraftController.aircraft.GetComponent<Transform>(); }
        }

        if (target != null && gameController.startDelay)
        {
            if (!missileController.crashed && !missileController.targetHit && MissionWaypoint.targetMeter < renderMeterLimit)
            {
                drawingGuideLine();
            }
            if (missileController.crashed || missileController.targetHit || MissionWaypoint.targetMeter > renderMeterLimit)
            {
                Vector3 lineZero = lineRenderer.GetPosition(0);
                if (lineZero != new Vector3(0, 0, 0))
                {
                    endGuideLine();
                }
            }
        }
    }

    void drawingGuideLine()
    {
        counter += 1f / lineDrawSpeed;

        dist = Vector3.Distance(missile.position, target.position);
        float x = Mathf.Lerp(0, dist, counter);

        Vector3 pointA = missile.position;
        Vector3 pointB = target.position;

        Vector3 pointALongLine = x * Vector3.Normalize(pointB - pointA) + pointA;

        lineRenderer.SetPosition(0, pointA);
        lineRenderer.SetPosition(1, pointALongLine);

     //   Debug.Log("target guide line render");
    }

    void endGuideLine()
    {
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(0, 0, 1));

        Debug.Log("guide line render end");
    }
}
