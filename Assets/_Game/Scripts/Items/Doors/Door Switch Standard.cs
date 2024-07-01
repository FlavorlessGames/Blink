using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Door))]
public class DoorSwitchStandard : MonoBehaviour
{
    [SerializeField] private List<Interactable> _switches;
    private Door _door;
    void Start()
    {
        _door = GetComponent<Door>();
        foreach (Interactable i in _switches)
        {
            i.InteractEvent += doorSwitch;
        }
    }

    private void doorSwitch(PlayerInteraction player)
    {
        _door.OpenClose();
    }
}