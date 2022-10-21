using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure : MonoBehaviour
{
    public static event OnStopPlane StopPlaneEvent;
    public delegate void OnStopPlane();
    
    private List<Transform> cubes = new List<Transform>();

    private void Start()
    {
        FigureDetection.TriggerEvent += OnTrigger;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            cubes.Add(transform.GetChild(i));
        }
    }

    void OnTrigger()
    {
        foreach (var cube in cubes)
        {
            RaycastHit hit;
            Ray ray = new Ray(cube.position, Vector3.left);
            if (Physics.Raycast(ray, out hit, 20f))
            {
                if (hit.collider != null && hit.collider.transform.name != "trigger" && hit.collider.transform.parent.name != "Figure")
                {
                    StartCoroutine(KnockOutCubes(hit));
                }
            }
        }
    }

    private IEnumerator KnockOutCubes(RaycastHit hit)
    {
        StopPlaneEvent();
        yield return new WaitForSeconds(0.2f);
        hit.collider.isTrigger = false;
        Rigidbody tempBody = hit.collider.transform.GetComponent<Rigidbody>();
        tempBody.isKinematic = false;
    }
}
