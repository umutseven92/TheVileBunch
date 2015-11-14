using System.Linq;
using UnityEngine;

public class serverBrowser : Photon.PunBehaviour
{
    public string LobbyName { get; set; }

    public void JoinRoom()
    {
        PhotonNetwork.ConnectUsingSettings(global.GameVersion);
        Debug.Log("Connecting..");
    }

    public void SetLobbyName(string _lobbyName)
    {
        LobbyName = _lobbyName;
        Debug.Log(LobbyName);
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRoom(LobbyName);
        Debug.Log("Joining Room..");
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.LogError("Could not join room.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Loading level");
        Application.LoadLevel("DunesOnline");
    }
}
