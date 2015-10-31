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
    private bool delay = true;

    public Text txtLevel;
    public Image imgLevel;

    private double delayCounter = 0.000d;
    private double delayMs = 0.2d;

    void Start()
    {
        _dunesImage = Resources.Load<Sprite>("dunes");
        _cavesImage = Resources.Load<Sprite>("caves");

        SetLevelText(_levels[0]);
    }


    void Update()
    {
        CheckDelayTimer();
        if (delay)
        {
            if (Input.GetAxis("Horizontal") > 0.5)
            {
                SetLevel(1);
            }
            else if (Input.GetAxis("Horizontal") < -0.5)
            {
                SetLevel(-1);
            }

            delay = false;
        }

    }

    private void CheckDelayTimer()
    {
        if (!delay)
        {
            delayCounter += Time.deltaTime;
            if (delayCounter > delayMs)
            {
                delayCounter = 0.000d;
                delay = true;
            }
        }
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
                Application.LoadLevel("DunesLoading");
                break;
            case "Caves":
                Application.LoadLevel("CavesLoading");
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
