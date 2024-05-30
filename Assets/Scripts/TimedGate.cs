using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedGate : MonoBehaviour
{
    [Header("Gate Door Options")] 
    [SerializeField] private GameObject activateOverlay;
    [SerializeField] private Gate parentGateComponent;
    [SerializeField] private float timeout;
    
    private void Awake()
    {
        // In the awake we get all the references that we will be using on the script, allowing us
        // to only find the reference once at the start, instead of everytime the reference is needed
        parentGateComponent = GetComponentInParent<Gate>();
        // In this case we need to deactivate the opened overlay for the terminal
        activateOverlay.SetActive(false);
    }

    private void ActivateGate()
    {
        // We activate the game, and invoke TimeoutCall to close the gate after a given amount of time
        parentGateComponent.SetOpened(true);
        activateOverlay.SetActive(true);
        Invoke(nameof(TimeoutCall),timeout);
    }


    private void TimeoutCall()
    {
        //After a given amount of time has passed, it closes the gate
        parentGateComponent.SetOpened(false);
        activateOverlay.SetActive(false);
    }
}
