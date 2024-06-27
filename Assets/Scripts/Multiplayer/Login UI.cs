using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System;

public class LoginUI : NetworkBehaviour
{
    private const string id_AuthButton = "AuthButton";
    private const string id_Insert = "insert";

    public UIDocument uiDocument;
    public SceneSelector Scenes;
    private VisualElement _insert;
    private VisualElement _lobbyElement;
    private Screen _currentScreen;
    private const float c_RefreshDelay = 1.5f;
    private float _nextRefreshTime;

    void Start()
    {
        _insert = uiDocument.rootVisualElement.Q<VisualElement>(id_Insert);
        Button authButton = _insert.Q<Button>(id_AuthButton);
        authButton.clicked += LoginAnonymously;
    }

    void Update()
    {
        if (Time.time >= _nextRefreshTime) RefreshUI();
    }

    void RefreshUI()
    {
        _nextRefreshTime = Time.time + c_RefreshDelay;

        switch (_currentScreen)
        {
            case Screen.Lobbies:
                refreshLobbies();
                break;
            default:
                return;
        }
    }

    public async void LoginAnonymously() {
        setLoading();
        await Authentication.Login();
        JoinScreen();
    }

    public void JoinScreen()
    {
        _insert.Clear();
        Button join = newButton("Join Game", LobbyPanel);
        _insert.Add(join);
        Button create = newButton("Create Game", createLobby);
        _insert.Add(create);
    } 

    private Button newButton(string label, Action clickEvent)
    {
        Button nb = new Button(clickEvent);
        nb.text = label;
        return nb;
    }

    private async void createLobby()
    {
        setLoading();
        await MultiplayerManager.Instance.CreateGame();
        NetworkManager.Singleton.StartHost();
        StartGameScreen();
    }

    public void LobbyPanel()
    {
        _insert.Clear();
        List<string> ids = new List<string>{"Test"};
        Label title = new Label("Lobbies");
        _insert.Add(title);
        _currentScreen = Screen.Lobbies;
        _lobbyElement = new VisualElement();
        _insert.Add(_lobbyElement);
        refreshLobbies();
    }

    public void StartGameScreen()
    {
        _insert.Clear();
        Button start = newButton("Start Game", startGame);
        _insert.Add(start);
    }

    private void startGame()
    {
        setLoading();
        string [] names = Scenes.SceneNames();

        NetworkManager.Singleton.SceneManager.LoadScene(names[0], LoadSceneMode.Single);
    }
    
    private void setLoading()
    {
        _insert.Clear();
        Label loading = new Label("Loading");
        _insert.Add(loading);
    }

    private async void refreshLobbies()
    {
        List<string> ids = await MultiplayerManager.Instance.FetchLobbies();
        _lobbyElement.Clear();
        foreach (string id in ids)
        {
            Button lobby = newButton(id, () => {selectLobby(id);});
            _lobbyElement.Add(lobby);
        }
    }

    private async void selectLobby(string id)
    {
        setLoading();
        await MultiplayerManager.Instance.JoinLobby(id);
        NetworkManager.Singleton.StartClient();
        RoomScreen();
    }

    public void RoomScreen()
    {
        _insert.Clear();
        _insert.Add(new Label("Waiting for Host to start Game"));
    }

    private enum Screen
    {
        None,
        Lobbies,
    }
}