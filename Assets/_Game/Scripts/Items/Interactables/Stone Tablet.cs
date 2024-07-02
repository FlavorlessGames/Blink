using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Interactable))]
public class StoneTablet : MonoBehaviour
{
    public string Text;
    private UIDocument _visual;

    void Start()
    {
        _visual = GetComponent<UIDocument>();
        Interactable i = GetComponent<Interactable>();
        i.InteractEvent += viewTablet;
    }
    
    private void viewTablet(PlayerInteraction pi)
    {
        pi.CloseUI += CloseTablet;
        _visual.enabled = true;
        Label textBlock = _visual.rootVisualElement.Q<Label>("text");
        textBlock.text = Text;
    }

    public void CloseTablet()
    {
        _visual.enabled = false;
    }
}