using UnityEngine.UIElements;

namespace fgames.Menus
{
    public class MultiplayerScreen
    {
        public event ButtonEvent JoinGame;
        public event ButtonEvent CreateGame;
        private Button join;
        private Button create;
        public MultiplayerScreen(MainMenuPage page)
        {
            page.Clear();
            join = Utils.NewButton("Join Game", () => {JoinGame?.Invoke();});
            create = Utils.NewButton("Create Game", () => {CreateGame?.Invoke();});
            Activate(page);
        }
        
        public void Activate(MainMenuPage page)
        {
            page.Clear();
            page.Body.Add(join);
            page.Body.Add(create);
        }
    }
}