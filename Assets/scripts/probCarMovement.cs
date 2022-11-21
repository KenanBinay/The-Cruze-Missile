using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class probCarMovement : MonoBehaviour
{
    public Transform suv;
    public Transform[] pathPoints;
    public float speed;

    int x;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        suv.position = Vector3.MoveTowards(suv.position, pathPoints[x].position, speed * Time.deltaTime);
        suv.DOLookAt(pathPoints[x].position, 2);

        if (suv.position == pathPoints[x].position) { x++; }

    }
}
