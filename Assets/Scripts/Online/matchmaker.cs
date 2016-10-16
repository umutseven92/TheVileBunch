using UnityEngine;
using System.Linq;
using log4net;
using UnityEngine.Networking;
using UnityEngine.UI;
using XInputDotNetPure;

public class matchmaker : NetworkBehaviour
{
	public GameObject Fade;
	private const double MAX_ALPHA = 255f;
	private double alphaPerSec;
	public float SlowMoScale = 0.2f;

	public AudioSource musicPlayer;
	public AudioSource dingPlayer;
	public Canvas multiCanvas;
	public Canvas scoreCanvas;
	public Canvas pauseCanvas;
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


	private double _slowMoCounter = 0.000d;
	public double SlowMoMs = 1.5d;

	private bool paused;
	public Vector3 PlayerOneSpawn;
	public Vector3 PlayerTwoSpawn;
	public Vector3 PlayerThreeSpawn;
	public Vector3 PlayerFourSpawn;
	public Button btnExit;

	private onlinePlayer comp;

	private double _counter = -1.000d;
	public double scoreCardMs = 3.000d;

	private string winner;
	private bool slowMo;
	private bool gameOver;
	private bool start;

	private bool started = false;

	private readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

	// Use this for initialization
	void Start()
	{
		if (Time.timeScale < 1.0f)
		{
			Time.timeScale = 1f;
		}
		multiCanvas.enabled = false;
		scoreCanvas.enabled = false;
		pauseCanvas.enabled = false;

		return;
		SetPlayerMenuValues();

		pId = PlayerPrefs.GetString(global.PlayerId);

		var speaker = GameObject.Find("Speaker");

		if (speaker != null) speaker.GetComponent<AudioSource>().Stop();

		CheckPlayerPrefs();

		playerSelect.PlayerList.ForEach(p =>
		{
			if (p.Control == pId)
			{
				var pos = new Vector3();

				// Player number
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
			   
				// Player class
				switch (p.Class)
				{
					case "The Cowboy":
						// Spawn here..
						break;
					case "The Dancer":
						break;
					case "The Prospector":
						break;
					case "The Freeman":
						break;
					default:
						// BUG
						Debug.LogError(string.Format("Player class {0} does not exist!", p.Class));
						break;

				}

				comp = _player.GetComponent<onlinePlayer>();
				comp.Control = p.OnlineControl;
				Debug.Log(p.OnlineControl);
				comp._playerClass = p.Class;
				comp.playerNum = p.Num;
				comp.AmmoText.enabled = false;

				_player.GetComponent<Rigidbody2D>().isKinematic = false;

				comp.OnlinePlayerName = PlayerPrefs.GetString(global.PlayerName);
				comp.OnlineNameText.text = PlayerPrefs.GetString(global.PlayerName);
				comp._slashCol.SendMessage("GetPlayerNum", comp.playerNum);
			}
		});

		alphaPerSec = (MAX_ALPHA / (SlowMoMs * (1 / SlowMoScale)) / 50);

	}

	public void Go()
	{
		Log.Info(pId + " GO");
		SetScoreCard();
	}

	void SetScoreCard()
	{
		musicPlayer.Pause();
		dingPlayer.loop = true;
		dingPlayer.Play();
		start = true;
	}

