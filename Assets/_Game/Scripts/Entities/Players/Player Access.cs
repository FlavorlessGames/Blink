using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerAccess : NetworkBehaviour
{
    public Vector3 Position { get { return transform.position; } }
    // Update is called once per frame
    public override void OnNetworkSpawn()
    {
        Debug.Assert(EntityManager.Instance != null);
        EntityManager.Instance.AddLiving(this);
    }

    void Update()
    {
        updatePostion();
    }

    public void updatePostion()
    {
        if (EntityManager.Instance == null) return;
        EntityManager.Instance.UpdatePosition(this, transform.position);
    }

    public void Kill()
    {
        Debug.Log("Killed");
        EntityManager.Instance.PlayerKilled(this);
    }
}
