using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class loadOnClick : MonoBehaviour
{
    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public void LoadScene(string levelName)
    {
        Application.LoadLevel(levelName);
    }

    public void ResetPlayers()
    {
        playerSelect.PlayerList = new List<playerSelect.Player>();
    }

    public void QuitGame()
    {
        Application.Quit();
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
        Application.LoadLevel(1);
    }
    public void FocusOn(Transform item)
    {
        EventSystem.current.SetSelectedGameObject(item.gameObject);
    }

}
