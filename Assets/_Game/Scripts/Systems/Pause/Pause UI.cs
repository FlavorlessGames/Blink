using UnityEngine;
using UnityEngine.UIElements;

namespace fgames.Menus
{
    public class PauseUI : MonoBehaviour
    {
        public UIDocument uiDocument;
        private PauseScreenPage _page;

        void Start()
        {
            _page = new PauseScreenPage(uiDocument.rootVisualElement);
        }
    }
}