// using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
// using Unity.Services.Authentication;
// using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class MultiplayerManager : MonoBehaviour
{
    private const int c_HeartbeatInterval = 15;
    private const int c_LobbyRefreshRate = 2;
    private const int c_MaxPlayers = 4;
    private const string c_JoinKey = "j";
    public static MultiplayerManager Instance;
    private static UnityTransport _transport;
    private static Lobby _currentLobby;   
    private static CancellationTokenSource _heartbeatSource, _updateLobbySource;
    public static event Action<Lobby> CurrentLobbyRefreshed;

    void Awake()
    {
        _transport = FindObjectOfType<UnityTransport>();
    }
    void Start()
    {
        if (Instance != null) Destroy(this);
        Instance = this;
    }

    public async Task CreateGame()
    {
        Allocation a = await RelayService.Instance.CreateAllocationAsync(c_MaxPlayers);
        var joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
        Debug.Log(joinCode);
        
        var options = new CreateLobbyOptions {
            Data = new Dictionary<string, DataObject> {
                { c_JoinKey, new DataObject(DataObject.VisibilityOptions.Member, joinCode) }
            }
        };
        _currentLobby = await Lobbies.Instance.CreateLobbyAsync("lobby name", c_MaxPlayers, options);
        _transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);
        
        Heartbeat();
        PeriodicallyRefreshLobby();
    }
    
    private static async void Heartbeat() {
        _heartbeatSource = new CancellationTokenSource();
        while (!_heartbeatSource.IsCancellationRequested && _currentLobby != null) {
            await Lobbies.Instance.SendHeartbeatPingAsync(_currentLobby.Id);
            await Task.Delay(c_HeartbeatInterval * 1000);
        }
    }
    
    private static async void PeriodicallyRefreshLobby() {
        _updateLobbySource = new CancellationTokenSource();
        await Task.Delay(c_LobbyRefreshRate * 1000);
        while (!_updateLobbySource.IsCancellationRequested && _currentLobby != null) {
            _currentLobby = await Lobbies.Instance.GetLobbyAsync(_currentLobby.Id);
            CurrentLobbyRefreshed?.Invoke(_currentLobby);
            await Task.Delay(c_LobbyRefreshRate * 1000);
        }
    } 
    private async Task<List<Lobby>> GatherLobbies() {
        var options = new QueryLobbiesOptions {
            Count = 15,

            Filters = new List<QueryFilter> {
                new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                new(QueryFilter.FieldOptions.IsLocked, "0", QueryFilter.OpOptions.EQ)
            }
        };

        var allLobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
        return allLobbies.Results;
    }

    public async Task<List<string>> FetchLobbies()
    {
        try
        {
            List<Lobby> allLobbies = await GatherLobbies();
            
            var lobbyIDs = allLobbies.Where(l => l.HostId != Authentication.PlayerId).Select(l => l.Id).ToList();

            return lobbyIDs;

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return new List<string>();
    }

    public async Task JoinLobby(string lobbyID)
    {
        _currentLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyID);
        var a = await RelayService.Instance.JoinAllocationAsync(_currentLobby.Data[c_JoinKey].Value); 
        _transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);

        PeriodicallyRefreshLobby();
    }
}
