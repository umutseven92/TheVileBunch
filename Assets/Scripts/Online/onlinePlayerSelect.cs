using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class onlinePlayerSelect : playerSelect
{
    public Text BackText;

    private PhotonView pView;
    private bool sure = false;
    private bool first = true;
    private bool selected;
    private bool again;

    [HideInInspector]
    public string PlayerId
    {
        get
        {
            return PlayerPrefs.GetString(global.PlayerId);
        }
    }

    [HideInInspector]
    public bool Master
    {
        get; set;
    }

    private SelectStages stage = SelectStages.Disabled;

    protected override void Start()
    {
        base.Start();

        PhotonNetwork.automaticallySyncScene = true;

        pView = GetComponentInParent<PhotonView>();

        Play.onClick.AddListener(() =>
        {
            if (PhotonNetwork.isMasterClient)
            {
                PhotonNetwork.LoadLevel("OnlineSceneSelect");
            }
        });

        InitialSubmit("Online");
    }

    public override void CheckInputs()
    {
        if (Input.GetButtonDown("kCancel") || Input.GetButtonDown("j1Cancel") || Input.GetButtonDown("j2Cancel") ||
            Input.GetButtonDown("j3Cancel") || Input.GetButtonDown("j4Cancel"))
        {
            if (stage == SelectStages.Browse)
            {
                if (!sure)
                {
                    BackText.text = "Sure?";
                    sure = true;
                }
                else
                {
                    PhotonNetwork.Disconnect();
                    PlayerList = new List<Player>();

                    SceneManager.LoadScene("Menu");
                }
                return;
            }
        }

        base.CheckInputs();

        CheckSubmit();
    }

    protected override void ConvertToController()
    {
        foreach (var button in playerButtons)
        {
            if (button.sprite.name.Equals("spacebutton"))
            {
                button.sprite = startButton;
            }
            else
            {
                button.sprite = bButton;
            }
        }

        NextButton.sprite = aButton;
        BackButton.sprite = bButton;
    }

    protected override void ConvertToKB()
    {
        foreach (var button in playerButtons)
        {
            if (button.sprite.name.Equals("startbutton"))
            {
                button.sprite = spaceButton;
            }
            else
            {
                button.sprite = escButton;
            }
        }

        NextButton.sprite = enterButton;
        BackButton.sprite = escButton;
    }

    protected override void CheckHorizontal()
    {
        if (kCanHorizontal)
        {
            if (Input.GetAxis("kHorizontal") > 0)
            {
                ChangePlayer("k", 1, PlayerId);
                kDelay = true;
            }
            else if (Input.GetAxis("kHorizontal") < 0)
            {
                ChangePlayer("k", -1, PlayerId);
                kDelay = true;
            }

        }
        if (j1CanHorizontal)
        {
            if (Input.GetAxis("j1Horizontal") > 0)
            {
                ChangePlayer("j1", 1, PlayerId);
                j1Delay = true;
            }
            else if (Input.GetAxis("j1Horizontal") < 0)
            {
                ChangePlayer("j1", -1, PlayerId);
                j1Delay = true;
            }

        }

        if (j2CanHorizontal)
        {
            if (Input.GetAxis("j2Horizontal") > 0)
            {
                ChangePlayer("j2", 1, PlayerId);
                j2Delay = true;
            }
            else if (Input.GetAxis("j2Horizontal") < 0)
            {
                ChangePlayer("j2", -1, PlayerId);
                j2Delay = true;
            }

        }

        if (j3CanHorizontal)
        {
            if (Input.GetAxis("j3Horizontal") > 0)
            {
                ChangePlayer("j3", 1, PlayerId);
                j3Delay = true;
            }
            else if (Input.GetAxis("j3Horizontal") < 0)
            {
                ChangePlayer("j3", -1, PlayerId);
                j3Delay = true;
            }

        }

        if (j4CanHorizontal)
        {
            if (Input.GetAxis("j4Horizontal") > 0)
            {
                ChangePlayer("j4", 1, PlayerId);
                j4Delay = true;
            }
            else if (Input.GetAxis("j4Horizontal") < 0)
            {
                ChangePlayer("j4", -1, PlayerId);
                j4Delay = true;
            }

        }
    }

    void ChangePlayer(string control, int dir, string playerId)
    {
        var delay = false;

        switch (control)
        {
            case "k":
                delay = kDelay;
                break;
            case "j1":
                delay = j1Delay;
                break;
            case "j2":
                delay = j2Delay;
                break;
            case "j3":
                delay = j3Delay;
                break;
            case "j4":
                delay = j4Delay;
                break;
            default:
                Debug.LogError("Control not recognized!");
                break;
        }

        pView.RPC("PlayerChangeRPC", PhotonTargets.All, control, dir, playerId, delay, pView.viewID);
    }

    public void OnlineChangePlayer(string control, int dir, bool delay, string playerId)
    {
        if (PlayerList.All(player => player.Control != playerId) || delay)
        {
            return;
        }

        int classPos = _classes.FindIndex(c => c == PlayerList.First(p => p.Control == playerId).Class);

        if (classPos == 0 && dir == -1)
        {
            classPos = _classes.Count;
        }
        if (classPos == _classes.Count - 1 && dir == 1)
        {
            classPos = -1;
        }

        PlayerList.Find(p => p.Control == playerId).Class = _classes[classPos + dir];

        PlayPlayersAudio(DirClip, control);

        UpdateSelect(PlayerList);
    }

    protected override void AddPlayer(string control)
    {
        pView.RPC("PlayerAddRPC", PhotonTargets.All, control, pView.viewID);
    }

    protected override void AddInitialPlayer(string control, string pClass, string inputControl)
    {
        pView.RPC("PlayerAddInitialRPC", PhotonTargets.All, control, pClass, PlayerPrefs.GetString(global.PlayerName), inputControl, pView.viewID);
    }

    protected override void RemovePlayer(string control)
    {
        pView.RPC("PlayerRemoveRPC", PhotonTargets.All, control, pView.viewID);
    }

    public void SetAllPlayers(List<Player> players)
    {
        if (players == null)
        {
            again = true;
            return;
        }

        again = false;

        PlayerList = players;

        foreach (var player in PlayerList)
        {
            if (player.Set && !pickedClasses.Contains(player.Class))
            {
                pickedClasses.Add(player.Class);
            }
        }

        UpdateSelect(PlayerList);
    }

    public List<Player> GetAllPlayers()
    {
        return PlayerList;
    }

    public void OnlineRemovePlayer(string control)
    {
        if (PlayerList.All(player => player.Control != control))
        {
            return;
        }
        Player playerToRemove = PlayerList.First(p => p.Control == control);

        if (stage == SelectStages.Chosen)
        {
            stage = SelectStages.Browse;
            kCanHorizontal = true;
            j1CanHorizontal = true;
            j2CanHorizontal = true;
            j3CanHorizontal = true;
            j4CanHorizontal = true;
        }

        if (playerToRemove.Set)
        {
            pickedClasses.Remove(playerToRemove.Class);
            playerToRemove.Set = false;
        }
        else
        {
            PlayerList.Remove(playerToRemove);
        }

        UpdateSelect(PlayerList);
    }

    protected override void UpdatePlayButton()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            return;
        }

        base.UpdatePlayButton();
    }

    public void OnlineAddInitialPlayer(string control, string pClass, string playerName, string inputControl = null)
    {
        var p = new Player
        {
            Control = control,
            Class = pClass,
            Num = PlayerList.Count,
            Set = false,
            OnlineControl = inputControl,
            OnlinePlayerName = playerName
        };

        PlayerList.Add(p);

        PlayPlayersAudio(ReadyClip, control);

        UpdateSelect(PlayerList);
    }

    public void OnlineAddPlayer(string control)
    {
        PlayerList.Find(pl => pl.Control == control).Set = true;

        pickedClasses.Add(PlayerList.Find(p2 => p2.Control == control).Class);

        PlayPlayersAudio(Clip, control);

        UpdateSelect(PlayerList);
    }

    public double joinDelay = 0.500d;
    private double joinCounter = 0.600d;

    public override void Update()
    {
        if (first || again)
        {
            if (joinCounter > joinDelay)
            {
                if (!PhotonNetwork.isMasterClient)
                {
                    pView.RPC("PlayerJoinRPC", PhotonTargets.All, pView.viewID);
                }
                else
                {
                    Master = true;
                }

                first = false;
                joinCounter = 0.000d;
            }
        }

        base.Update();

        UpdateSelect(PlayerList);

        joinCounter += 1 * Time.deltaTime;
    }

    public override void CheckCancel()
    {
        base.CheckCancel();

        if (stage == SelectStages.Browse)
        {
            return;
        }

        if (Input.GetButton("kCancel"))
        {
            if (kCancel)
            {
                RemovePlayer(PlayerId);
                kCancel = false;
            }
        }
        if (Input.GetButton("j1Cancel"))
        {
            if (j1Cancel)
            {
                RemovePlayer(PlayerId);
                j1Cancel = false;
            }
        }
        if (Input.GetButton("j2Cancel"))
        {
            if (j2Cancel)
            {
                RemovePlayer(PlayerId);
                j2Cancel = false;
            }
        }
        if (Input.GetButton("j3Cancel"))
        {
            if (j3Cancel)
            {
                RemovePlayer(PlayerId);
                j3Cancel = false;
            }
        }
        if (Input.GetButton("j4Cancel"))
        {
            if (j4Cancel)
            {
                RemovePlayer(PlayerId);
                j4Cancel = false;
            }
        }
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    protected override void SelectInitialPlayer(string control, string inputControl)
    {
        if (PlayerList.Count >= 4)
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

    private void InitialSubmit(string control)
    {
        selected = true;
        SelectInitialPlayer(PlayerId, control);
        stage = SelectStages.Browse;
    }

    void CheckSubmit()
    {
        if (Input.GetButton("kStart"))
        {
            if (kSubmit)
            {
                if (stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == PlayerId).Class))
                    {
                        PlayPlayersAudio(InvalidClip, PlayerId);
                        return;
                    }

                    AddPlayer(PlayerId);
                    stage = SelectStages.Chosen;
                    kCanHorizontal = false;
                }
                kSubmit = false;
            }
        }
        if (Input.GetButton("j1Start"))
        {
            if (j1Submit)
            {
                if (stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == PlayerId).Class))
                    {
                        PlayPlayersAudio(InvalidClip, PlayerId);
                        return;
                    }
                    AddPlayer(PlayerId);
                    stage = SelectStages.Chosen;
                    j1CanHorizontal = false;
                }
                j1Submit = false;

            }

        }
        if (Input.GetButton("j2Start"))
        {
            if (j2Submit)
            {
                if (stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == PlayerId).Class))
                    {
                        PlayPlayersAudio(InvalidClip, PlayerId);
                        return;
                    }

                    AddPlayer(PlayerId);
                    stage = SelectStages.Chosen;
                    j2CanHorizontal = false;

                }
                j2Submit = false;

            }

        }
        if (Input.GetButton("j3Start"))
        {
            if (j3Submit)
            {
                if (stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == PlayerId).Class))
                    {
                        PlayPlayersAudio(InvalidClip, PlayerId);
                        return;
                    }

                    AddPlayer(PlayerId);
                    stage = SelectStages.Chosen;
                    j3CanHorizontal = false;

                }
                j3Submit = false;

            }

        }
        if (Input.GetButton("j4Start"))
        {
            if (j4Submit)
            {
                if (stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == PlayerId).Class))
                    {
                        PlayPlayersAudio(InvalidClip, PlayerId);
                        return;
                    }

                    AddPlayer(PlayerId);
                    stage = SelectStages.Chosen;
                    j4CanHorizontal = false;

                }
                j4Submit = false;

            }
        }
    }
}
