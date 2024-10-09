using UnityEngine;
using UnityEngine.UIElements;

namespace fgames.Menus
{
    public class PauseUI : MonoBehaviour
    {
        public UIDocument uiDocument;
        private PauseScreenPage _page;
        private MainPausePage _mainPage;
        private DebugPage _debugPage;

        void Awake()
        {
            _page = new PauseScreenPage(uiDocument.rootVisualElement);
        }

        void Start()
        {
            Hide();
        }
    
        public void Hide()
        {
            uiDocument.rootVisualElement.visible = false;
        }

        public void Show()
        {
            uiDocument.rootVisualElement.visible = true;
            mainScreen();
        }

        private void mainScreen()
        {
            if (_mainPage != null)
            {
                _mainPage.Activate(_page);
                return;
            }
            _mainPage = new MainPausePage(_page);
            _mainPage.DebugScreen += debugScreen;
        }

        private void debugScreen()
        {
            Debug.Log("Debug");
            if (_debugPage != null)
            {
                _debugPage.Activate(_page);
                return;
            }
            _debugPage = new DebugPage(_page);
        }
    }
}
