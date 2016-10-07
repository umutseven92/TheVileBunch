using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class settingsScript : MonoBehaviour
{
    public Toggle MusicToggle;

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

        Loaded = true;

    }
}
