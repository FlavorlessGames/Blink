using UnityEngine.UIElements;

namespace fgames.Menus
{
    public class PauseScreenPage
    {
        private const string id_Body = "body";
        private const string id_Footer = "footer";
        private const string id_Header = "header";
        public string Title;
        public VisualElement Body;
        public VisualElement Footer;
        public VisualElement Header;

        public PauseScreenPage(VisualElement root)
        {
            Body = root.Q<VisualElement>(id_Body);
            Footer = root.Q<VisualElement>(id_Footer);
            Header = root.Q<VisualElement>(id_Header);
        }

        public void Clear()
        {
            Body.Clear();
            Footer.Clear();
            Header.Clear();
        }
    }
}