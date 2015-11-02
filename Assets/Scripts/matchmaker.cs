using UnityEngine;
using System.Collections;

public class matchmaker : Photon.PunBehaviour
{

    // Use this for initialization
    void Start()
    {
        GameObject speaker = GameObject.Find("Speaker");

        if (speaker != null) speaker.GetComponent<AudioSource>().Stop();

        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        var player = PhotonNetwork.Instantiate("PlayerOnline", new Vector3(1, 1, 0), Quaternion.identity, 0);
        player.GetComponent<playerControl>().enabled = true;
    }
}