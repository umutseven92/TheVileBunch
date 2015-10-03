using UnityEngine;
using System.Collections;

public class matchmaker : Photon.PunBehaviour {

	// Use this for initialization
	void Start ()
    {
        GameObject speaker = GameObject.Find("Speaker");
        speaker.GetComponent<AudioSource>().Stop();

        Debug.Log("Connection to master..");
	    PhotonNetwork.ConnectUsingSettings("0.1");
	}

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Trying to join random room..");
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Cant join random room. Creating a new room..");
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(string.Format("Connected to room {0}",PhotonNetwork.room.name));
    }
}