﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadingScreen :MonoBehaviour
{
    public Text LoadingText;
    public Text DiaryText;
    public double LoadingMs = 1.500d;
    public string LevelName;

    private double _loadingTextCounter;
    private const string Loading = "Loading";
    private List<string> _diaries = new List<string>();
    private System.Random _rand = new System.Random();

    void LoadTips(string tips)
    {
        using (var reader = XmlReader.Create(new StringReader(tips)))
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name.Equals("tip"))
                    {
                        if (reader.Read())
                        {
                            _diaries.Add(reader.Value.Trim());
                        }
                    }
                }
            }
        }

        var count = _rand.Next(0, _diaries.Count);
        DiaryText.text = _diaries[count];
    }

    void Start()
    {
        GameObject speaker = GameObject.Find("Speaker");
        if (speaker != null) speaker.GetComponent<AudioSource>().Stop();
        var tips = Resources.Load("tips") as TextAsset;

        LoadTips(tips.text);
    }

    void Update()
    {
        AnimateLoadingText();
        if (Application.GetStreamProgressForLevel(LevelName) >= 1)
        {
            LoadingText.text = "Loaded";
            SceneManager.LoadScene(LevelName);
        }

    }

    private void AnimateLoadingText()
    {
        _loadingTextCounter += 1 * Time.deltaTime;
        if (_loadingTextCounter >= LoadingMs)
        {
            AddDot();
            _loadingTextCounter = 0.000d;
        }

    }

    private void AddDot()
    {
        if (LoadingText.text.Count(c => c == '.') == 3)
        {
            LoadingText.text = Loading;
        }
        else
        {
            LoadingText.text += ".";
        }

    }
}
