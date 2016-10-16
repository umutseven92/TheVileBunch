using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadOnClick : MonoBehaviour
{
    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void Disconnect()
    {
        //PhotonNetwork.Disconnect();
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

    public void EndMultiGame()
    {
        playerSelect.PlayerList = new List<playerSelect.Player>();
        //PhotonNetwork.automaticallySyncScene = false;
        Time.timeScale = 1;
        //PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu");
    }

    public void QuickJoin()
    {
        onlineHelper.Joining = true;
        //PhotonNetwork.LoadLevel("OnlineLoading");
    }

    public void FocusOn(Transform item)
    {
        EventSystem.current.SetSelectedGameObject(item.gameObject);
    }
}
