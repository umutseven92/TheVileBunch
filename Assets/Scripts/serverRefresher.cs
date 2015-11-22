using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class serverRefresher : MonoBehaviour {

    public RectTransform scrContent;
    public GameObject lobbyButton;

    public void RefreshServerBrowser()
    {
        if (PhotonNetwork.connectionState == ConnectionState.Connected)
        {
            ClearServerBrowser();
            PopulateServerBrowser();
        }
    }

    void ClearServerBrowser()
    {
        for (int i = 0; i < scrContent.childCount; i++)
        {
            if (scrContent.GetChild(i).name.StartsWith("lobbyButton"))
            {
                Destroy(scrContent.GetChild(i).gameObject);
            }
        }
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
