using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using log4net;
using UnityEngine.UI;
using XInputDotNetPure;

public class playerSelect : Photon.PunBehaviour
{
    public AudioClip Clip;
    public AudioClip DirClip;
    public AudioClip InvalidClip;
    public AudioClip ReadyClip;

    public Button Play;

    public Text P1Text;
    public Text P2Text;
    public Text P3Text;
    public Text P4Text;
    public Image p1Image;
    public Image p2Image;
    public Image p3Image;
    public Image p4Image;

    public Image p1Button;
    public Image p2Button;
    public Image p3Button;
    public Image p4Button;

    public AudioSource p1Audio;
    public AudioSource p2Audio;
    public AudioSource p3Audio;
    public AudioSource p4Audio;

    public Button p1Right;
    public Button p1Left;
    public Button p2Right;
    public Button p2Left;
    public Button p3Right;
    public Button p3Left;
    public Button p4Right;
    public Button p4Left;

    private Sprite cowboyImage;
    private Sprite dancerImage;
    private Sprite prospectorImage;
    private Sprite freemanImage;

    public Image NextButton;
    public Image BackButton;
    protected Sprite aButton;
    protected Sprite bButton;
    protected Sprite startButton;
    protected Sprite escButton;
    protected Sprite spaceButton;
    protected Sprite enterButton;

    protected bool kDelay = false;
    protected bool j1Delay = false;
    protected bool j2Delay = false;
    protected bool j3Delay = false;
    protected bool j4Delay = false;

    protected bool kSubmit = true;
    protected bool j1Submit = true;
    protected bool j2Submit = true;
    protected bool j3Submit = true;
    protected bool j4Submit = true;

    protected bool kCancel = true;
    protected bool j1Cancel = true;
    protected bool j2Cancel = true;
    protected bool j3Cancel = true;
    protected bool j4Cancel = true;

    protected bool kCanHorizontal = true;
    protected bool j1CanHorizontal = true;
    protected bool j2CanHorizontal = true;
    protected bool j3CanHorizontal = true;
    protected bool j4CanHorizontal = true;

    protected enum SelectStages
    {
        Browse,
        Chosen,
        Disabled
    }

    private enum ControlKeys
    {
        Controller,
        Keyboard,
    }

    private ControlKeys controlKey;

    protected SelectStages kStage = SelectStages.Disabled;
    protected SelectStages j1Stage = SelectStages.Disabled;
    protected SelectStages j2Stage = SelectStages.Disabled;
    protected SelectStages j3Stage = SelectStages.Disabled;
    protected SelectStages j4Stage = SelectStages.Disabled;

    public static List<Player> PlayerList = new List<Player>();

    private List<Image> playerImages = new List<Image>();

    [HideInInspector]
    public List<Image> playerButtons = new List<Image>();

    private List<Text> playerTexts = new List<Text>();
    private List<List<Button>> playerHorizontals = new List<List<Button>>();

    protected List<string> pickedClasses = new List<string>();
    protected List<string> _classes = new List<string>();
    private const string SelectText = "";


    // Use this for initialization
    protected virtual void Start()
    {
        Play.enabled = false;
        cowboyImage = Resources.Load<Sprite>("cowboy");
        dancerImage = Resources.Load<Sprite>("dancer");
        prospectorImage = Resources.Load<Sprite>("prospector");
        freemanImage = Resources.Load<Sprite>("freeman");

        aButton = Resources.Load<Sprite>("abutton");
        bButton = Resources.Load<Sprite>("bbutton");
        escButton = Resources.Load<Sprite>("escbutton");
        enterButton = Resources.Load<Sprite>("enterbutton");
        startButton = Resources.Load<Sprite>("startbutton");
        spaceButton = Resources.Load<Sprite>("spacebutton");

        if (Input.GetJoystickNames().Length > 0)
        {
            controlKey = ControlKeys.Controller;
            ConvertToController();
        }
        else
        {
            controlKey = ControlKeys.Keyboard;
            ConvertToKB();
        }

        _classes.Add("The Cowboy");
        _classes.Add("The Dancer");
        _classes.Add("The Prospector");
        _classes.Add("The Freeman");

        playerHorizontals.Add(new List<Button>() { p1Left, p1Right });
        playerHorizontals.Add(new List<Button>() { p2Left, p2Right });
        playerHorizontals.Add(new List<Button>() { p3Left, p3Right });
        playerHorizontals.Add(new List<Button>() { p4Left, p4Right });

        playerImages.Add(p1Image);
        playerImages.Add(p2Image);
        playerImages.Add(p3Image);
        playerImages.Add(p4Image);

        playerButtons.Add(p1Button);
        playerButtons.Add(p2Button);
        playerButtons.Add(p3Button);
        playerButtons.Add(p4Button);

        playerTexts.Add(P1Text);
        playerTexts.Add(P2Text);
        playerTexts.Add(P3Text);
        playerTexts.Add(P4Text);

        p1Button.sprite = startButton;
        p2Button.sprite = startButton;
        p3Button.sprite = startButton;
        p4Button.sprite = startButton;

        NextButton.enabled = false;
        BackButton.enabled = true;

        var speaker = GameObject.Find("Speaker");

        if (speaker != null)
        {
            var audio = speaker.GetComponent<AudioSource>();
            if (!audio.isPlaying)
            {
                audio.Play();
            }
        }
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CheckInputs();
        UpdatePlayButton();
        UpdateControlSprites();
    }

