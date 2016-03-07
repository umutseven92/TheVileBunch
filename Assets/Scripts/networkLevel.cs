using UnityEngine;
using System.Collections.Generic;

public class networkLevel : Photon.PunBehaviour
{
    private readonly List<string> _readyPlayers = new List<string>();
    private bool _ready = false;

    [PunRPC]
    public void Ready(string id)
    {
        Debug.Log("Ready RPC - " + id + " ready");
        _readyPlayers.Add(id);
    }

    private void Update()
    {
        if (_readyPlayers.Count == playerSelect.PlayerList.Count && !_ready)
        {
            GetComponentInParent<matchmaker>().Go();
            _ready = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
