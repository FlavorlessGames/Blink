using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System;

public class LoadingManager : SingletonPersistent<LoadingManager>
{
    public string SceneName => _sceneName;
    private string _sceneName;
    public static string [] MenuScenes = {
        "Main Menu",
        "Start With This Scene",
    };

    public void Init()
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnLoadComplete;
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
    }

    public void LoadScene(string name)
    {
        Debug.Assert(NetworkManager.Singleton.IsServer);
        NetworkManager.Singleton.SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
    
    private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {
        if (!NetworkManager.Singleton.IsServer)
            return;

        if (!Array.Exists(MenuScenes, x => x == sceneName))
        {
            GameManager.Instance.ServerSceneInit(clientId);
        }
    }
}