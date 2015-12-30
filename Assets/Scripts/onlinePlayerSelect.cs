﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class onlinePlayerSelect : playerSelect
{
    private PhotonView pView;

    private string _playerId;

    [HideInInspector]
    public string PlayerId
    {
        get
        {
            if (_playerId == null)
            {
                _playerId = Guid.NewGuid().ToString();
            }
            return _playerId;
        }
    }

    protected override void Start()
    {
        base.Start();
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

        CheckSubmit();
    }

    protected override void CheckHorizontal()
    {
        if (kCanHorizontal)
        {
            if (Input.GetAxis("kHorizontal") > 0)
            {
                ChangePlayer("k", 1, _playerId);
                kDelay = true;
            }
            else if (Input.GetAxis("kHorizontal") < 0)
            {
                ChangePlayer("k", -1, _playerId);
                kDelay = true;
            }

        }
        if (j1CanHorizontal)
        {
            if (Input.GetAxis("j1Horizontal") > 0)
            {
                ChangePlayer("j1", 1, _playerId);
                j1Delay = true;
            }
            else if (Input.GetAxis("j1Horizontal") < 0)
            {
                ChangePlayer("j1", -1, _playerId);
                j1Delay = true;
            }

        }

        if (j2CanHorizontal)
        {
            if (Input.GetAxis("j2Horizontal") > 0)
            {
                ChangePlayer("j2", 1, _playerId);
                j2Delay = true;
            }
            else if (Input.GetAxis("j2Horizontal") < 0)
            {
                ChangePlayer("j2", -1, _playerId);
                j2Delay = true;
            }

        }

        if (j3CanHorizontal)
        {
            if (Input.GetAxis("j3Horizontal") > 0)
            {
                ChangePlayer("j3", 1, _playerId);
                j3Delay = true;
            }
            else if (Input.GetAxis("j3Horizontal") < 0)
            {
                ChangePlayer("j3", -1, _playerId);
                j3Delay = true;
            }

        }

        if (j4CanHorizontal)
        {
            if (Input.GetAxis("j4Horizontal") > 0)
            {
                ChangePlayer("j4", 1, _playerId);
                j4Delay = true;
            }
            else if (Input.GetAxis("j4Horizontal") < 0)
            {
                ChangePlayer("j4", -1, _playerId);
                j4Delay = true;
            }

        }
    }
    void ChangePlayer(string control, int dir, string playerId)
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

        if (PlayerList.All(player => player.Control != playerId) || check)
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
        pView.RPC("PlayerAddInitialRPC", PhotonTargets.All, control, pClass, inputControl, pView.viewID);
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

        switch (playerToRemove.OnlineControl)
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

    public void OnlineAddInitialPlayer(string control, string pClass, string inputControl = null)
    {
        var p = new Player
        {
            Control = control,
            Class = pClass,
            Num = PlayerList.Count,
            Set = false,
            OnlineControl = inputControl
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
        GUILayout.Label("Online:\t\t" + PhotonNetwork.playerList.Length + "/" + PhotonNetwork.room.maxPlayers + "\n" +
                        "Player List:\t" + PlayerList.Count + "/" + 4);
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

    void CheckSubmit()
    {
        if (Input.GetButton("kStart"))
        {
            if (kSubmit)
            {
                if (kStage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == PlayerId).Class))
                    {
                        PlayPlayersAudio(InvalidClip, PlayerId);
                        return;
                    }

                    AddPlayer(PlayerId);
                    kStage = SelectStages.Chosen;
                    kCanHorizontal = false;
                }
                if (kStage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(PlayerId, "k");
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
                if (j1Stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == PlayerId).Class))
                    {
                        PlayPlayersAudio(InvalidClip, PlayerId);
                        return;
                    }
                    AddPlayer(PlayerId);
                    j1Stage = SelectStages.Chosen;
                    j1CanHorizontal = false;
                }
                if (j1Stage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(PlayerId, "j1");
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
                if (j2Stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == PlayerId).Class))
                    {
                        PlayPlayersAudio(InvalidClip, PlayerId);
                        return;
                    }

                    AddPlayer(PlayerId);
                    j2Stage = SelectStages.Chosen;
                    j2CanHorizontal = false;

                }
                if (j2Stage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(PlayerId, "j2");
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
                if (j3Stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == PlayerId).Class))
                    {
                        PlayPlayersAudio(InvalidClip, PlayerId);
                        return;
                    }

                    AddPlayer(PlayerId);
                    j3Stage = SelectStages.Chosen;
                    j3CanHorizontal = false;

                }
                if (j3Stage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(PlayerId, "j3");
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
                if (j4Stage == SelectStages.Browse)
                {
                    if (pickedClasses.Count > 0 && pickedClasses.Contains(PlayerList.Find(c => c.Control == PlayerId).Class))
                    {
                        PlayPlayersAudio(InvalidClip, PlayerId);
                        return;
                    }

                    AddPlayer(PlayerId);
                    j4Stage = SelectStages.Chosen;
                    j4CanHorizontal = false;

                }
                if (j4Stage == SelectStages.Disabled)
                {
                    SelectInitialPlayer(PlayerId, "j4");
                    j4Stage = SelectStages.Browse;
                    j4CanHorizontal = true;

                }
                j4Submit = false;

            }
        }
    }



}
