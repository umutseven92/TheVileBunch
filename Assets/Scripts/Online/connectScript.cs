using UnityEngine;
using UnityEngine.UI;

public class connectScript : MonoBehaviour
{
    public Button[] Buttons;

    // Use this for initialization
    void Start()
    {
        DisableButtons();

        //if (PhotonNetwork.connectionState != ConnectionState.Connected)
        //{
        //    PhotonNetwork.ConnectUsingSettings(global.GameVersion);
        //}
    }


    //public override void OnConnectedToMaster()
    //{
    //    EnableButtons();
    //}

    private void EnableButtons()
    {
        foreach (var button in Buttons)
        {
            button.interactable = true;
        }
    }

    private void DisableButtons()
    {
        //if (PhotonNetwork.connectionState != ConnectionState.Connected)
        //{
        //    foreach (var button in Buttons)
        //    {
        //        button.interactable = false;
        //    }
        //}
    }

}
