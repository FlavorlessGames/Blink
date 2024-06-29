using UnityEngine;
using UnityEngine.UIElements;
using System;

public class AuthUI : MonoBehaviour
{
    
    private const string id_Insert = "body";
    public UIDocument uiDocument;
    private VisualElement _insert;
    public SceneSelector Scenes;

    void Start()
    {
        _insert = uiDocument.rootVisualElement.Q<VisualElement>(id_Insert);
        Button authButton = newButton("Login", LoginAnonymously);
        _insert.Add(authButton);
    }
    
    public async void LoginAnonymously() {
        setLoading();
        await Authentication.Login();
        Scenes.GoToMenu();
    }
    
    private void setLoading()
    {
        _insert.Clear();
        Label loading = new Label("Loading");
        _insert.Add(loading);
    }

    private Button newButton(string label, Action clickEvent)
    {
        Button nb = new Button(clickEvent);
        nb.text = label;
        return nb;
    }
}