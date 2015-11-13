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
        LobbyName = _lobbyName;
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.CreateRoom(LobbyName, new RoomOptions() { maxPlayers = 4 }, null);
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
