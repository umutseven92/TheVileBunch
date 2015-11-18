using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class lobbyPopulator : Photon.PunBehaviour 
{

    public RectTransform scrContent;
    public GameObject lobbyButton;
    public double RefreshMs;

    private double _refreshCounter;


    private List<string> testList = new List<string>() { "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Eight", "Ninth", "Tenth" };

    // Use this for initialization
    void Start()
    {
        PopulateServerBrowser();
    }

    void Update()
    {
        _refreshCounter += Time.deltaTime;

        if (_refreshCounter >= RefreshMs)
        {
            RefreshServerBrowser();
            _refreshCounter = 0;
        }
    }

    void RefreshServerBrowser()
    {
        ClearServerBrowser();
        PopulateServerBrowser();
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
        for (int i = 0; i < list.Length; i++)
        {
            var lobby = (GameObject)Instantiate(lobbyButton, new Vector3(0, 560 - 120 * i, 0), Quaternion.identity);
            lobby.transform.SetParent(scrContent, false);
            lobby.transform.localScale = new Vector3(1, 1, 1);
            lobby.GetComponentInChildren<Text>().text = list[i].name;
        }

    }
}
