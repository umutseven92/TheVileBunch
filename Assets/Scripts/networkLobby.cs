using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class networkLobby : Photon.PunBehaviour
{
    private List<playerSelect.Player> Players = null;

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
        if (!PhotonNetwork.isMasterClient)
        {
            var pView = PhotonView.Find(pId);
            pView.GetComponentInParent<onlinePlayerSelect>().SetAllPlayers(Players);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // Our player
            if (PhotonNetwork.isMasterClient)
            {
                var players = GameObject.FindObjectsOfType<onlinePlayerSelect>().Single(p => p.Master).GetAllPlayers();

                // Player count
                stream.SendNext(players.Count);

                foreach (var p in players)
                {
                    stream.SendNext(p.Control);
                    stream.SendNext(p.Class);
                    stream.SendNext(p.Num);
                    stream.SendNext(p.OnlineControl);
                    stream.SendNext(p.Set);
                }
            }
        }
        else
        {
            // Other player
            if (!PhotonNetwork.isMasterClient)
            {
                // Player count
                var count = (int)stream.ReceiveNext();

                for (int i = 0; i < count; i++)
                {
                    var control = (string)stream.ReceiveNext();
                    var pClass = (string)stream.ReceiveNext();
                    var num = (int)stream.ReceiveNext();
                    var onlineControl = (string)stream.ReceiveNext();
                    var set = (bool)stream.ReceiveNext();

                    if (Players == null)
                    {
                        Players = new List<playerSelect.Player>();
                    }

                    if (Players.All(a => a.Control != control))
                    {
                        var player = new playerSelect.Player()
                        {
                            Control = control,
                            Class = pClass,
                            Num = num,
                            OnlineControl = onlineControl,
                            Set = set
                        };

                        Players.Add(player);
                    }

                }
            }
        }
    }
}
