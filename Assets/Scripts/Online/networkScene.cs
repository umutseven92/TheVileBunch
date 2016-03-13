using UnityEngine;
using System.Collections;

public class networkScene : Photon.PunBehaviour {

    [PunRPC]
    public void IncreaseVoteRpc(onlineSceneSelect.OnlineLevel map, int pId)
    {
        var pView = PhotonView.Find(pId);
        pView.GetComponentInParent<onlineSceneSelect>().IncreaseVote(map);
    }

    [PunRPC]
    public void DecreaseVoteRpc(onlineSceneSelect.OnlineLevel map, int pId)
    {
        var pView = PhotonView.Find(pId);
        pView.GetComponentInParent<onlineSceneSelect>().DecreaseVote(map);
    }

}
