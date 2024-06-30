using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BatteryPack : NetworkBehaviour
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

    private void interactHandler(PlayerInteraction player)
    {
        bool pickedUp = player.PickupBattery();
        if (!pickedUp) return;
        spinRpc();
        // Debug.Log("picked up");
    }

    [Rpc(SendTo.Everyone)]
    private void spinRpc()
    {
        gameObject.SetActive(false);
    }

    public void Toss(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(direction * 5f, ForceMode.Impulse);
    }
}
