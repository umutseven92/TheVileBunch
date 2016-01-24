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


    // Use this for initialization
    void Start()
    {
        Version.text = "v" + global.GameVersion;

        Time.timeScale = 1.0f;

        //Application.targetFrameRate = global.FrameRateLimit;
        CheckPlayerPrefs();
        SetMusic();
    }

    private void CheckPlayerPrefs()
    {
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

        // Player Id is volatile
        PlayerPrefs.SetString(global.PlayerId, Guid.NewGuid().ToString());
    }

    private void SetMusic()
    {
        var audioSource = GameObject.Find("Speaker").GetComponent<AudioSource>();
        audioSource.mute = PlayerPrefs.GetInt(global.Music) != 1;
    }

}
