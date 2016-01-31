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
        Version.text = "v" + global.GameVersion;

        Time.timeScale = 1.0f;

        Log.Info("Initializing game..");

        //Application.targetFrameRate = global.FrameRateLimit;
        CheckPlayerPrefs();
        SetMusic();
        Log.Debug("MENU");
    }

    private void CheckPlayerPrefs()
    {
        Log.Info("Checking player preferences..");

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

        Log.InfoFormat("Player ID for this session is {0}, machine name {1}", id, Environment.MachineName);
    }

    private void SetMusic()
    {
        var audioSource = GameObject.Find("Speaker").GetComponent<AudioSource>();
        audioSource.mute = PlayerPrefs.GetInt(global.Music) != 1;
    }

}
