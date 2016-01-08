using UnityEngine;
using UnityEngine.UI;

public class onlineSceneSelect : sceneSelect
{
    public Text LevelText;
    public Text BackText;
    public Button[] LevelSelectButtons;

    private bool sure = false;

    protected override void Start()
    {
        base.Start();

        if (!PhotonNetwork.isMasterClient)
        {
            foreach (var button in LevelSelectButtons)
            {
                button.enabled = false;
            }
            PlayButton.enabled = false;
            BackText.text = "Quit";
        }
        else
        {
            BackText.text = "Back";
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
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.LoadLevel("OnlinePlayerSelect");
            }
            else
            {
                if (!sure)
                {
                    BackText.text = "Sure?";
                    sure = true;
                }
                else
                {
                    PhotonNetwork.Disconnect();
                    Application.LoadLevel("Menu");
                }
            }
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
