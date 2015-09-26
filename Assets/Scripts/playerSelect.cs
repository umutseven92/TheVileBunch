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
	private Sprite pirateImage;

	private bool kDelay = false;
	private bool j1Delay = false;
	private bool j2Delay = false;
	private bool j3Delay = false;
	private bool j4Delay = false;

	private bool kSubmit = true;
	private bool j1Submit = true;
	private bool j2Submit = true;
	private bool j3Submit = true;
	private bool j4Submit = true;

	private bool kCancel = true;
	private bool j1Cancel = true;
	private bool j2Cancel = true;
	private bool j3Cancel = true;
	private bool j4Cancel = true;

	private bool kCanHorizontal = false;
	private bool j1CanHorizontal = false;
	private bool j2CanHorizontal = false;
	private bool j3CanHorizontal = false;
	private bool j4CanHorizontal = false;

	private enum SelectStages
	{
		Browse,
		Chosen,
		Disabled
	}

	private SelectStages kStage = SelectStages.Disabled;
	private SelectStages j1Stage = SelectStages.Disabled;
	private SelectStages j2Stage = SelectStages.Disabled;
	private SelectStages j3Stage = SelectStages.Disabled;
	private SelectStages j4Stage = SelectStages.Disabled;

	public static List<Player> PlayerList = new List<Player>();

	private List<Image> playerImages = new List<Image>();
	private List<Text> playerTexts = new List<Text>();
	private List<List<Button>> playerHorizontals = new List<List<Button>>();

	private List<string> pickedClasses = new List<string>();
	private List<string> _classes = new List<string>();
	private const string SelectText = "Press Start\n(Space)";

	// Use this for initialization
	void Start()
	{
		Play.enabled = false;
		cowboyImage = Resources.Load<Sprite>("cowboy");
		dancerImage = Resources.Load<Sprite>("dancer");
		prospectorImage = Resources.Load<Sprite>("prospector");
		pirateImage = Resources.Load<Sprite>("pirate");

		_classes.Add("The Cowboy");
		_classes.Add("The Dancer");
		_classes.Add("The Prospector");
		_classes.Add("The Pirate");

		playerHorizontals.Add(new List<Button>() { p1Left, p1Right });
		playerHorizontals.Add(new List<Button>() { p2Left, p2Right });
		playerHorizontals.Add(new List<Button>() { p3Left, p3Right });
		playerHorizontals.Add(new List<Button>() { p4Left, p4Right });

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
		CheckSubmitCancel();
		CheckCancelCancel();
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

	void CheckSubmit()
	{
		if (Input.GetButton("kStart"))
		{
			if (kSubmit)
			{
				if (kStage == SelectStages.Browse)
				{
					AddPlayer("k");
					kStage = SelectStages.Chosen;
					kCanHorizontal = false;
				}
				if (kStage == SelectStages.Disabled)
				{
					SelectInitialPlayer("k");
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
					AddPlayer("j1");
					j1Stage = SelectStages.Chosen;
					j1CanHorizontal = false;
				}
				if (j1Stage == SelectStages.Disabled)
				{
					SelectInitialPlayer("j1");
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
					AddPlayer("j2");
					j2Stage = SelectStages.Chosen;
					j2CanHorizontal = false;
				}
				if (j2Stage == SelectStages.Disabled)
				{
					SelectInitialPlayer("j2");
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
					AddPlayer("j3");
					j3Stage = SelectStages.Chosen;
					j3CanHorizontal = false;
				}
				if (j3Stage == SelectStages.Disabled)
				{
					SelectInitialPlayer("j3");
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
					AddPlayer("j4");
					j4Stage = SelectStages.Chosen;
					j4CanHorizontal = false;
				}
				if (j4Stage == SelectStages.Disabled)
				{
					SelectInitialPlayer("j4");
					j4Stage = SelectStages.Browse;
					j4CanHorizontal = true;
				}
				j4Submit = false;

			}

		}

	}

	void CheckHorizontal()
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

	void CheckCancel()
	{
		if (Input.GetButton("kCancel"))
		{
			if (kCancel)
			{
				RemovePlayer("k", kStage);
				kCancel = false;
			}
		}
		if (Input.GetButton("j1Cancel"))
		{
			if (j1Cancel)
			{
				RemovePlayer("j1", j1Stage);
				j1Cancel = false;
			}
		}
		if (Input.GetButton("j2Cancel"))
		{
			if (j2Cancel)
			{
				RemovePlayer("j2", j2Stage);
				j2Cancel = false;
			}
		}
		if (Input.GetButton("j3Cancel"))
		{
			if (j3Cancel)
			{
				RemovePlayer("j3", j3Stage);
				j3Cancel = false;
			}
		}
		if (Input.GetButton("j4Cancel"))
		{
			if (j4Cancel)
			{
				RemovePlayer("j4", j4Stage);
				j4Cancel = false;
			}
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

	private void SelectInitialPlayer(string control)
	{
		if (PlayerList.Count >= 4 || PlayerList.Any(player => player.Control == control))
		{
			return;
		}

		string pClass = _classes[0];

		if (pClass == string.Empty)
		{
			Debug.LogError("Class name null!");
		}

		AddInitialPlayer(control, pClass);
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
		UpdateSelect(PlayerList);
	}

	void AddPlayer(string control)
	{
		// There is a bug here.
		PlayerList.Find(pl => pl.Control == control).Set = true;
		pickedClasses.Add(PlayerList.Find(p2 => p2.Control == control).Class);
		UpdateSelect(PlayerList);
	}

	void AddInitialPlayer(string control, string pClass)
	{
		var p = new Player
		{
			Control = control,
			Class = pClass,
			Num = PlayerList.Count,
			Set = false
		};

		PlayerList.Add(p);

		Source.PlayOneShot(Clip);
		UpdateSelect(PlayerList);
	}


	void UpdateSelect(List<Player> players)
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
			SetClassImage(players[i].Class, playerImages[i]);
			playerTexts[i].text = players[i].Class;

			if (players[i].Set)
			{
				foreach (var h in playerHorizontals[i])
				{
					h.enabled = false;
					h.GetComponentInChildren<CanvasRenderer>().SetAlpha(0);
				}

			}
			else
			{
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
		else if (pClass.Equals("The Pirate"))
		{
			image.sprite = pirateImage;
		}
	}


	private void RemovePlayer(string control, SelectStages stage)
	{
		if (PlayerList.All(player => player.Control != control))
		{
			return;
		}
		Player playerToRemove = PlayerList.First(p => p.Control == control);

		if (stage == kStage)
		{
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

		}
		else if (stage == j1Stage)
		{
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

		}
		else if (stage == j2Stage)
		{
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

		}
		else if (stage == j3Stage)
		{
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

		}
		else if (stage == j4Stage)
		{
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

		}
		UpdateSelect(PlayerList);
	}

	private void UpdatePlayButton()
	{
		if (PlayerList.Count(o => o.Set) >= 2)
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
		public bool Set { get; set; }
	}

}
