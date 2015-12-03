using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class serverBrowser : Photon.PunBehaviour
{
    [HideInInspector]
    public string RoomName;
    
    public Button caller;

    public void JoinRoom()
    {
        //PhotonNetwork.JoinRoom(RoomName);
        onlineHelper.LobbyName = RoomName;
        onlineHelper.Joining = true;
        Application.LoadLevel("DunesOnlineLoading");
    }

    /*
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("DunesOnline");
    }
    */
}
