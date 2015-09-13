using System;
using UnityEngine;
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
    public Image p1Image;
    public Image p2Image;
    public Image p3Image;
    public Image p4Image;

    private Sprite cowboyImage;
    private Sprite dancerImage;
    private Sprite prospectorImage;
    private Sprite pirateImage;

    private bool kDelay = false;
    private bool j1Delay = false;
    private bool j2Delay = false;
    private bool j3Delay = false;
    private bool j4Delay = false;

    public static List<Player> PlayerList = new List<Player>();

    private List<Image> playerImages = new List<Image>();
    private List<Text> playerTexts = new List<Text>();
    private readonly string[] _classes = new[] { "The Cowboy", "The Dancer", "The Prospector", "The Pirate" };

    // Use this for initialization
    void Start()
    {
        Play.enabled = false;
        cowboyImage = Resources.Load<Sprite>("cowboy");
        dancerImage = Resources.Load<Sprite>("dancer");
        prospectorImage = Resources.Load<Sprite>("prospector");
        pirateImage = Resources.Load<Sprite>("pirate");

        playerImages.Add(p1Image);
        playerImages.Add(p2Image);
        playerImages.Add(p3Image);
        playerImages.Add(p4Image);

        playerTexts.Add(P1Text);
        playerTexts.Add(P2Text);
        playerTexts.Add(P3Text);
        playerTexts.Add(P4Text);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputs();
        UpdatePlayButton();
    }

    void CheckInputs()
    {
        CheckSubmit();
        CheckHorizontal();
        CheckCancel();
        CheckHorizontalCancel();
    }

    void CheckSubmit()
    {
        if (Input.GetButton("kStart"))
        {
            SelectPlayer("k");
        }
        if (Input.GetButton("j1Start"))
        {
            SelectPlayer("j1");
        }
        if (Input.GetButton("j2Start"))
        {
            SelectPlayer("j2");
        }
        if (Input.GetButton("j3Start"))
        {
            SelectPlayer("j3");
        }
        if (Input.GetButton("j4Start"))
        {
            SelectPlayer("j4");
        }

    }

    void CheckHorizontal()
    {
        if (Input.GetAxis("kHorizontal") > 0)
        {
            ChangePlayer("k", 1);
            kDelay = true;
        }
        else if (Input.GetAxis("kHorizontal") < 0)
        {
            ChangePlayer("k", -1);
            kDelay = true;
        }
        if (Input.GetAxis("j1Horizontal") > 0)
        {
            ChangePlayer("j1", 1);
            j1Delay = true;
        }
        else if (Input.GetAxis("j1Horizontal") < 0)
        {
            ChangePlayer("j1", -1);
            j1Delay = true;
        }
        if (Input.GetAxis("j2Horizontal") > 0)
        {
            ChangePlayer("j2", 1);
            j2Delay = true;
        }
        else if (Input.GetAxis("j2Horizontal") < 0)
        {
            ChangePlayer("j2", -1);
            j2Delay = true;
        }
        if (Input.GetAxis("j3Horizontal") > 0)
        {
            ChangePlayer("j3", 1);
            j3Delay = true;
        }
        else if (Input.GetAxis("j3Horizontal") < 0)
        {
            ChangePlayer("j3", -1);
            j3Delay = true;
        }
        if (Input.GetAxis("j4Horizontal") > 0)
        {
            ChangePlayer("j4", 1);
            j4Delay = true;
        }
        else if (Input.GetAxis("j4Horizontal") < 0)
        {
            ChangePlayer("j4", -1);
            j4Delay = true;
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

    private void CheckHorizontalCancel()
    {
        if (Input.GetAxis("kHorizontal") == 0)
        {
            kDelay = false;
        }
        if (Input.GetAxis("j1Horizontal") == 0)
        {
            j1Delay = false;
        }
        if (Input.GetAxis("j2Horizontal") == 0)
        {
            j2Delay = false;
        }
        if (Input.GetAxis("j3Horizontal") == 0)
        {
            j3Delay = false;
        }
        if (Input.GetAxis("j4Horizontal") == 0)
        {
            j4Delay = false;
        }
    }

    private void SelectPlayer(string control)
    {
        if (PlayerList.Count >= 4 || PlayerList.Any(player => player.Control == control))
        {
            return;
        }

        string pClass = string.Empty;

        foreach (var c in _classes)
        {
            var player = PlayerList.Where(pl => pl.Class == c);

            if (!player.Any())
            {
                pClass = c;
            }
        }

        if (pClass == string.Empty)
        {
            Debug.LogError("Class name null!");
        }

        Player p = new Player
        {
            Control = control,
            Class = pClass,
            Num = PlayerList.Count
        };
        PlayerList.Add(p);

        Source.PlayOneShot(Clip);

        UpdateSelect(PlayerList);
    }

    void ChangePlayer(string control, int dir)
    {
        bool check = false;

        switch (control)
        {
            case "k":
                check = kDelay;
                break;
            case "j1":
                check = j1Delay;
                break;
            case "j2":
                check = j2Delay;
                break;
            case "j3":
                check = j3Delay;
                break;
            case "j4":
                check = j4Delay;
                break;
            default:
                Debug.LogError("Control not recognized!");
                break;
        }

        if (PlayerList.All(player => player.Control != control) || check)
        {
            return;
        }

        int classPos = Array.FindIndex(_classes, c => c == PlayerList.First(p => p.Control == control).Class);

        if (classPos == 0 && dir == -1)
        {
            classPos = _classes.Length;
        }
        if (classPos == _classes.Length - 1 && dir == 1)
        {
            classPos = -1;
        }
      
        PlayerList.First(p => p.Control == control).Class = _classes[classPos + dir];

        UpdateSelect(PlayerList);

    }

    void UpdateSelect(List<Player> players)
    {
        P1Text.text = "Press Start";
        P2Text.text = "Press Start";
        P3Text.text = "Press Start";
        P4Text.text = "Press Start";

        p1Image.sprite = null;
        p2Image.sprite = null;
        p3Image.sprite = null;
        p4Image.sprite = null;

        for (int i = 0; i < PlayerList.Count; i++)
        {
            SetClassImage(players[i].Class, playerImages[i]);
            playerTexts[i].text = players[i].Class;
        }
    }

    void SetClassImage(string pClass, Image image)
    {
        image.color = Color.white;

        if (pClass.Equals("The Cowboy"))
        {
            image.sprite = cowboyImage;
        }
        else if (pClass.Equals("The Dancer"))
        {
            image.sprite = dancerImage;
        }
        else if (pClass.Equals("The Prospector"))
        {
            image.sprite = prospectorImage;
        }
        else if (pClass.Equals("The Pirate"))
        {
            image.sprite = pirateImage;
        }
    }


    private void RemovePlayer(string control)
    {
        if (PlayerList.All(player => player.Control != control))
        {
            return;
        }

        PlayerList.Remove(PlayerList.First(p => p.Control == control));

        UpdateSelect(PlayerList);
    }

    private void UpdatePlayButton()
    {
        if (PlayerList.Count >= 2)
        {
            Play.enabled = true;
            Play.Select();
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
        public int Num { get; set; }
    }

}
