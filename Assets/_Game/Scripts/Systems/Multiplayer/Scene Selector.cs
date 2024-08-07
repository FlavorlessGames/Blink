using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using UnityEditor;

public class SceneSelector : MonoBehaviour
{
    public SceneAsset MainMenu;
    public List<SceneAsset> Scenes;


    public void GoToMenu()
    {
        SceneManager.LoadScene(MainMenu.name);
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
        NetworkManager.Singleton.SceneManager.LoadScene(name, LoadSceneMode.Single);
    }
}
