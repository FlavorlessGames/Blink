using Unity.Netcode;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using fgames.Playtesting;

public class GameManager : SingletonNetwork<GameManager> {
    [SerializeField] private PlayerMovement _playerPrefab;
    [SerializeField] private GameObject _spawnPoint;
    [SerializeField] private bool _resetOnDeath;
    private bool _singlePlayer;

    private List<ulong> _connectedClients = new List<ulong>();
    public Vector3 SpawnLocation => getSpawnLocation();

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
        _singlePlayer = true;
        NetworkManager.Singleton.StartHost();
    }

    public override void OnNetworkSpawn()
    {
        if (!_singlePlayer) return;
        try
        {
            if (_spawnPoint.transform == null) FindSpawnPoint();
        }
        catch (UnassignedReferenceException)
        {
            Debug.Log("Spawn Point was not manually set. Searching for a player prefab in the scene");
            FindSpawnPoint();
        }
        SpawnPlayer(NetworkManager.Singleton.LocalClientId, 0);
    }   

    private void SpawnPlayer(ulong playerId, int playerIndex = 0) 
    {
        // Debug.Log("Spawn");
        Debug.Assert(_spawnPoint != null);
        Transform sp = _spawnPoint.transform;
        Vector3 spawnPoint = sp.position + Vector3.forward * playerIndex;
        // GameObject spawn = (GameObject) Instantiate(_playerPrefab.gameObject, sp.position + Vector3.forward * playerId, sp.rotation);
        // spawn.GetComponent<NetworkObject>().SpawnWithOwnership(playerId);
        SpawnNewNetworkObjectAsPlayerObject(_playerPrefab.gameObject, spawnPoint, playerId, true);
    }

    public override void OnDestroy() 
    {
        base.OnDestroy();
        if(NetworkManager.Singleton != null )NetworkManager.Singleton.Shutdown();
    }

    public void AllPlayersKilled()
    {
        if (DebugManager.Instance.LevelDoesNotReset) return;
        if (!IsHost) return;
        Scene currentScene = SceneManager.GetActiveScene();
        NetworkManager.Singleton.SceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);
    }

    public void ServerSceneInit(ulong clientId)
    {
        _connectedClients.Add(clientId);

        if (_connectedClients.Count < NetworkManager.Singleton.ConnectedClients.Count) return;

        int index = 0;
        foreach (ulong client in _connectedClients)
        {
            Debug.Log(client);
            SpawnPlayer(client, index++);
        }
    }

    public static GameObject SpawnNewNetworkObjectAsPlayerObject(
        GameObject prefab,
        Vector3 position,
        ulong newClientOwnerId,
        bool destroyWithScene = true)
    {
#if UNITY_EDITOR
        if (!NetworkManager.Singleton.IsServer)
        {
            Debug.LogError("ERROR: Spawning not happening in the server!");
        }
#endif
        Debug.Assert(NetworkManager.Singleton.IsHost);
        // We're first instantiating the new instance in the host client
        GameObject newGameObject = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity);

        // Replicating that same new instance to all connected clients
        NetworkObject newGameObjectNetworkObject = newGameObject.GetComponent<NetworkObject>();
        newGameObjectNetworkObject.SpawnWithOwnership(newClientOwnerId, destroyWithScene);

        return newGameObject;
    }

    private Vector3 getSpawnLocation()
    {
        Debug.Assert(_spawnPoint != null);
        return _spawnPoint.transform.position;
    }
}