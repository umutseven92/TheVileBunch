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
        PhotonNetwork.ConnectUsingSettings(global.GameVersion);

    }

    public void SetLobbyName(string _lobbyName)
    {
        LobbyName = _lobbyName.ToLower();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.CreateRoom(LobbyName, new RoomOptions() { maxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("DunesOnline");
//        Application.LoadLevel("DunesOnline");
    }
}
