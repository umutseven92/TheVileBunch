using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class menuScript : MonoBehaviour
{
    public Text Version;

    // Use this for initialization
    void Start()
    {
        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetInt("Music", 1);
        }

        SetPrefs();

        Version.text = "v" + global.GameVersion;
    }

    private void SetPrefs()
    {
        var audioSource = GameObject.Find("Speaker").GetComponent<AudioSource>();

        audioSource.mute = PlayerPrefs.GetInt("Music") != 1;
    }
}
