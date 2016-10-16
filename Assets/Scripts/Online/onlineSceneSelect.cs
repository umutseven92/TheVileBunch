using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class onlineSceneSelect : sceneSelect
{
    public Text BackText;
    private readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private bool sure = false;

    public Button DunesButton;
    public Button CavesButton;
    public Button SaloonButton;
    public Button MountainButton;

    public Text DunesText;
    public Text CavesText;
    public Text SaloonText;
    public Text MountainText;
    public Text Timer;

    private int _dunesVote;
    private int _cavesVote;
    private int _saloonVote;
    private int _mountainVote;
    public int Time;

    private bool voted;
    private double timerCounter;
    private bool done = false;

  //  private OnlineLevel VotedOn;


    //protected override void Start()
    //{
    //    base.Start();

    //    timerCounter = Time;
    //    _pView = GetComponentInParent<PhotonView>();

    //    if (!PhotonNetwork.isMasterClient)
    //    {
    //        BackText.text = "Quit";
    //    }

    //    BackButton.onClick.AddListener(() =>
    //    {
    //        if (PhotonNetwork.isMasterClient)
    //        {
    //            PhotonNetwork.LoadLevel("OnlinePlayerSelect");
    //        }
    //        else
    //        {
    //            if (!sure)
    //            {
    //                BackText.text = "Sure?";
    //                sure = true;
    //            }
    //            else
    //            {
    //                PhotonNetwork.Disconnect();
    //                SceneManager.LoadScene("Menu");
    //            }
    //        }
    //    });

    //    DunesButton.onClick.AddListener(() =>
    //    {
    //        if (voted)
    //        {
    //            _pView.RPC("DecreaseVoteRPC", PhotonTargets.All, (int)VotedOn, _pView.viewID);
    //        }
    //        _pView.RPC("IncreaseVoteRPC", PhotonTargets.All, (int)OnlineLevel.Dunes, _pView.viewID);

    //        VotedOn = OnlineLevel.Dunes;
    //        voted = true;
    //    });

    //    CavesButton.onClick.AddListener(() =>
    //    {
    //        if (voted)
    //        {
    //            _pView.RPC("DecreaseVoteRPC", PhotonTargets.All, (int)VotedOn, _pView.viewID);
    //        }
    //        _pView.RPC("IncreaseVoteRPC", PhotonTargets.All, (int)OnlineLevel.Caves, _pView.viewID);

    //        VotedOn = OnlineLevel.Caves;
    //        voted = true;
    //    });

    //    SaloonButton.onClick.AddListener(() =>
    //    {
    //        if (voted)
    //        {
    //            _pView.RPC("DecreaseVoteRPC", PhotonTargets.All, (int)VotedOn, _pView.viewID);
    //        }

    //        _pView.RPC("IncreaseVoteRPC", PhotonTargets.All, (int)OnlineLevel.Saloon, _pView.viewID);

    //        VotedOn = OnlineLevel.Saloon;
    //        voted = true;
    //    });

    //    MountainButton.onClick.AddListener(() =>
    //    {
    //        if (voted)
    //        {
    //            _pView.RPC("DecreaseVoteRPC", PhotonTargets.All, (int)VotedOn, _pView.viewID);
    //        }

    //        _pView.RPC("IncreaseVoteRPC", PhotonTargets.All, (int)OnlineLevel.Mountain, _pView.viewID);

    //        VotedOn = OnlineLevel.Mountain;
    //        voted = true;
    //    });
    //}

    //protected override void Update()
    //{
    //    base.Update();
    //    UpdateVoteTexts();
    //    CheckTimer();
    //}

    //void UpdateVoteTexts()
    //{
    //    DunesText.text = _dunesVote.ToString();
    //    CavesText.text = _cavesVote.ToString();
    //    SaloonText.text = _saloonVote.ToString();
    //    MountainText.text = _mountainVote.ToString();

    //    var time = Convert.ToInt32(timerCounter);

    //    if (time <= 5)
    //    {
    //        Timer.color = Color.red;
    //    }

    //    Timer.text = time.ToString();
    //}


    //void CheckTimer()
    //{
    //    if (!done)
    //    {
    //        if (_cavesVote + _dunesVote + _mountainVote + _saloonVote >= playerSelect.PlayerList.Count && timerCounter > 5)
    //        {
    //            timerCounter = 5;
    //        }

    //        timerCounter -= 1 * UnityEngine.Time.deltaTime;
    //        if (timerCounter <= 0)
    //        {
    //            if (PhotonNetwork.isMasterClient)
    //            {
    //                done = true;
    //                ChooseLevel();
    //            }
    //        }
    //    }
    //}

    //private void ChooseLevel()
    //{
    //    var dict = new Dictionary<OnlineLevel, int>
    //    {
    //        {OnlineLevel.Dunes, _dunesVote},
    //        {OnlineLevel.Caves, _cavesVote},
    //        {OnlineLevel.Saloon, _saloonVote},
    //        {OnlineLevel.Mountain, _mountainVote}
    //    };

    //    var map = dict.Where(a => a.Value == dict.Values.Max()).ToList();

    //    if (map.Count > 1)
    //    {
    //        var rand = new Random();
    //        var val = rand.Next(0, map.Count);

    //        LoadLevel(map[val].Key);
    //    }
    //    else
    //    {
    //        LoadLevel(map[0].Key);
    //    }
    //}

    //void OnGUI()
    //{
    //    GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    //}

    //private void LoadLevel(OnlineLevel map)
    //{
    //    switch (map)
    //    {
    //        case OnlineLevel.Dunes:
    //            PhotonNetwork.LoadLevel("DunesOnlineLoading");
    //            break;
    //        case OnlineLevel.Caves:
    //            PhotonNetwork.LoadLevel("CavesOnlineLoading");
    //            break;
    //        case OnlineLevel.Saloon:
    //            PhotonNetwork.LoadLevel("SaloonOnlineLoading");
    //            break;
    //        case OnlineLevel.Mountain:
    //            PhotonNetwork.LoadLevel("MountainOnlineLoading");
    //            break;
    //    }
    //}

    //public void IncreaseVote(OnlineLevel map)
    //{
    //    switch (map)
    //    {
    //        case OnlineLevel.Dunes:
    //            _dunesVote++;
    //            break;
    //        case OnlineLevel.Caves:
    //            _cavesVote++;
    //            break;
    //        case OnlineLevel.Saloon:
    //            _saloonVote++;
    //            break;
    //        case OnlineLevel.Mountain:
    //            _mountainVote++;
    //            break;
    //    }
    //}

    //public void DecreaseVote(OnlineLevel map)
    //{
    //    switch (map)
    //    {
    //        case OnlineLevel.Dunes:
    //            _dunesVote--;
    //            break;
    //        case OnlineLevel.Caves:
    //            _cavesVote--;
    //            break;
    //        case OnlineLevel.Saloon:
    //            _saloonVote--;
    //            break;
    //        case OnlineLevel.Mountain:
    //            _mountainVote--;
    //            break;
    //    }
    //}

    //public enum OnlineLevel
    //{
    //    Dunes,
    //    Caves,
    //    Saloon,
    //    Mountain
    //}
}
