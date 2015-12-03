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
        //       PhotonNetwork.ConnectUsingSettings(global.GameVersion);
    }

    public void SetLobbyName(string lobbyName)
    {
        LobbyName = lobbyName.ToLower(CultureInfo.InvariantCulture);
    }
    /*
    public override void OnJoinedLobby()
    {
        PhotonNetwork.CreateRoom(LobbyName, new RoomOptions() { maxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("DunesOnline");
    }
    */
}
