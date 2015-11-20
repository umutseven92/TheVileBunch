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
        PhotonNetwork.JoinRoom(RoomName);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("DunesOnline");
    }
}