    public virtual void CheckInputs()
    {
        CheckHorizontal();
        CheckCancel();
        CheckHorizontalCancel();
        CheckSubmitCancel();
        CheckCancelCancel();
        CheckBackButton();
    }

    protected void UpdateControlSprites()
    {
        if (Input.anyKeyDown)
        {
            if (!CheckGamepadInput())
            {
                if (controlKey == ControlKeys.Controller)
                {
                    ConvertToKB();
                    controlKey = ControlKeys.Keyboard;
                }
            }
        }

        if (CheckGamepadInput() && controlKey == ControlKeys.Keyboard)
        {
            ConvertToController();
            controlKey = ControlKeys.Controller;
        }
    }

    private bool CheckGamepadInput()
    {
        return Input.GetButtonDown("jA") || Input.GetButtonDown("jB") || Input.GetButtonDown("jX") ||
               Input.GetButtonDown("jY") || Input.GetButtonDown("jStart");
    }

    protected virtual void ConvertToKB()
    {
    }

    protected virtual void ConvertToController()
    {
    }

    void CheckBackButton()
    {
        if (kStage == SelectStages.Disabled && j1Stage == SelectStages.Disabled && j2Stage == SelectStages.Disabled && j3Stage == SelectStages.Disabled && j4Stage == SelectStages.Disabled)
        {
            BackButton.enabled = true;
        }
        else
        {
            BackButton.enabled = false;
        }
    }

    void CheckCancelCancel()
    {
        if (Input.GetButtonUp("kCancel"))
        {
            kCancel = true;
        }
        if (Input.GetButtonUp("j1Cancel"))
        {
            j1Cancel = true;
        }
        if (Input.GetButtonUp("j2Cancel"))
        {
            j2Cancel = true;
        }
        if (Input.GetButtonUp("j3Cancel"))
        {
            j3Cancel = true;
        }
        if (Input.GetButtonUp("j4Cancel"))
        {
            j4Cancel = true;
        }

    }

    void CheckSubmitCancel()
    {
        if (Input.GetButtonUp("kStart"))
        {
            kSubmit = true;
        }
        if (Input.GetButtonUp("j1Start"))
        {
            j1Submit = true;
        }
        if (Input.GetButtonUp("j2Start"))
        {
            j2Submit = true;
        }
        if (Input.GetButtonUp("j3Start"))
        {
            j3Submit = true;
        }
        if (Input.GetButtonUp("j4Start"))
        {
            j4Submit = true;
        }

    }
    protected virtual void SelectInitialPlayer(string control, string inputControl = null)
    {
        if (PlayerList.Count >= 4 || PlayerList.Any(player => player.Control == control))
        {
            return;
        }

        string pClass = _classes.FirstOrDefault(c => !pickedClasses.Contains(c));

        if (pClass == string.Empty)
        {
            Debug.LogError("Class name null!");
        }

        AddInitialPlayer(control, pClass, inputControl);
    }
    protected virtual void AddInitialPlayer(string control, string pClass, string inputControl)
    {
        var p = new Player
        {
            Control = control,
            Class = pClass,
            Num = PlayerList.Count,
            Set = false
        };

        PlayerList.Add(p);

        PlayPlayersAudio(ReadyClip, control);

        UpdateSelect(PlayerList);
    }


    protected virtual void CheckHorizontal()
    {
        if (kCanHorizontal)
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

        }
        if (j1CanHorizontal)
        {
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

        }

        if (j2CanHorizontal)
        {
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

        }

        if (j3CanHorizontal)
        {
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

        }

        if (j4CanHorizontal)
        {
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
    }

    public virtual void CheckCancel()
    {

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

        int classPos = _classes.FindIndex(c => c == PlayerList.First(p => p.Control == control).Class);

        if (classPos == 0 && dir == -1)
        {
            classPos = _classes.Count;
        }
        if (classPos == _classes.Count - 1 && dir == 1)
        {
            classPos = -1;
        }

        PlayerList.Find(p => p.Control == control).Class = _classes[classPos + dir];

        PlayPlayersAudio(DirClip, control);

        UpdateSelect(PlayerList);
    }

    protected virtual void AddPlayer(string control)
    {
        PlayerList.Find(pl => pl.Control == control).Set = true;
        pickedClasses.Add(PlayerList.Find(p2 => p2.Control == control).Class);

        PlayPlayersAudio(Clip, control);

        UpdateSelect(PlayerList);
    }

    protected void PlayPlayersAudio(AudioClip clip, string control)
    {
        Player p = PlayerList.Find(pl => pl.Control == control);

        switch (PlayerList.IndexOf(p))
        {
            case 0:
                p1Audio.PlayOneShot(clip);
                break;
            case 1:
                p2Audio.PlayOneShot(clip);
                break;
            case 2:
                p3Audio.PlayOneShot(clip);
                break;
            case 3:
                p4Audio.PlayOneShot(clip);
                break;
        }

    }

