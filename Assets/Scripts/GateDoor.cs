using UnityEngine;

[AddComponentMenu("Gate/Gate Door")]
public class GateDoor : MonoBehaviour
{
    [Header("Gate Door Options")] 
    [SerializeField] private bool activated;
    [SerializeField] private Sprite activatedTexture2D;
    [SerializeField] private Sprite deactivatedTexture2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Gate parentGateComponent;
    
    private void Awake()
    {
        // In the awake we get all the references that we will be using on the script, allowing us
        // to only find the reference once at the start, instead of everytime the reference is needed
        spriteRenderer = GetComponent<SpriteRenderer>();
        // We get the GatComponent form the parent object
        parentGateComponent = GetComponentInParent<Gate>();
    }

    private void Update()
    {
        //Changes the sprite depending on the current state
        spriteRenderer.sprite = activated ? activatedTexture2D : deactivatedTexture2D;
    }

    private void ActivateGate()
    {
        // Activate the gate, by setting its activated to true, and setting gate as Opened using SetOpened(true)
        activated = true;
        parentGateComponent.SetOpened(true);
    }
    
    private void DeactivateGate()
    {
        // Deacctivate the gate, by setting its activated to false, and setting gate as Opened using SetOpened(false)
        activated = false;
        parentGateComponent.SetOpened(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // checks if a Box ( GameObject with tag "MovableBlock") has entered the collider
        // and triggered and OnTriggerEnter2D, if so we need to activate the gate
        if (other is { gameObject: { tag: "MovableBlock" } })
        {
            ActivateGate();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // checks if a Box ( GameObject with tag "MovableBlock") has left the collider
        // and triggered and OnTriggerExit2D, if so we need to deactivate the gate
        if (other is { gameObject: { tag: "MovableBlock" } })
        {
            DeactivateGate();
        }
    }
}
