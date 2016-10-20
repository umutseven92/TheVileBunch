using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadOnClick : MonoBehaviour
{
	public Dropdown ResolutionDropdown;
	public Toggle FullScreenToggle;

	public void LoadScene(int level)
	{
		SceneManager.LoadScene(level);
	}

	public void LoadScene(string levelName)
	{
		SceneManager.LoadScene(levelName);
	}

	public void ResetPlayers()
	{
		playerSelect.PlayerList = new List<playerSelect.Player>();
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void SetResolution()
	{
		var scr = GameObject.Find("SettingsScripts");

		if (!scr.GetComponent<settingsScript>().Loaded)
		{
			return;
		}

		PlayerPrefs.SetInt(global.Resolution, ResolutionDropdown.value);


		var res = ResolutionDropdown.value;

		var resArray = res.ToString().Substring(1, res.ToString().Length - 1).Split('x');

		Screen.SetResolution(int.Parse(resArray[0]), int.Parse(resArray[1]), Screen.fullScreen);
	}

	public void ToggleFullScreen()
	{
		var scr = GameObject.Find("SettingsScripts");

		if (!scr.GetComponent<settingsScript>().Loaded)
		{
			return;
		}

		if (FullScreenToggle.isOn)
		{
			PlayerPrefs.SetInt(global.FullScreen, 1);
		}
		else
		{
			PlayerPrefs.SetInt(global.FullScreen, 0);
		}

		Screen.fullScreen = !Screen.fullScreen;
	}


	public void MuteGame()
	{
		var toggle = GameObject.Find("MusicToggle");
		var scr = GameObject.Find("SettingsScripts");

		if (!scr.GetComponent<settingsScript>().Loaded)
		{
			return;
		}
		var speaker = GameObject.Find("Speaker");
		var audio = speaker.GetComponent<AudioSource>();

		int val;

		if (toggle.GetComponent<Toggle>().isOn)
		{
			val = 1;
			audio.mute = false;
		}
		else
		{
			val = 0;
			audio.mute = true;
		}

		PlayerPrefs.SetInt(global.Music, val);
	}

	public void EndLocalGame()
	{
		playerSelect.PlayerList = new List<playerSelect.Player>();
		Time.timeScale = 1;
		SceneManager.LoadScene("LocalPlayerSelect");
	}


	public void FocusOn(Transform item)
	{
		EventSystem.current.SetSelectedGameObject(item.gameObject);
	}
}