    protected void UpdateSelect(List<Player> players)
    {
        P1Text.text = SelectText;
        P2Text.text = SelectText;
        P3Text.text = SelectText;
        P4Text.text = SelectText;

        p1Image.sprite = null;
        p2Image.sprite = null;
        p3Image.sprite = null;
        p4Image.sprite = null;


        for (int i = 0; i < PlayerList.Count; i++)
        {
            var player = players[i];

            SetClassImage(players[i].Class, playerImages[i]);

            if (string.IsNullOrEmpty(player.OnlinePlayerName))
            {
                playerTexts[i].text = player.Class;
            }
            else
            {
                playerTexts[i].text = string.Format("{0}\n{1}", player.Class, player.OnlinePlayerName);
            }

            if (player.Set)
            {
                if (player.Control.StartsWith("j"))
                {
                    playerButtons[i].sprite = bButton;
                }
                else
                {
                    playerButtons[i].sprite = escButton;
                }

                foreach (var h in playerHorizontals[i])
                {
                    h.enabled = false;
                    h.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
                }

            }
            else
            {
                if (player.Control.StartsWith("j"))
                {
                    playerButtons[i].sprite = startButton;
                }
                else
                {
                    playerButtons[i].sprite = spaceButton;
                }

                foreach (var h in playerHorizontals[i])
                {
                    h.enabled = true;
                    h.GetComponentInChildren<CanvasRenderer>().SetAlpha(1);
                }

            }

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
        else if (pClass.Equals("The Freeman"))
        {
            image.sprite = freemanImage;
        }
    }


    protected virtual void RemovePlayer(string control)
    {
        if (PlayerList.All(player => player.Control != control))
        {
            return;
        }
        Player playerToRemove = PlayerList.First(p => p.Control == control);

        switch (control)
        {
            case "k":
                if (kStage == SelectStages.Browse)
                {
                    kStage = SelectStages.Disabled;
                    kCanHorizontal = false;
                    PlayerList.Remove(playerToRemove);
                }
                if (kStage == SelectStages.Chosen)
                {
                    kStage = SelectStages.Browse;
                    kCanHorizontal = true;
                    pickedClasses.Remove(playerToRemove.Class);
                    playerToRemove.Set = false;
                }
                break;

            case "j1":
                if (j1Stage == SelectStages.Browse)
                {
                    j1Stage = SelectStages.Disabled;
                    j1CanHorizontal = false;
                    PlayerList.Remove(playerToRemove);
                }
                if (j1Stage == SelectStages.Chosen)
                {
                    j1Stage = SelectStages.Browse;
                    j1CanHorizontal = true;
                    pickedClasses.Remove(playerToRemove.Class);
                    playerToRemove.Set = false;
                }
                break;

            case "j2":
                if (j2Stage == SelectStages.Browse)
                {
                    j2Stage = SelectStages.Disabled;
                    j2CanHorizontal = false;
                    PlayerList.Remove(playerToRemove);
                }
                if (j2Stage == SelectStages.Chosen)
                {
                    j2Stage = SelectStages.Browse;
                    j2CanHorizontal = true;
                    pickedClasses.Remove(playerToRemove.Class);
                    playerToRemove.Set = false;

                }
                break;

            case "j3":
                if (j3Stage == SelectStages.Browse)
                {
                    j3Stage = SelectStages.Disabled;
                    j3CanHorizontal = false;
                    PlayerList.Remove(playerToRemove);
                }
                if (j3Stage == SelectStages.Chosen)
                {
                    j3Stage = SelectStages.Browse;
                    j3CanHorizontal = true;
                    pickedClasses.Remove(playerToRemove.Class);
                    playerToRemove.Set = false;
                }
                break;

            case "j4":
                if (j4Stage == SelectStages.Browse)
                {
                    j4Stage = SelectStages.Disabled;
                    j4CanHorizontal = false;
                    PlayerList.Remove(playerToRemove);
                }
                if (j4Stage == SelectStages.Chosen)
                {
                    j4Stage = SelectStages.Browse;
                    j4CanHorizontal = true;
                    pickedClasses.Remove(playerToRemove.Class);
                    playerToRemove.Set = false;
                }
                break;

            default:
                Debug.LogError(control + " not found!");
                break;

        }

        UpdateSelect(PlayerList);
    }

    protected virtual void UpdatePlayButton()
    {
        if (PlayerList.Count(o => o.Set) >= 2)
        {
            Play.enabled = true;
            NextButton.enabled = true;
            Play.Select();
        }
        else
        {
            NextButton.enabled = false;
            Play.enabled = false;
        }
    }

    public class Player
    {
        /// <summary>
        /// On single player, this is k for keyboard, or j1 through j4 for controller.
        /// On online play, this is players ID.
        /// </summary>
        public string Control { get; set; } 
        public string Class { get; set; }
        public int Num { get; set; }
        public bool Set { get; set; }
        public string OnlineControl { get; set; }
        public string OnlinePlayerName { get; set; }
        public int Ping { get; set; }
    }

}
