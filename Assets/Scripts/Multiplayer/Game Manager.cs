using Unity.Netcode;
using UnityEngine;
using System;

public class GameManager : NetworkBehaviour {
    [SerializeField] private PlayerMovement _playerPrefab;
    [SerializeField] private GameObject _spawnPoint;

    void Start()
    {
        if (!IsSpawned) InitSingleplayer(); // Todo: make this safer
       
    }

    public void FindSpawnPoint()
    {
        UnityEngine.Object [] objs = FindObjectsOfType(typeof(PlayerMovement), true);
        if (objs.Length < 1) throw new Exception("Player Spawn point not found or set");
        PlayerMovement pm = (PlayerMovement) objs[0];
        _spawnPoint = pm.gameObject;
    }
    
    // [Rpc(SendTo.Everyone)]
    // private void disablePlayerPrefabsRpc()
    // {
    //     PlayerMovement [] objs = (PlayerMovement []) FindObjectsOfType(typeof(PlayerMovement), true);
    //     foreach (PlayerMovement pm in objs)
    //     {
    //         pm.gameObject.SetActive(false);
    //     }
    // }

    public void InitSingleplayer()
    {
        NetworkManager.Singleton.StartHost();
    }

    public override void OnNetworkSpawn() {
        try
        {
            if (_spawnPoint.transform == null) FindSpawnPoint();
        }
        catch (UnassignedReferenceException)
        {
            Debug.Log("Spawn Point was not manually set. Searching for a player prefab in the scene");
            FindSpawnPoint();
        }
        SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId);
    }   

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(ulong playerId) {
        Transform sp = _spawnPoint.transform;
        GameObject spawn = (GameObject) Instantiate(_playerPrefab.gameObject, sp.position, sp.rotation);
        spawn.GetComponent<NetworkObject>().SpawnWithOwnership(playerId);
    }

    public override void OnDestroy() {
        base.OnDestroy();
        if(NetworkManager.Singleton != null )NetworkManager.Singleton.Shutdown();
    }
}