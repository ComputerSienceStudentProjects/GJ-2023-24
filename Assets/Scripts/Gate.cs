using UnityEngine;

[AddComponentMenu("Gate/Gate")]
public class Gate : MonoBehaviour
{
    [Header("Gate Options")]
    [SerializeField] private GameObject gate;
    [SerializeField] private GameObject gateBackdrop;
    private bool _opened;

    private void Awake()
    {
        // In the awake set all default values as a failsafe to ensure
        // nothing is incorrectly set in the editor
        _opened = false;
        gate.SetActive(true);
        gateBackdrop.SetActive(false);
    }

    public void SetOpened(bool openedStatus)
    {
        // Sets the status for the gate
        _opened = openedStatus;
    }

    private void Update()
    {
        // If the gate is activated, deactivate the GameObject blocking the path,
        // and activate the walkableTile for the player to wak on
        // or if the status is closed, it reverses the operations
        if (gate != null) gate.SetActive(!_opened);
        if (gateBackdrop != null) gateBackdrop.SetActive(_opened);
    }
}
