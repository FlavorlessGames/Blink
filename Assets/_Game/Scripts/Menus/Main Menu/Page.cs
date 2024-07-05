using UnityEngine.UIElements;

namespace fgames.Menus
{
    public class MainMenuPage
    {
        private const string id_Body = "body";
        private const string id_Footer = "footer";
        private const string id_Header = "header";
        public VisualElement Body;
        public VisualElement Header;
        public VisualElement Footer;

        public MainMenuPage(VisualElement root)
        {
            Body = root.Q<VisualElement>(id_Body);
            Footer = root.Q<VisualElement>(id_Footer);
            Header = root.Q<VisualElement>(id_Header);
        }

        public void Clear()
        {
            Body.Clear();
            Header.Clear();
            Footer.Clear();
        }
    }
}