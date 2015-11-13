using System.Linq;
using UnityEngine;

public class serverBrowser : Photon.PunBehaviour
{
    public string LobbyName { get; set; }

    // Use this for initialization
    void Start()
    {
    }

    public void JoinRoom()
    {
        var rooms = PhotonNetwork.GetRoomList();

        foreach (var r in rooms.Where(r => r.name.Equals(LobbyName)))
        {
            PhotonNetwork.ConnectUsingSettings(global.GameVersion);
        }
    }

    public void SetLobbyName(string _lobbyName)
    {
        LobbyName = _lobbyName;
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRoom(LobbyName);
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.LogError("Could not join room.");
    }

    public override void OnJoinedRoom()
    {
        Application.LoadLevel("DunesOnline");
    }
}
