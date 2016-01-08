﻿using UnityEngine;
using System.Collections;

public class localSceneSelect : sceneSelect
{
    protected override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("kCancel") || Input.GetButtonDown("j1Cancel") || Input.GetButtonDown("j2Cancel") ||
            Input.GetButtonDown("j3Cancel") || Input.GetButtonDown("j4Cancel"))
        {
            Application.LoadLevel("LocalPlayerSelect");
        }
    }
}