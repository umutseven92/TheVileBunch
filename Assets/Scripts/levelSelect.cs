using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class levelSelect : MonoBehaviour
{
    private readonly string[] _levels = new[] { "Dunes", "Caves" };
    private int _pos = 0;
    private Sprite _dunesImage;
    private Sprite _cavesImage;

    public Text txtLevel;
    public Image imgLevel;


    void Start()
    {
        _dunesImage = Resources.Load<Sprite>("dunes");
        _cavesImage = Resources.Load<Sprite>("caves");

        SetLevelText(_levels[0]);
    }

    public void LoadLevel()
    {

        foreach (var p in playerSelect.PlayerList)
        {
            if (!p.Set)
            {
                playerSelect.PlayerList.Remove(p);
            }
        }

        switch (txtLevel.text)
        {
            case "Dunes":
                Application.LoadLevel("Dunes");
                break;
            case "Caves":
                Application.LoadLevel("Caves");
                break;
            default:
                Debug.LogError(txtLevel.text + " not found!");
                break;
        }
    }

    public void SetLevel(int dir)
    {
        if (dir == 1)
        {
            if (_pos == _levels.Length - 1)
            {
                _pos = -1;
            }
            _pos++;
            SetLevelText(_levels[_pos]);
        }
        else if (dir == -1)
        {
            if (_pos == 0)
            {
                _pos = _levels.Length;
            }
            _pos--;
            SetLevelText(_levels[_pos]);
        }

    }

    private void SetLevelText(string text)
    {
        txtLevel.text = text;

        if (text.Equals("Caves"))
        {
            imgLevel.sprite = _cavesImage;
        }
        else if (text.Equals("Dunes"))
        {
            imgLevel.sprite = _dunesImage;
        }
    }
}
