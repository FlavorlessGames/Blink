using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System;
using fgames.Menus;

public class LoginUI : NetworkBehaviour
{
    private const string id_AuthButton = "AuthButton";
    private const string id_Insert = "body";
    private const string id_Header = "header";
    private const string id_Footer = "footer";
    private const string c_lobbyPlayerText = "Players in Lobby: {0}/4";

    public UIDocument uiDocument;
    public SceneSelector Scenes;
    private MainMenuPage _page;
    private MultiplayerScreen _multiScreen;
    private LobbySelectScreen _lobbyScreen;
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
        _page = new MainMenuPage(uiDocument.rootVisualElement);
        _insert = uiDocument.rootVisualElement.Q<VisualElement>(id_Insert);
        _footer = uiDocument.rootVisualElement.Q<VisualElement>(id_Footer);
        _header = uiDocument.rootVisualElement.Q<VisualElement>(id_Header);
        JoinScreen();

        MultiplayerManager.Instance.LobbyUpdated += updateLobbyInfo;
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
        if (_multiScreen != null)
        {
            _multiScreen.Activate(_page);
            return;
        }
        _multiScreen = new MultiplayerScreen(_page);
        _multiScreen.JoinGame += LobbyPanel;
        _multiScreen.CreateGame += createLobby;
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
        if (_lobbyScreen != null)
        {
            _lobbyScreen.Activate(_page);
            return;
        }
        _lobbyScreen = new LobbySelectScreen(_page);
        _lobbyScreen.LobbySelected += selectLobby;
        // _insert.Clear();
        // List<string> ids = new List<string>{"Test"};
        // Label title = new Label("Lobbies");
        // _insert.Add(title);
        // _currentScreen = Screen.Lobbies;
        // _lobbyElement = new VisualElement();
        // _insert.Add(_lobbyElement);
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
        _currentScreen = Screen.StartGame;
        _footer.Add(new Label(string.Format("Selected Scene: {0}", _selectedScene)));
        Button start = newButton("Start Game", startGame);
        _header.Add(new Label(string.Format(c_lobbyPlayerText, MultiplayerManager.Instance.PlayersInLobby)));
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

    private void refreshLobbies()
    {
        _lobbyScreen.Refresh();
        // List<LobbyDetails> lds = await MultiplayerManager.Instance.FetchLobbies();
        // _lobbyElement.Clear();
        // foreach (LobbyDetails ld in lds)
        // {
        //     Button lobby = newButton(ld.ToString(), () => {selectLobby(ld.ID);});
        //     _lobbyElement.Add(lobby);
        // }
    }

    private async void selectLobby(string id)
    {
        setLoading();

        try
        {
            await MultiplayerManager.Instance.JoinLobby(id);
            NetworkManager.Singleton.StartClient();
            RoomScreen();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            LobbyPanel();
        }
    }

    public void RoomScreen()
    {
        _insert.Clear();
        _insert.Add(new Label("Waiting for Host to start Game"));
    }

    private void updateLobbyInfo(int playerCount)
    {
        if (_currentScreen != Screen.StartGame) return;
        
        _header.Clear();
        _header.Add(new Label(string.Format(c_lobbyPlayerText, playerCount)));
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
        StartGame,
    }
}