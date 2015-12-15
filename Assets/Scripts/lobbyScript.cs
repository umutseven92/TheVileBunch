using System.Globalization;
using UnityEngine;

public class lobbyScript : Photon.PunBehaviour
{
    private string LobbyName { get; set; }

    public void CreateLobby()
    {
        if (string.IsNullOrEmpty(LobbyName))
        {
            return;
        }

        onlineHelper.LobbyName = LobbyName;
        onlineHelper.Joining = false;
        Application.LoadLevel("DunesOnlineLoading");
    }

    public void SetLobbyName(string lobbyName)
    {
        LobbyName = lobbyName.ToLower(CultureInfo.InvariantCulture);
    }
}
