using System;
using UnityEngine;
using System.Collections;

public class screenShotScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyUp(KeyCode.S))
		{
			var guid = new Guid().ToString();
			TakeScreenshot(guid);
		}
	}

	private void TakeScreenshot(string filename)
	{
		Application.CaptureScreenshot(filename + ".png");
	}
}
