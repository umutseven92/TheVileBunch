using UnityEngine;
using UnityEngine.UI;

public class lobbyPopulator : Photon.PunBehaviour
{
    public RectTransform scrContent;
    public GameObject lobbyButton;

    private bool first = true;

    // Use this for initialization
    void Start()
    {
        if (PhotonNetwork.connectionState != ConnectionState.Connected)
        {
            PhotonNetwork.ConnectUsingSettings(global.GameVersion);
        }
    }

    void Update()
    {
        if (PhotonNetwork.connectionState == ConnectionState.Connected)
        {
            if (first)
            {
                PopulateServerBrowser();
                first = false;
            }
        }
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    void PopulateServerBrowser()
    {
        var list = PhotonNetwork.GetRoomList();
        if (list.Length == 0)
        {
            var lobby = (GameObject)Instantiate(lobbyButton, new Vector3(0, 560, 0), Quaternion.identity);
            lobby.transform.SetParent(scrContent, false);
            lobby.transform.localScale = new Vector3(1, 1, 1);
            lobby.GetComponentInChildren<Text>().text = "Empty";
            lobby.GetComponentInChildren<Text>().color = Color.gray;
            lobby.GetComponent<Button>().interactable = false;
        }
        else
        {
            for (int i = 0; i < list.Length; i++)
            {
                var lobby = (GameObject)Instantiate(lobbyButton, new Vector3(0, 560 - 120 * i, 0), Quaternion.identity);
                lobby.transform.SetParent(scrContent, false);
                lobby.transform.localScale = new Vector3(1, 1, 1);
                lobby.GetComponent<serverBrowser>().RoomName = list[i].name;
                lobby.GetComponentInChildren<Text>().text = list[i].name + " " + list[i].playerCount + "/" + list[i].maxPlayers;
            }
        }
    }
}
