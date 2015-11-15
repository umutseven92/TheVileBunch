using UnityEngine;
using UnityEngine.UI;

public class settingsScript : MonoBehaviour
{
	public Toggle MusicToggle;

	private AudioSource audio;

	[HideInInspector]
	public bool Loaded;

	// Use this for initialization
	void Start () {
		var speaker = GameObject.Find("Speaker");

		audio = speaker.GetComponent<AudioSource>();

		if (PlayerPrefs.GetInt("Music") == 1)
		{
			MusicToggle.GetComponent<Toggle>().isOn = true;
		}
		else
		{
			MusicToggle.GetComponent<Toggle>().isOn = false;
		}

	    Loaded = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
