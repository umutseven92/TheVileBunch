using System;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class menuScript : MonoBehaviour
{
	public Text Version;

	private readonly Random _rand = new Random();
	private readonly string[] _names = { "Chuck", "Cedric", "Iggy", "Woodrow", "Fergie", "Martin", "Volpe", "Tommy", "Steve", "Townie", "Deuce", "Townie", "Marvin", "Nazo", "Clive", "Ula", "Hobo", "Punky", "Yuri", "Gus", "Stan", "Bill", "Salim", "Ed", "Rob", "Paul", "Ernosto", "Ramon" };

	private readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

	// Use this for initialization
	void Start()
	{
		global.LogInfo(Log, "Initializing game..");

		CheckPlayerPrefs();

		Version.text = "v" + global.GameVersion;
		Time.timeScale = 1.0f;

		SetMusic();
	}

	private void CheckPlayerPrefs()
	{
		global.LogInfo(Log, "Checking player preferences..");

		if (!PlayerPrefs.HasKey(global.FullScreen))
		{
			PlayerPrefs.SetInt(global.FullScreen, 1);
		}
		else
		{
			Screen.fullScreen = PlayerPrefs.GetInt(global.FullScreen) == 1;
		}

		if (!PlayerPrefs.HasKey(global.Resolution))
		{
			PlayerPrefs.SetInt(global.Resolution, (int)(global.Resolutions.r1920x1080));
		}
		else
		{
			var res = (global.Resolutions)PlayerPrefs.GetInt(global.Resolution);

			var resArray = res.ToString().Substring(1, res.ToString().Length - 1).Split('x');

			Screen.SetResolution(int.Parse(resArray[0]), int.Parse(resArray[1]), Screen.fullScreen);
		}

		if (!PlayerPrefs.HasKey(global.Music))
		{
			PlayerPrefs.SetInt(global.Music, 1);
		}

		if (!PlayerPrefs.HasKey(global.SoundEffects))
		{
			PlayerPrefs.SetInt(global.SoundEffects, 1);
		}

		if (!PlayerPrefs.HasKey(global.PlayerName))
		{
			PlayerPrefs.SetString(global.PlayerName, _names[_rand.Next(_names.Length)]);
		}

		var id = Guid.NewGuid();

		// Player Id is volatile
		PlayerPrefs.SetString(global.PlayerId, id.ToString());

		global.LogInfo(Log, string.Format("Player ID for this session is {0}, machine name {1}", id, Environment.MachineName));
	}

	private void SetMusic()
	{
		var audioSource = GameObject.Find("Speaker").GetComponent<AudioSource>();
		audioSource.mute = PlayerPrefs.GetInt(global.Music) != 1;
	}

}
