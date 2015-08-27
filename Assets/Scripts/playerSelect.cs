using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class playerSelect : MonoBehaviour
{

    public AudioSource source;
    public AudioClip clip;

    public Button play;
    public Text p1Text;
    public Text p2Text;
    public Text p3Text;
    public Text p4Text;

    public static int playerCount = 0;

    public static List<Player> playerList = new List<Player>();

    private string[] classes = new[] {"The Cowboy", "The Dancer", "The Doctor", "The Prospector"};

    private string firstPlace;
    private string secondPlace;
    private string thirdPlace;
    private string fourthPlace;

    // Use this for initialization
    void Start()
    {
        play.enabled = false;
        playerCount = 0;
    }

    private void SelectPlayer(string control)
    {
        if (playerCount >= 4 || playerList.Any(player => player.Control == control))
        {
            return;
        }

        playerCount++;
        Player p = new Player
        {
            Control = control,
            Num = playerCount,
            Class = classes[playerCount - 1]
        };
        playerList.Add(p);
        SetPlayerText(control);
        source.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("kSubmit"))
        {
            SelectPlayer("k");
        }
        if (Input.GetButton("j1Submit"))
        {
            SelectPlayer("j1");
        }
        if (Input.GetButton("j2Submit") || Input.GetAxis("kVertical") >= 0.5f)
        {
            SelectPlayer("j2");
        }
        if (Input.GetButton("j3Submit") || Input.GetAxis("kVertical") <= -0.5f)
        {
            SelectPlayer("j3");
        }
        if (Input.GetButton("j4Submit"))
        {
            SelectPlayer("j4");
        }

        if (Input.GetButton("kCancel"))
        {
            RemovePlayer("k");
        }
        if (Input.GetButton("j1Cancel"))
        {
            RemovePlayer("j1");
        }
        if (Input.GetButton("j2Cancel"))
        {
            RemovePlayer("j2");
        }
        if (Input.GetButton("j3Cancel"))
        {
            RemovePlayer("j3");
        }
        if (Input.GetButton("j4Cancel"))
        {
            RemovePlayer("j4");
        }

        UpdatePlayButton();
    }

    private void RemovePlayer(string control)
    {
        if (playerList.All(player => player.Control != control))
        {
            return;
        }
        playerList.Remove(playerList.First(p => p.Control == control));
        playerCount--;

        if (control == firstPlace)
        {
            p1Text.text = "Press Start";
        }
        else if (control == secondPlace)
        {
            p2Text.text = "Press Start";
        }
        else if (control == thirdPlace)
        {
            p3Text.text = "Press Start";
        }
        else if (control == fourthPlace)
        {
            p4Text.text = "Press Start";
        }

    }

    private void UpdatePlayButton()
    {
        if (playerCount >= 2)
        {
            play.enabled = true;
            play.GetComponentInChildren<CanvasRenderer>().SetAlpha(1);
            play.GetComponentInChildren<Text>().color = Color.black;
        }
        else
        {
            play.enabled = false;
            play.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
            play.GetComponentInChildren<Text>().color = Color.clear;
        }
    }

    private void SetPlayerText(string control)
    {
        switch (playerCount)
        {
            case 1:
                p1Text.text = classes[playerCount-1];
                firstPlace = control;
                break;
            case 2:
                p1Text.text = classes[playerCount-1];
                secondPlace = control;
                break;
            case 3:
                p1Text.text = classes[playerCount-1];
                thirdPlace = control;
                break;
            case 4:
                p1Text.text = classes[playerCount-1];
                fourthPlace = control;
                break;

        }
    }

    public class Player
    {
        public int Num { get; set; }
        public string Control { get; set; }
        public string Class { get; set; }
    }

}
