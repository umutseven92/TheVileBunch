using UnityEngine;
using System.Collections.Generic;

public class localSceneSelect : sceneSelect
{
    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("kCancel") || Input.GetButtonDown("j1Cancel") || Input.GetButtonDown("j2Cancel") ||
            Input.GetButtonDown("j3Cancel") || Input.GetButtonDown("j4Cancel"))
        {
            playerSelect.PlayerList = new List<playerSelect.Player>();
            Time.timeScale = 1;
            Application.LoadLevel("LocalPlayerSelect");
        }
    }
}