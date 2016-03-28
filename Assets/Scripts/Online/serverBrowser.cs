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
        onlineHelper.Joining = true;
        PhotonNetwork.LoadLevel("OnlineLoading");
    }
}
