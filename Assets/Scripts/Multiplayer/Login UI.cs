using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System;

public class LoginUI : NetworkBehaviour
{
    private const string id_AuthButton = "AuthButton";
    private const string id_Insert = "body";
    private const string id_Header = "header";
    private const string id_Footer = "footer";

    public UIDocument uiDocument;
    public SceneSelector Scenes;
    private VisualElement _insert;
    private VisualElement _header;
    private VisualElement _footer;
    private VisualElement _lobbyElement;
    private Screen _currentScreen;
    private const float c_RefreshDelay = 1.5f;
    private float _nextRefreshTime;
    private string _selectedScene;

    void Start()
    {
        _insert = uiDocument.rootVisualElement.Q<VisualElement>(id_Insert);
        _footer = uiDocument.rootVisualElement.Q<VisualElement>(id_Footer);
        _header = uiDocument.rootVisualElement.Q<VisualElement>(id_Header);
        JoinScreen();
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
        SelectSceneScreen();
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

    public void SelectSceneScreen()
    {
        clear();
        _header.Add(new Label("Select a Scene"));
        foreach (string name in Scenes.SceneNames())
        {
            _insert.Add(newButton(name, () => {setScene(name);}));
        }
    }

    public void StartGameScreen()
    {
        clear();
        _footer.Add(new Label(string.Format("Selected Scene: {0}", _selectedScene)));
        Button start = newButton("Start Game", startGame);
        _header.Add(new Label("Connected Players: 0/3 (currently not operational)"));
        _insert.Add(start);
    }

    private void setScene(string name)
    {
        _selectedScene = name;
        StartGameScreen();
    }

    private void startGame()
    {
        setLoading();
        string [] names = Scenes.SceneNames();

        Scenes.Load(_selectedScene);
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

    private void clear()
    {
        _insert.Clear();
        _header.Clear();
        _footer.Clear();
    }

    private enum Screen
    {
        None,
        Lobbies,
    }
}