using UnityEngine;
using System.Collections;

public class matchmaker : Photon.PunBehaviour
{
    public AudioSource musicPlayer;

    // Use this for initialization
    void Start()
    {
        GameObject speaker = GameObject.Find("Speaker");

        if (speaker != null) speaker.GetComponent<AudioSource>().Stop();

        CheckPlayerPrefs();

        var player = PhotonNetwork.Instantiate("PlayerOnline", new Vector3(1, 1, 0), Quaternion.identity, 0);
        player.GetComponent<playerControl>().Enabled = true;
    }
    private void CheckPlayerPrefs()
    {
        musicPlayer.mute = PlayerPrefs.GetInt(global.Music) != 1;
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}