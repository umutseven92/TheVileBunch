using System.Collections.Generic;
using System.Linq;
using log4net;

public class networkLobby : Photon.PunBehaviour
{
    public List<playerSelect.Player> OnlinePlayers { get; set; }

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
    public void InitialAddToPlayerListRPC(playerSelect.Player player)
    {
        OnlinePlayers.Add(player);
    }

    [PunRPC]
    public void AddToPlayerListRPC(string control)
    {
        OnlinePlayers.Find(pl => pl.Control == control).Set = true;
    }

    [PunRPC]
    public void ChangeToPlayerListRPC(string playerId, string playerClass)
    {
        OnlinePlayers.Find(p => p.Control == playerId).Class = playerClass;
    }

    [PunRPC]
    public void RemoveFromPlayerListRPC(string control)
    {
        var toRemove = OnlinePlayers.First(p => p.Control == control);
        OnlinePlayers.Remove(toRemove);
    }

    [PunRPC]
    public void RemoveSetFromPlayerListRPC(string control)
    {
        var toRemove = OnlinePlayers.First(p => p.Control == control);
        toRemove.Set = false;
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
        var pView = PhotonView.Find(pId);
        pView.GetComponentInParent<onlinePlayerSelect>().GetAllPlayers(OnlinePlayers);

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }
}
