using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sceneSelect : MonoBehaviour
{
    public Button PlayButton;

    // Use this for initialization
    void Start()
    {
        PlayButton.Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("kCancel") || Input.GetButtonDown("j1Cancel") || Input.GetButtonDown("j2Cancel") || Input.GetButtonDown("j3Cancel") || Input.GetButtonDown("j4Cancel"))
        {
            Application.LoadLevel("LocalPlayerSelect");
        }
    }
}
