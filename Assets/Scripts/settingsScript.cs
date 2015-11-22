using UnityEngine;
using UnityEngine.UI;

public class settingsScript : MonoBehaviour
{
    public Toggle MusicToggle;
    public InputField NameInput;

    [HideInInspector]
    public bool Loaded;

    private string PlayerName;

    // Use this for initialization
    void Start()
    {
        if (PlayerPrefs.GetInt(global.Music) == 1)
        {
            MusicToggle.GetComponent<Toggle>().isOn = true;
        }
        else
        {
            MusicToggle.GetComponent<Toggle>().isOn = false;
        }

        NameInput.text = PlayerPrefs.GetString(global.PlayerName);

        Loaded = true;
    }

    public void SetPlayerName(string playerName)
    {
        PlayerPrefs.SetString(global.PlayerName, playerName);
    }
}
