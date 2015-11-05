using UnityEngine;
using System.Collections;

public class menuScript : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		if (!PlayerPrefs.HasKey("Music"))
		{
			PlayerPrefs.SetInt("Music", 1);
		}

		SetPrefs();
	}

	private void SetPrefs()
	{
		var audioSource = GameObject.Find("Speaker").GetComponent<AudioSource>();

		audioSource.mute = PlayerPrefs.GetInt("Music") != 1;
	}
}
