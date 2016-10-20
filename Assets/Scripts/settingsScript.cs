using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class settingsScript : MonoBehaviour
{
	public Toggle MusicToggle;
	public Toggle FullScreenToggle;
	public Dropdown ResolutionDropdown;

	[HideInInspector]
	public bool Loaded;

	private string PlayerName;

	// Use this for initialization
	void Start()
	{
		// Music 
		if (PlayerPrefs.GetInt(global.Music) == 1)
		{
			MusicToggle.GetComponent<Toggle>().isOn = true;
		}
		else
		{
			MusicToggle.GetComponent<Toggle>().isOn = false;
		}

		// FullScreen
		if (PlayerPrefs.GetInt(global.FullScreen) == 1)
		{
			FullScreenToggle.GetComponent<Toggle>().isOn = true;
		}
		else
		{
			FullScreenToggle.GetComponent<Toggle>().isOn = false;
		}

		// Resolution
		var res = PlayerPrefs.GetInt(global.Resolution);

		ResolutionDropdown.value = res;

		Loaded = true;

	}
}