	private void ScoreCardTimer()
	{
		if (start)
		{
			_counter += 1 * Time.deltaTime;

			if (_counter >= 0 && _counter < 1.000)
			{
				scoreCanvas.enabled = true;
				started = true;
				CountdownText.text = "3";
			}
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
				start = false;
				musicPlayer.UnPause();
				comp.Enabled = true;
				_counter = 0.000d;
			}
		}
	}

	void CheckTimers()
	{
		ScoreCardTimer();
		SlowMotionTimer();
	}

	void SetPlayerMenuValues()
	{
		playerNames = new[] { txtP1, txtP2, txtP3, txtP4 };
		playerClasses = new[] { txtP1Class, txtP2Class, txtP3Class, txtP4Class };
		playerPings = new[] { txtP1Ping, txtP2Ping, txtP3Ping, txtP4Ping };
	}

	void Update()
	{
		return;
		CheckTimers();
		SetCanvas();
		CheckInputs();

		if (!started)
		{
			return;
		}

		GameObject[] players = GameObject.FindGameObjectsWithTag("Player").Where(p => p.transform.position != Vector3.zero).ToArray();

		if (players.Length == 1)
		{
			var winnerComp = players[0].GetComponent<onlinePlayer>();

			winner = winnerComp.OnlineNameText.text;
			slowMo = true;
			gameOver = true;
		}
	}

	private void CheckInputs()
	{
		if (scoreCanvas.enabled || paused)
		{
			return;
		}

		if (Input.GetButtonDown("Tab"))
		{
			if (!paused && !scoreCanvas.enabled)
			{
				multiCanvas.enabled = true;
			}
		}
		else if (Input.GetButtonUp("Tab"))
		{
			multiCanvas.enabled = false;
		}

		if (Input.GetButtonDown("Pause") && scoreCanvas.enabled == false)
		{
			if (!scoreCanvas.enabled && !multiCanvas.enabled)

				if (paused)
				{
					comp.Enabled = true;
					GameObject myEventSystem = GameObject.Find("EventSystem");
					myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

					pauseCanvas.enabled = false;
				}
				else
				{
					comp.Enabled = false;
					pauseCanvas.enabled = true;
					btnExit.Select();
				}
			paused = !paused;
		}
	}

	void SetCanvas()
	{

		// Fill player list
		for (int i = 0; i < playerSelect.PlayerList.Count; i++)
		{
			var player = playerSelect.PlayerList[i];

			playerNames[i].text = player.OnlinePlayerName;
			playerClasses[i].text = player.Class;
			playerPings[i].text = player.Ping.ToString();
		}

		/*
		// Clear unused lines
		for (int i = 0; i < PhotonNetwork.room.maxPlayers - PhotonNetwork.playerList.Length; i++)
		{
			playerNames[PhotonNetwork.room.maxPlayers - i - 1].text = string.Empty;
			playerClasses[PhotonNetwork.room.maxPlayers - i - 1].text = string.Empty;
			playerPings[PhotonNetwork.room.maxPlayers - i - 1].text = string.Empty;
		}
		*/
	}

	private void CheckPlayerPrefs()
	{
		musicPlayer.mute = PlayerPrefs.GetInt(global.Music) != 1;
	}

	void OnGUI()
	{
	}

	private void SlowMotionTimer()
	{
		if (slowMo)
		{
			Time.timeScale = SlowMoScale;
			musicPlayer.pitch = 0.5f;
			_slowMoCounter += 1 * Time.deltaTime;

			var tmp = Fade.GetComponent<SpriteRenderer>().color;
			tmp.a += float.Parse(alphaPerSec.ToString()) * Time.deltaTime;

			Fade.GetComponent<SpriteRenderer>().color = tmp;

			if (_slowMoCounter >= SlowMoMs)
			{
				Time.timeScale = 1f;
				musicPlayer.pitch = 1f;
				slowMo = false;
				_slowMoCounter = 0.000d;

				onlineSceneHelper.Winner = winner;

				StopVibration();

				//PhotonNetwork.LoadLevel("GraveyardOnline");
			}
		}
	}

	private void StopVibration()
	{
		GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
		GamePad.SetVibration(PlayerIndex.Two, 0f, 0f);
		GamePad.SetVibration(PlayerIndex.Three, 0f, 0f);
		GamePad.SetVibration(PlayerIndex.Four, 0f, 0f);
	}


	//public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
	//{
	//	Debug.Log("Player disconnected: " + player.name);
	//	var dc = ((onlinePlayer)player.TagObject);
	//	dc.Enabled = false;
	//	dc.Spawn = 0;
	//	dc.Health = 0;
	//}
}