using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public UIDocument uiDocument;

    void Start()
    {
        var create = new Button { text = "Start Game" };
        create.clicked += loadScene;
        uiDocument.rootVisualElement.Add(create);
    }

    void loadScene()
    {
        SceneManager.LoadScene("Main Scene");
    }
}