using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEditor;

public class SceneSelector : MonoBehaviour
{
    public List<SceneAsset> Scenes;

    public IEnumerator Start()
    {
        yield return new WaitUntil(() => NetworkManager.Singleton.SceneManager != null);
        LoadingManager.Instance.Init();
    }

    public string [] SceneNames()
    {
        string [] names = new string [Scenes.Count];

        for (int i=0; i<Scenes.Count; i++)
        {
            names[i] = Scenes[i].name;
        }

        return names;
    }

    public void Load(string name)
    {
        Debug.Assert(LoadingManager.Instance != null);
        LoadingManager.Instance.LoadScene(name);
    }
}
