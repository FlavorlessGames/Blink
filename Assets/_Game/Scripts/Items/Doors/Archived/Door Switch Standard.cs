using UnityEngine;
using System.Collections.Generic;
using System;

[Obsolete("This Script has been deprecated. Use the Door Control Script in its place")]
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