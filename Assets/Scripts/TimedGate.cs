using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGate : MonoBehaviour
{
    [Header("Gate Door Options")] 
    [SerializeField] private GameObject activateOverlay;
    [SerializeField] private Gate parentGateComponent;
    [SerializeField] private float timeout = 5;
    
    private void Awake()
    {
        parentGateComponent = GetComponentInParent<Gate>();
        activateOverlay.SetActive(false);
    }

    private void ActivateGate()
    {
        parentGateComponent.SetOpened(true);
        activateOverlay.SetActive(true);
        Invoke(nameof(timeoutCall),timeout);
    }


    private void timeoutCall()
    {
        parentGateComponent.SetOpened(false);
        activateOverlay.SetActive(false);
    }
    
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 25), "Activate Timed terminal"))
        {
            ActivateGate();
        }
    }
}
