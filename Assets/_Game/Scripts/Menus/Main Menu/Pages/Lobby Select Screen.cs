using UnityEngine.UIElements;
using System.Collections.Generic;

namespace fgames.Menus
{
    public class LobbySelectScreen
    {
        public event SelectHandler LobbySelected;
        private VisualElement _container;

        public LobbySelectScreen(MainMenuPage page)
        {
            _container = new VisualElement();
            Activate(page);
        }

        public void Activate(MainMenuPage page)
        {
            page.Clear();
            page.Header.Add(new Label("Select a Lobby"));
            page.Body.Add(_container);
            loadLobbies();
        }

        public void Refresh()
        {
            loadLobbies();
        }

        #pragma warning disable CS1998 
        private async void loadLobbies()
        {
            List<LobbyDetails> lds = await MultiplayerManager.Instance.FetchLobbies();
            _container.Clear();
            foreach (LobbyDetails ld in lds)
            {
                Button lobby = Utils.NewButton(ld.ToString(), () => {LobbySelected?.Invoke(ld.ID);});
                _container.Add(lobby);
            }
        }
        #pragma warning restore CS1998
    }

    public delegate void SelectHandler(string id);
}