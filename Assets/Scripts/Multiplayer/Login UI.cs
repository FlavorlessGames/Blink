using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class LoginUI : MonoBehaviour
{
    private const string id_AuthButton = "AuthButton";

    public UIDocument uiDocument;

    void Start()
    {
        Button authButton = uiDocument.rootVisualElement.Q<Button>(id_AuthButton);
        authButton.clicked += LoginAnonymously;
    }

    public async void LoginAnonymously() {
        await Authentication.Login();
        SceneManager.LoadSceneAsync("Multiplayer Menu");
    }
}