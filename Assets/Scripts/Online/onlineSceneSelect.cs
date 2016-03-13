using log4net;
using UnityEngine;
using UnityEngine.UI;

public class onlineSceneSelect : sceneSelect
{
    public Text BackText;
    public Button[] LevelSelectButtons;

    private readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private bool sure = false;

    protected override void Start()
    {
        base.Start();

        if (!PhotonNetwork.isMasterClient)
        {
            BackText.text = "Quit";

            foreach (var button in LevelSelectButtons)
            {
                button.enabled = false;
            }
        }

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

}
