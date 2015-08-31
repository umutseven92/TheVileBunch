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

    private readonly string[] _classes = new[] { "The Cowboy", "The Dancer", "The Prospector", "The Amazon", "The Pirate", "The Unknown" };


    private Dictionary<int, Player> controlPosition = new Dictionary<int, Player>();

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

        string pClass = _classes[PlayerList.Count];

        Player p = new Player
        {
            Control = control,
            Class = pClass
        };
        PlayerList.Add(p);

        controlPosition[PlayerList.Count] = p;

        Source.PlayOneShot(Clip);

        UpdateSelect(controlPosition);
    }

    void ChangePlayer(string control, int dir)
    {

    }

    void UpdateSelect(Dictionary<int , Player> dict)
    {
        P1Text.text = "Press Start";
        P2Text.text = "Press Start";
        P3Text.text = "Press Start";
        P4Text.text = "Press Start";

        foreach (var i in dict)
        {
            switch (i.Key)
            {
                case 1:
                    P1Text.text = dict[i.Key].Class;
                    break; 
                case 2: 
                    P2Text.text = dict[i.Key].Class;
                    break;
                case 3:
                    P3Text.text = dict[i.Key].Class;
                    break;
                case 4:
                    P4Text.text = dict[i.Key].Class;
                    break;
            }
        }
    }

    void CheckSubmit()
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
        if (Input.GetButton("j3Submit"))
        {
            SelectPlayer("j3");
        }
        if (Input.GetButton("j4Submit"))
        {
            SelectPlayer("j4");
        }
        
    }

    void CheckHorizontal()
    {
        if (Input.GetAxis("kHorizontal") > 0)
        {
            ChangePlayer("k", 1);
        }
        else if (Input.GetAxis("kHorizontal") < 0)
        {
            ChangePlayer("k", -1);
        }
        if (Input.GetAxis("j1Horizontal") > 0)
        {
            ChangePlayer("j1", 1);
        }
        else if (Input.GetAxis("j1Horizontal") < 0)
        {
            ChangePlayer("j1", -1);
        }
        if (Input.GetAxis("j2Horizontal") > 0)
        {
            ChangePlayer("j2", 1);
        }
        else if (Input.GetAxis("j2Horizontal") < 0)
        {
            ChangePlayer("j2", -1);
        }
        if (Input.GetAxis("j3Horizontal") > 0)
        {
            ChangePlayer("j3", 1);
        }
        else if (Input.GetAxis("j3Horizontal") < 0)
        {
            ChangePlayer("j3", -1);
        }
        if (Input.GetAxis("j4Horizontal") > 0)
        {
            ChangePlayer("j4", 1);
        }

        else if (Input.GetAxis("j4Horizontal") < 0)
        {
            ChangePlayer("j4", -1);
        }
    }

    void CheckCancel()
    {
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
        
    }

    void CheckInputs()
    {
        CheckSubmit();
        CheckHorizontal();
        CheckCancel();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
        UpdatePlayButton();

    }

    private void RemovePlayer(string control)
    {
        if (PlayerList.All(player => player.Control != control))
        {
            return;
        }
        PlayerList.Remove(PlayerList.First(p => p.Control == control));

        controlPosition.Remove(controlPosition.First(p => p.Value.Control == control).Key);

        UpdateSelect(controlPosition);
    }

    private void UpdatePlayButton()
    {
        if (PlayerList.Count >= 2)
        {
            Play.enabled = true;
        }
        else
        {
            Play.enabled = false;

        }
    }


    public class Player
    {
        public string Control { get; set; }
        public string Class { get; set; }
    }

}
