using UnityEngine.UIElements;

namespace fgames.Menus
{
    public class MainPausePage
    {
        public event ButtonEvent DebugScreen;
        // public event ButtonEvent CreateGame;
        private Button debugScreen;
        // private Button create;
        public MainPausePage(PauseScreenPage page)
        {
            // page.Clear();
            debugScreen = Utils.NewButton("Debug", () => {DebugScreen?.Invoke();});
            // create = Utils.NewButton("Create Game", () => {CreateGame?.Invoke();});
            Activate(page);
        }
        
        public void Activate(PauseScreenPage page)
        {
            page.Clear();
            page.Header.Add(new Label("Paused"));
            page.Body.Add(debugScreen);
            // page.Body.Add(join);
            // page.Body.Add(create);
        }
    }
}