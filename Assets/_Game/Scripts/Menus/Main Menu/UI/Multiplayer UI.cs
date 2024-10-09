using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class MultiplayerUI : MonoBehaviour
{
    private const string id_StartButton = "StartGameButton";
    private const string id_CreateButton = "CreateButton";
    private const string id_JoinButton = "JoinButton";
    private const string id_CodeInput = "CodeInput";
    private const string id_CodeLabel = "CodeLabel";
    private const string id_ConnectButtons = "ConnectButtons";

    public UIDocument uiDocument;
    private VisualElement connectButtons;
    private Label _joinCodeText;
    private TextField _joinInput;
    private UnityTransport _transport;

    private const int MaxPlayers = 4;

    void Awake()
    {
        _transport = FindObjectOfType<UnityTransport>();
    }

    void Start()
    {
        Button startButton = uiDocument.rootVisualElement.Q<Button>(id_StartButton);
        startButton.clicked += startGame;
        Button createButton = uiDocument.rootVisualElement.Q<Button>(id_CreateButton);
        createButton.clicked += createGame;
        Button joinButton = uiDocument.rootVisualElement.Q<Button>(id_JoinButton);
        joinButton.clicked += joinGame;

        _joinCodeText = uiDocument.rootVisualElement.Q<Label>(id_CodeLabel);
        _joinInput = uiDocument.rootVisualElement.Q<TextField>(id_CodeInput);

        connectButtons = uiDocument.rootVisualElement.Q<VisualElement>(id_ConnectButtons);
    }

    void startGame()
    {
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) return;
        uiDocument.enabled = false;
        NetworkManager.Singleton.SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
    }

    async void joinGame()
    {
        JoinAllocation a = await RelayService.Instance.JoinAllocationAsync(_joinInput.text);
        
        _transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
        
        NetworkManager.Singleton.StartClient();
        hideButtons();
    }

    async void createGame()
    {
        Allocation a = await RelayService.Instance.CreateAllocationAsync(MaxPlayers);
        _joinCodeText.text = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        _transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);
        
        NetworkManager.Singleton.StartHost();
        hideButtons();
    }

    void hideButtons()
    {
        connectButtons.SetEnabled(false);
    }
}