using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class onlinePlayerSelect : playerSelect
{
    private bool chosen = false;
    private PhotonView pView;

    void Start()
    {
        pView = GetComponentInParent<PhotonView>();
    }

    public override void CheckInputs()
    {
        if (Input.GetButtonDown("kCancel") || Input.GetButtonDown("j1Cancel") || Input.GetButtonDown("j2Cancel") ||
            Input.GetButtonDown("j3Cancel") || Input.GetButtonDown("j4Cancel"))
        {
            if (kStage == SelectStages.Disabled && j1Stage == SelectStages.Disabled && j2Stage == SelectStages.Disabled && j3Stage == SelectStages.Disabled && j4Stage == SelectStages.Disabled)
            {
                PhotonNetwork.Disconnect();

                Application.LoadLevel("Menu");
                return;
            }
        }

        base.CheckInputs();

        if (!chosen)
        {
            CheckSubmit();
        }
    }


    protected override void AddPlayer(string control)
    {
        pView.RPC("PlayerAddRPC", PhotonTargets.All, control, pView.viewID);
    }

    protected override void AddInitialPlayer(string control, string pClass)
    {
        pView.RPC("PlayerAddInitialRPC", PhotonTargets.All, control, pClass, pView.viewID);
    }

    protected override void RemovePlayer(string control)
    {
        pView.RPC("PlayerRemoveRPC", PhotonTargets.All, control, pView.viewID);
    }

    public void OnlineRemovePlayer(string control)
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

    public void OnlineAddInitialPlayer(string control, string pClass)
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

    public void OnlineAddPlayer(string control)
    {
        Debug.Log("RPCLOBBY");
        PlayerList.Find(pl => pl.Control == control).Set = true;
        pickedClasses.Add(PlayerList.Find(p2 => p2.Control == control).Class);

        PlayPlayersAudio(Clip, control);

        UpdateSelect(PlayerList);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /*
        if (stream.isWriting)
        {
            // Our player
            stream.SendNext(PlayerList.Count);

            for (int i = 0; i < PlayerList.Count; i++)
            {
                stream.SendNext(PlayerList[i].Class);
               // stream.SendNext(PlayerList[i].Control);
                stream.SendNext(PhotonNetwork.player.ID);
                stream.SendNext(PlayerList[i].Num);
                stream.SendNext(PlayerList[i].Set);
            }
        }
        else
        {
            // Network player
            var count = (int)stream.ReceiveNext();

            var list = new List<Player>();

            for (int i = 0; i < count; i++)
            {

                var pClass = (string)stream.ReceiveNext();
                var control = (int)stream.ReceiveNext();
                var num = (int)stream.ReceiveNext();
                var set = (bool)stream.ReceiveNext();

                var player = new Player
                {
                    Class = pClass,
                    Control = control.ToString(),
                    Num = num,
                    Set = set
                };

                list.Add(player);
            }

            PlayerList = list;
        }
        */
    }

    public override void Update()
    {
        base.Update();
        UpdateSelect(PlayerList);
    }

    public override void CheckCancel()
    {
        base.CheckCancel();
        if (Input.GetButton("kCancel"))
        {
            if (kCancel)
            {
                RemovePlayer("k");
                kCancel = false;
                chosen = false;
            }
        }
        if (Input.GetButton("j1Cancel"))
        {
            if (j1Cancel)
            {
                RemovePlayer("j1");
                j1Cancel = false;
                chosen = false;
            }
        }
        if (Input.GetButton("j2Cancel"))
        {
            if (j2Cancel)
            {
                RemovePlayer("j2");
                j2Cancel = false;
                chosen = false;
            }
        }
        if (Input.GetButton("j3Cancel"))
        {
            if (j3Cancel)
            {
                RemovePlayer("j3");
                j3Cancel = false;
                chosen = false;
            }
        }
        if (Input.GetButton("j4Cancel"))
        {
            if (j4Cancel)
            {
                RemovePlayer("j4");
                j4Cancel = false;
                chosen = false;
            }
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Online:\t\t" + PhotonNetwork.playerList.Length + "/" + PhotonNetwork.room.maxPlayers + "\n" +
                        "Player List:\t" + PlayerList.Count + "/" + 4);
    }

    void CheckSubmit()
    {
        if (Input.GetButton("kStart"))
        {
            if (kSubmit)
            {
                string control = "k";
                if (kStage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == control).Class))
                    {
                        PlayPlayersAudio(InvalidClip, control);
                        return;
                    }

                    AddPlayer(control);
                    kStage = SelectStages.Chosen;
                    kCanHorizontal = false;
                    chosen = true;
                }
                if (kStage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(control);
                    kStage = SelectStages.Browse;
                    kCanHorizontal = true;
                }
                kSubmit = false;
            }
        }
        if (Input.GetButton("j1Start"))
        {
            if (j1Submit)
            {
                string control = "j1";

                if (j1Stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == control).Class))
                    {
                        PlayPlayersAudio(InvalidClip, control);
                        return;
                    }
                    AddPlayer(control);
                    j1Stage = SelectStages.Chosen;
                    j1CanHorizontal = false;
                    chosen = true;
                }
                if (j1Stage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(control);
                    j1Stage = SelectStages.Browse;
                    j1CanHorizontal = true;
                }
                j1Submit = false;

            }

        }
        if (Input.GetButton("j2Start"))
        {
            if (j2Submit)
            {
                string control = "j2";

                if (j2Stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == control).Class))
                    {
                        PlayPlayersAudio(InvalidClip, control);
                        return;
                    }

                    AddPlayer(control);
                    j2Stage = SelectStages.Chosen;
                    j2CanHorizontal = false;
                    chosen = true;

                }
                if (j2Stage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(control);
                    j2Stage = SelectStages.Browse;
                    j2CanHorizontal = true;
                }
                j2Submit = false;

            }

        }
        if (Input.GetButton("j3Start"))
        {
            if (j3Submit)
            {
                string control = "j3";

                if (j3Stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == control).Class))
                    {
                        PlayPlayersAudio(InvalidClip, control);
                        return;
                    }

                    AddPlayer(control);
                    j3Stage = SelectStages.Chosen;
                    j3CanHorizontal = false;
                    chosen = true;

                }
                if (j3Stage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(control);
                    j3Stage = SelectStages.Browse;
                    j3CanHorizontal = true;
                }
                j3Submit = false;

            }

        }
        if (Input.GetButton("j4Start"))
        {
            if (j4Submit)
            {
                string control = "j4";

                if (j4Stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == control).Class))
                    {
                        PlayPlayersAudio(InvalidClip, control);
                        return;
                    }

                    AddPlayer(control);
                    j4Stage = SelectStages.Chosen;
                    j4CanHorizontal = false;
                    chosen = true;

                }
                if (j4Stage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(control);
                    j4Stage = SelectStages.Browse;
                    j4CanHorizontal = true;
                }
                j4Submit = false;

            }
        }
    }



}
