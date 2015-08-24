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

    // Use this for initialization
    void Start()
    {
        play.enabled = false;
        playerCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("kSubmit"))
        {
            if (playerCount >= 4 || playerList.Any(player => player.Control == "k"))
            {
                return;
            }

            playerCount++;
            Player p = new Player
            {
                Control = "k",
                Num = playerCount
            };
            playerList.Add(p);
            SetPlayerText();
            source.PlayOneShot(clip);
        }
        else if (Input.GetButton("j1Submit"))
        {
            if (playerCount >= 4 || playerList.Any(player => player.Control == "j1"))
            {
                return;
            }
            playerCount++;
            Player p = new Player
            {
                Control = "j1",
                Num = playerCount
            };
            playerList.Add(p);

            SetPlayerText();
            source.PlayOneShot(clip);

        }
        else if (Input.GetButton("j2Submit"))
        {
            if (playerCount >= 4 || playerList.Any(player => player.Control == "j2"))
            {
                return;
            }
            playerCount++;
            Player p = new Player
            {
                Control = "j2",
                Num = playerCount
            };
            playerList.Add(p);

            SetPlayerText();
            source.PlayOneShot(clip);

        }
        else if (Input.GetButton("j3Submit"))
        {
            if (playerCount >= 4 || playerList.Any(player => player.Control == "j3"))
            {
                return;
            }
            playerCount++;
            Player p = new Player
            {
                Control = "j3",
                Num = playerCount
            };
            playerList.Add(p);

            SetPlayerText();
            source.PlayOneShot(clip);

        }
        else if (Input.GetButton("j4Submit"))
        {
            if (playerCount >= 4 || playerList.Any(player => player.Control == "j4"))
            {
                return;
            }
            playerCount++;
            Player p = new Player
            {
                Control = "j4",
                Num = playerCount
            };
            playerList.Add(p);

            SetPlayerText();
            source.PlayOneShot(clip);

        }

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

    private void SetPlayerText()
    {
        switch (playerCount)
        {
            case 1:
                p1Text.text = "The Cowboy";
                break;
            case 2:
                p2Text.text = "The Dancer";
                break;
            case 3:
                p3Text.text = "The Doctor";
                break;
            case 4:
                p4Text.text = "The Prospector";
                break;

        }
    }

    public class Player
    {
        public int Num { get; set; }
        public string Control { get; set; }
    }

}
