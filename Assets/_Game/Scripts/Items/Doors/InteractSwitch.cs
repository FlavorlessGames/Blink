using UnityEngine;

[RequireComponent(typeof(DoorSwitch))]
[RequireComponent(typeof(Interactable))]
public class InteractSwitch : MonoBehaviour
{
    void Start()
    {
        Interactable interactable = GetComponent<Interactable>();
        interactable.InteractEvent += interacted;
    }

    private void interacted(PlayerInteraction pi)
    {
        DoorSwitch doorSwitch = GetComponent<DoorSwitch>();
        doorSwitch.Invert();
    }
}