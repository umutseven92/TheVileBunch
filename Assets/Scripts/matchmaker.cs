using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;

public class matchmaker : Photon.PunBehaviour
{
    public AudioSource musicPlayer;
    public AudioSource dingPlayer;
    public Canvas multiCanvas;
    public Canvas scoreCanvas;
    public Text CountdownText;

    public Text txtP1;
    public Text txtP2;
    public Text txtP3;
    public Text txtP4;
    public Text txtP1Class;
    public Text txtP2Class;
    public Text txtP3Class;
    public Text txtP4Class;
    public Text txtP1Ping;
    public Text txtP2Ping;
    public Text txtP3Ping;
    public Text txtP4Ping;

    private Text[] playerNames;
    private Text[] playerClasses;
    private Text[] playerPings;

    [HideInInspector]
    public GameObject _player;

    [HideInInspector]
    public string pId;

    private PhotonView _pView;


    public Vector3 PlayerOneSpawn;
    public Vector3 PlayerTwoSpawn;
    public Vector3 PlayerThreeSpawn;
    public Vector3 PlayerFourSpawn;

    private double _counter = 0.000d;
    private double scoreCardMs = 3.000d;

    // Use this for initialization
    void Start()
    {
        multiCanvas.enabled = false;
        scoreCanvas.enabled = false;

        SetPlayerMenuValues();
        _pView = GetComponentInParent<PhotonView>();

        pId = PlayerPrefs.GetString(global.PlayerId);

        var speaker = GameObject.Find("Speaker");

        if (speaker != null) speaker.GetComponent<AudioSource>().Stop();

        CheckPlayerPrefs();

        playerSelect.PlayerList.ForEach(p =>
        {
            if (p.Control == pId)
            {
                var pos = new Vector3();

                switch (p.Num)
                {
                    case 0:
                        pos = PlayerOneSpawn;
                        break;
                    case 1:
                        pos = PlayerTwoSpawn;
                        break;
                    case 2:
                        pos = PlayerThreeSpawn;
                        break;
                    case 3:
                        pos = PlayerFourSpawn;
                        break;
                    default:
                        // BUG HERE
                        Debug.LogError("Player number not within bounds!");
                        break;
                }

                _player = PhotonNetwork.Instantiate("PlayerOnline", pos, Quaternion.identity, 0);

                var comp = _player.GetComponent<onlinePlayer>();
                comp.Control = p.OnlineControl;
                comp._playerClass = p.Class;
                comp.playerNum = p.Num;

                _player.GetComponent<Rigidbody2D>().isKinematic = false;
                comp.OnlinePlayerName = PlayerPrefs.GetString(global.PlayerName);
                comp.OnlineNameText.text = PlayerPrefs.GetString(global.PlayerName);
                comp._slashCol.SendMessage("GetPlayerNum", comp.playerNum);
                comp.enabled = true;
            }
        });

        _pView.RPC("Ready", PhotonTargets.All, pId);
    }

    public void Go()
    {
        Debug.Log(pId + " GO");
        SetScoreCard();
    }

    void SetScoreCard()
    {
        musicPlayer.Pause();
        dingPlayer.loop = true;
        dingPlayer.Play();
        scoreCanvas.enabled = true;
    }

    private void ScoreCardTimer()
    {
        if (scoreCanvas.enabled)
        {
            _counter += 1 * Time.deltaTime;

            if (_counter >= 1.000 && _counter < 2.000)
            {
                CountdownText.text = "2";
            }
            if (_counter >= 2.000 && _counter < scoreCardMs)
            {
                CountdownText.text = "1";
            }

            if (_counter >= 2.500 && _counter < scoreCardMs)
            {
                dingPlayer.Stop();
            }

            if (_counter >= scoreCardMs)
            {
                scoreCanvas.enabled = false;
                musicPlayer.UnPause();
                var comp = _player.GetComponent<onlinePlayer>();
               // comp.enabled = true;
                _counter = 0.000d;
            }
        }
    }

    void CheckTimers()
    {
        ScoreCardTimer();
    }

    void SetPlayerMenuValues()
    {
        playerNames = new[] { txtP1, txtP2, txtP3, txtP4 };
        playerClasses = new[] { txtP1Class, txtP2Class, txtP3Class, txtP4Class };
        playerPings = new[] { txtP1Ping, txtP2Ping, txtP3Ping, txtP4Ping };
    }

    void Update()
    {
        CheckTimers();
        SetCanvas();
        CheckTimers();
    }

    private void CheckInputs()
    {
        if (scoreCanvas.enabled)
        {
            return;
        }

        if (Input.GetButtonDown("Tab"))
        {
            multiCanvas.enabled = true;
        }
        else if (Input.GetButtonUp("Tab"))
        {
            multiCanvas.enabled = false;
        }

    }

    void SetCanvas()
    {
        PhotonNetwork.playerName = _player.GetComponent<onlinePlayer>().OnlinePlayerName;

        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            playerNames[i].text = PhotonNetwork.playerList[i].name;
            playerClasses[i].text = "CLASS";
            playerPings[i].text = 45.ToString();
        }

        for (int i = 0; i < PhotonNetwork.room.maxPlayers - PhotonNetwork.playerList.Length; i++)
        {
            playerNames[PhotonNetwork.room.maxPlayers - i - 1].text = string.Empty;
            playerClasses[PhotonNetwork.room.maxPlayers - i - 1].text = string.Empty;
            playerPings[PhotonNetwork.room.maxPlayers - i - 1].text = string.Empty;
        }
    }

    private void CheckPlayerPrefs()
    {
        musicPlayer.mute = PlayerPrefs.GetInt(global.Music) != 1;
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }
}