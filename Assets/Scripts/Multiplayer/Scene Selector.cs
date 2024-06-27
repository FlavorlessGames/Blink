using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SceneSelector : MonoBehaviour
{
    public List<SceneAsset> Scenes;

    public string [] SceneNames()
    {
        string [] names = new string [Scenes.Count];

        for (int i=0; i<Scenes.Count; i++)
        {
            names[i] = Scenes[i].name;
        }

        return names;
    }
}
