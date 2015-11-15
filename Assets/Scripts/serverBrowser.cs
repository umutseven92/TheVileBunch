using System.Linq;
using UnityEngine;

public class serverBrowser : Photon.PunBehaviour
{
    public string LobbyName { get; set; }

    public void JoinRoom()
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
        PhotonNetwork.JoinRoom(LobbyName);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("DunesOnline");

        //Application.LoadLevel("DunesOnline");
    }
}
