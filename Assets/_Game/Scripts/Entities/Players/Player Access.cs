using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using fgames.Debug;


public class PlayerAccess : NetworkBehaviour
{
    public Vector3 Position { get { return transform.position; } }
    public event DestroyedPlayerHandler PlayerDestroyed;
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
        if (DebugManager.Instance.PlayerWarpOnDeath) warpToSpawn();
    }

    public override void OnDestroy()
    {
        PlayerDestroyed?.Invoke(this);
    }

    private void warpToSpawn()
    {
        if (!IsServer) return;
        Vector3 spawnPoint = GameManager.Instance.SpawnLocation;
        WarpToRpc(spawnPoint);
    }

    [Rpc(SendTo.Everyone)]
    private void WarpToRpc(Vector3 location)
    {
        WarpTo(location);
    }

    public void WarpTo(Vector3 location)
    {
        CharacterController cc = GetComponent<CharacterController>();
        Debug.Assert(cc != null);
        cc.enabled = false;
        gameObject.transform.position = location;
        cc.enabled = true;
    }
}

public delegate void DestroyedPlayerHandler(PlayerAccess pa);
