using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawLine : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public Transform missile;
    public Transform target;

    public float renderMeterLimit, targetDistance;

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

        if (gameController.startDelay)
        {
            if (!missileController.crashed && !missileController.targetHit)
            {
                targetDistance = Vector3.Distance(missile.position, target.position);
                if (targetDistance < renderMeterLimit)
                {
                    drawingGuideLine();
                }
            }
            if (missileController.crashed || missileController.targetHit || targetDistance > renderMeterLimit)
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
        lineRenderer.SetPosition(0, missile.position);
        lineRenderer.SetPosition(1, target.position);
    }

    void endGuideLine()
    {
        lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
        lineRenderer.SetPosition(1, new Vector3(0, 0, 1));

        Debug.Log("guide line render end");
    }
}
