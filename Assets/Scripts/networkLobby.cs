
public class networkLobby : Photon.PunBehaviour
{
    [PunRPC]
    public void PlayerAddRPC(string control, int pId)
    {
        var pView = PhotonView.Find(pId);
        pView.GetComponentInParent<onlinePlayerSelect>().OnlineAddPlayer(control);
    }

    [PunRPC]
    public void PlayerAddInitialRPC(string control, string pClass, int pId)
    {
        var pView = PhotonView.Find(pId);
        pView.GetComponentInParent<onlinePlayerSelect>().OnlineAddInitialPlayer(control, pClass);
    }

    [PunRPC]
    public void PlayerRemoveRPC(string control, int pId)
    {
        var pView = PhotonView.Find(pId);
        pView.GetComponentInParent<onlinePlayerSelect>().OnlineRemovePlayer(control);
    }

}
