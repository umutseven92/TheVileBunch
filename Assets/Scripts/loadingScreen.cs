using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine.UI;

public class loadingScreen : MonoBehaviour
{
    public Text LoadingText;
    public Text DiaryText;
    public double LoadingMs = 1.500d;
    public int LevelIndex;
    public Transform AButton;

    private double _loadingTextCounter;
    private const string Loading = "Loading";
    private List<string> _diaries = new List<string>();
    private System.Random _rand = new System.Random();

    // Use this for initialization
    void Start()
    {
        GameObject speaker = GameObject.Find("Speaker");
        if (speaker != null) speaker.GetComponent<AudioSource>().Stop();

        var diaries = Resources.Load("diaries") as TextAsset;

        using (var reader = XmlReader.Create(new StringReader(diaries.text)))
        {
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name.Equals("entry"))
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

        AButton.GetComponent<SpriteRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateLoadingText();
        if (Application.GetStreamProgressForLevel(LevelIndex) == 1)
        {
            LoadingText.text = "Loaded";
            AButton.GetComponent<SpriteRenderer>().enabled = true;

            if (Input.GetButtonDown("Submit") )
            {
                Application.LoadLevel(LevelIndex);
            }
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
