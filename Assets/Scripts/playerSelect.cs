using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class playerSelect : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip Clip;
    public Button Play;
    public Text P1Text;
    public Text P2Text;
    public Text P3Text;
    public Text P4Text;

    public static List<Player> PlayerList = new List<Player>();

    private readonly string[] _classes = new[] {"The Cowboy", "The Dancer", "The Doctor", "The Prospector"};

    private string _firstPlace;
    private string _secondPlace;
    private string _thirdPlace;
    private string _fourthPlace;

    // Use this for initialization
    void Start()
    {
        Play.enabled = false;
    }

    private void SelectPlayer(string control)
    {
        if (PlayerList.Count >= 4 || PlayerList.Any(player => player.Control == control))
        {
            return;
        }

        Player p = new Player
        {
            Control = control,
            Class = _classes[PlayerList.Count]
        };
        PlayerList.Add(p);
        SetPlayerText(control);
        Source.PlayOneShot(Clip);
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
        if (Input.GetButton("j2Submit"))
        {
            SelectPlayer("j2");
        }
        if (Input.GetButton("j3Submit") )
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
        if (PlayerList.All(player => player.Control != control))
        {
            return;
        }
        PlayerList.Remove(PlayerList.First(p => p.Control == control));

        if (control == _firstPlace)
        {
            P1Text.text = "Press Start";
        }
        else if (control == _secondPlace)
        {
            P2Text.text = "Press Start";
        }
        else if (control == _thirdPlace)
        {
            P3Text.text = "Press Start";
        }
        else if (control == _fourthPlace)
        {
            P4Text.text = "Press Start";
        }

    }

    private void UpdatePlayButton()
    {
        if (PlayerList.Count>= 2)
        {
            Play.enabled = true;
            Play.Select();
        }
        else
        {
            Play.enabled = false;
            
        }
    }

    private void SetPlayerText(string control)
    {
        switch (PlayerList.Count)
        {
            case 1:
                P1Text.text = _classes[PlayerList.Count-1];
                _firstPlace = control;
                break;
            case 2:
                P2Text.text = _classes[PlayerList.Count-1];
                _secondPlace = control;
                break;
            case 3:
                P3Text.text = _classes[PlayerList.Count-1];
                _thirdPlace = control;
                break;
            case 4:
                P4Text.text = _classes[PlayerList.Count-1];
                _fourthPlace = control;
                break;
            default:
                Debug.LogError("Player count not between 1 and 5!");
                break;
        }
    }

    public class Player
    {
        public string Control { get; set; }
        public string Class { get; set; }
    }

}
