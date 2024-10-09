using UnityEngine.UIElements;

namespace fgames.Menus
{
    public class SelectSceneScreen
    {
        public event SelectHandler SceneSelected;
        private string [] sceneNames;

        public SelectSceneScreen(MainMenuPage page, string [] sceneNames)
        {
            this.sceneNames = sceneNames;
            Activate(page);
        }

        public void Activate(MainMenuPage page)
        {
            page.Clear();
            page.Header.Add(new Label("Select a Scene"));
            foreach (string name in sceneNames)
            {
                Button selectButton = Utils.NewButton(name, () => {SceneSelected?.Invoke(name);});
                page.Body.Add(selectButton);
            }
        }
    }
}