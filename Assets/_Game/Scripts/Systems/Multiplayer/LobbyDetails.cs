using Unity.Services.Lobbies.Models;

public struct LobbyDetails
{
    public string ID;
    public int PlayerCount;
    
    public LobbyDetails(Lobby lobby)
    {
        ID = lobby.Id;
        PlayerCount = lobby.Players.Count;
    }

    public override string ToString()
    {
        return string.Format("Current Players: {0}/{1}", PlayerCount, 4);
    }
}