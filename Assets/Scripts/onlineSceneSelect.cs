using UnityEngine;
using UnityEngine.UI;

public class onlineSceneSelect : sceneSelect
{
    public Text LevelText;

    protected override void Start()
    {
        base.Start();

        if (!PhotonNetwork.isMasterClient)
        {
            PlayButton.enabled = false;
            BackButton.enabled = false;
        }

        PlayButton.onClick.AddListener(() =>
        {
            switch (LevelText.text)
            {
                case "Dunes":
                    PhotonNetwork.LoadLevel("DunesOnlineLoading");
                    break;
                case "Caves":
                    PhotonNetwork.LoadLevel("CavesOnlineLoading");
                    break;
                default:
                    Debug.LogError(LevelText.text + " not found!");
                    break;
            }
        });
        BackButton.onClick.AddListener(() =>
        {
            PhotonNetwork.LoadLevel("OnlinePlayerSelect");
        });
    }
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("kCancel") || Input.GetButtonDown("j1Cancel") || Input.GetButtonDown("j2Cancel") ||
            Input.GetButtonDown("j3Cancel") || Input.GetButtonDown("j4Cancel"))
        {
            PhotonNetwork.LoadLevel("OnlinePlayerSelect");
        }
    }
}
