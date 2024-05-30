using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Gate/Gate")]
public class Gate : MonoBehaviour
{
    [Header("Gate Options")] 
    [SerializeField] private GameObject gate;
    [SerializeField] private bool opened = false;

    public void SetOpened(bool opened)
    {
        this.opened = opened;
    }

    private void Update()
    {
        gate.SetActive(!opened);
    }
    
    
}
