
public class networkScene : Photon.PunBehaviour {

    [PunRPC]
    public void IncreaseVoteRPC(int map, int pId)
    {
        var pView = PhotonView.Find(pId);
        var level = (onlineSceneSelect.OnlineLevel) map;

        pView.GetComponentInParent<onlineSceneSelect>().IncreaseVote(level);
    }

    [PunRPC]
    public void DecreaseVoteRPC(int map, int pId)
    {
        var pView = PhotonView.Find(pId);
        var level = (onlineSceneSelect.OnlineLevel) map;

        pView.GetComponentInParent<onlineSceneSelect>().DecreaseVote(level);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}
