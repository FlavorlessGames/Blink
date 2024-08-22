using UnityEngine.UIElements;

namespace fgames.Menus
{
    public class PauseScreenPage
    {
        private const string id_Body = "Body";
        public string Title;
        public VisualElement Body;

        public PauseScreenPage(VisualElement root)
        {
            Body = root.Q<VisualElement>(id_Body);
        }
    }
}