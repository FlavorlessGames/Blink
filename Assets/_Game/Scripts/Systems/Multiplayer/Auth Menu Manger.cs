using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEditor;

public class AuthMenuManger : Singleton<AuthMenuManger>
{ 
    public SceneAsset MainMenu;

    public void GoToMenu()
    {
        SceneManager.LoadScene(MainMenu.name);
    }

}
