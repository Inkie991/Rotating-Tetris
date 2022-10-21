using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureDetection : MonoBehaviour
{
    public static event OnFigureInTrigger TriggerEvent;
    public delegate void OnFigureInTrigger();
    
    private void OnTriggerEnter(Collider other)
    {
        TriggerEvent();
        transform.position = new Vector3(transform.position.x, -10, transform.position.z);
    }
}
