using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Gate/Gate Door")]
public class GateDoor : MonoBehaviour
{
    [Header("Gate Door Options")] 
    [SerializeField] private bool activated = false;

    [SerializeField] private Sprite activatedTexture2D;
    [SerializeField] private Sprite deactivatedTexture2D;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Gate parentGateComponent;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        parentGateComponent = GetComponentInParent<Gate>();
    }

    private void Update()
    {
        spriteRenderer.sprite = (activated)?activatedTexture2D:deactivatedTexture2D;
    }

    private void ActivateGate()
    {
        activated = true;
        parentGateComponent.SetOpened(true);
    }
    
    private void DeactivateGate()
    {
        activated = false;
        parentGateComponent.SetOpened(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other is { gameObject: { tag: "MovableBlock" } })
        {
            ActivateGate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other is { gameObject: { tag: "MovableBlock" } })
        {
            DeactivateGate();
        }
    }
}
