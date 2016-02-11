using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class networkLobby : Photon.PunBehaviour
{
    [PunRPC]
    public void PlayerAddRPC(string control, int pId)
    {
        var pView = PhotonView.Find(pId);
        pView.GetComponentInParent<onlinePlayerSelect>().OnlineAddPlayer(control);
    }

    [PunRPC]
    public void PlayerAddInitialRPC(string control, string pClass, string inputControl, int pId)
    {
        var pView = PhotonView.Find(pId);
        pView.GetComponentInParent<onlinePlayerSelect>().OnlineAddInitialPlayer(control, pClass, inputControl);
    }

    [PunRPC]
    public void PlayerRemoveRPC(string control, int pId)
    {
        var pView = PhotonView.Find(pId);
        pView.GetComponentInParent<onlinePlayerSelect>().OnlineRemovePlayer(control);
    }

    [PunRPC]
    public void PlayerChangeRPC(string control, int dir, string playerId, bool delay, int pId)
    {
        var pView = PhotonView.Find(pId);
        pView.GetComponentInParent<onlinePlayerSelect>().OnlineChangePlayer(control, dir, delay, playerId);
    }

    [PunRPC]
    public void PlayerJoinRPC(int pId)
    {
        var players =  FindObjectsOfType<onlinePlayerSelect>().Single(p => p.Master).GetAllPlayers();

        var pView = PhotonView.Find(pId);
        pView.GetComponentInParent<onlinePlayerSelect>().SetAllPlayers(players);

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*
        if (stream.isWriting)
        {
            // Our player
            if (PhotonNetwork.isMasterClient)
            {
                GameObject.FindObjectsOfType<onlinePlayerSelect>().Single(p => p.Master).GetAllPlayers();
                stream.SendNext();
            }
        }
        else
        {
            if (!PhotonNetwork.isMasterClient)
            {

            }
        }
        */
    }
}
