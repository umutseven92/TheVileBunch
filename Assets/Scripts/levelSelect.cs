using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class levelSelect : MonoBehaviour
{
    private string[] levels = new[] {"Dunes", "Caves"};
    private int pos = 0;

    public Text txtLevel;

    void Start()
    {
        SetLevelText(levels[0]);
    }

    public void LoadLevel()
    {
        switch (txtLevel.text)
        {
            case "Dunes":
                Application.LoadLevel(2);
                break;
            case "Caves":
                Application.LoadLevel(3);
                break;
            default:
                Debug.LogError(txtLevel.text +  " not found!");
                break;
        }
    }

    public void SetLevel(int dir)
    {
        if (dir == 1)
        {
            if (pos == levels.Length-1)
            {
                pos = -1;
            }
            pos++;
            SetLevelText(levels[pos]);
        }
        else if (dir == -1)
        {
            if (pos == 0)
            {
                pos = levels.Length;
            }
            pos--;
            SetLevelText(levels[pos]);
        }

    }

    private void SetLevelText(string text)
    {
        txtLevel.text = text;
    }
}
