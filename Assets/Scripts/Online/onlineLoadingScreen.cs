using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using log4net;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class onlineLoadingScreen : Photon.PunBehaviour
{

    public Text LoadingText;
    public Text DiaryText;
    public double LoadingMs = 1.500d;
    public string LevelName;

    private double _loadingTextCounter;
    private const string Loading = "Loading";
    private const string NoRooms = "No rooms found, creating";
    private List<string> _diaries = new List<string>();
    private System.Random _rand = new System.Random();
    private bool _ready = false;

    private bool creating = false;
    private double creatingCounter = 0.000d;
    public double CreateMs = 3.000d;

    private readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    void Start()
    {
        GameObject speaker = GameObject.Find("Speaker");
        if (speaker != null) speaker.GetComponent<AudioSource>().Stop();

        var tips = Resources.Load("tips") as TextAsset;

        using (var reader = XmlReader.Create(new StringReader(tips.text)))
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name.Equals("tip"))
                    {
                        if (reader.Read())
                        {
                            _diaries.Add(reader.Value.Trim());
                        }
                    }
                }
            }
        }

        var count = _rand.Next(0, _diaries.Count);

        DiaryText.text = _diaries[count];

        if (PhotonNetwork.inRoom)
        {
            _ready = true;
            return;
        }

        if (!onlineHelper.Joining)
        {
            PhotonNetwork.CreateRoom(onlineHelper.LobbyName, new RoomOptions() { maxPlayers = 4 }, null);
        }
        else
        {
            var roomCount = PhotonNetwork.countOfRooms;

            Log.DebugFormat("{0} rooms online.", roomCount);

            if (roomCount <= 0)
            {
                // No rooms online, create
                LoadingText.text = "No rooms found, creating..";
                creating = true;
            }
            else
            {
                // Join lobby
                PhotonNetwork.JoinRandomRoom();
            }
        }
    }

    private void CheckCreating()
    {
        if (creating)
        {
            creatingCounter += 1 * Time.deltaTime;
            if (creatingCounter >= CreateMs)
            {
                creatingCounter = 0;
                creating = false;
                SceneManager.LoadScene("CreateLobby");
            }
        }
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public override void OnJoinedRoom()
    {
        _ready = true;
    }

    void Update()
    {
        CheckCreating();
        AnimateLoadingText(!creating ? Loading : NoRooms);
        if (_ready && !creating)
        {
            LoadingText.text = "Loaded";

            PhotonNetwork.LoadLevel(LevelName);
        }
    }

    private void AnimateLoadingText(string message)
    {
        _loadingTextCounter += 1 * Time.deltaTime;
        if (_loadingTextCounter >= LoadingMs)
        {
            AddDot(message);
            _loadingTextCounter = 0.000d;
        }

    }

    private void AddDot(string message)
    {
        if (LoadingText.text.Count(c => c == '.') == 3)
        {
            LoadingText.text = message;
        }
        else
        {
            LoadingText.text += ".";
        }
    }
}
