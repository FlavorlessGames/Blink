using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class InteractCube : NetworkBehaviour
{
    void Start()
    {
        Interactable _interact = GetComponent<Interactable>();
        if (_interact == null)
        {
            Debug.LogError("Interactable component not found");
            return;
        }
        _interact.InteractEvent += interactHandler;
    }

    private void interactHandler()
    {
        spinRpc();
    }

    [Rpc(SendTo.Server)]
    private void spinRpc()
    {
        GetComponent<Rotator>().ToggleSpin();
    }
}
