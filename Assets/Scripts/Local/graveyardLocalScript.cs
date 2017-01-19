using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class graveyardLocalScript : graveyardScript {

	protected override void Start()
	{
		base.Start();
		WinnerText.text = WinnerText.text.Replace("{class}", localSceneHelper.Winner);
		WinnerTextShadow.text = WinnerTextShadow.text.Replace("{class}", localSceneHelper.Winner);

		var remaining = playerSelect.PlayerList;

		remaining.Remove(remaining.Find(p=>p.Class.Equals(localSceneHelper.Winner)));

		switch (remaining.Count)
		{
			case 3:
				Three[0].text= remaining[0].Class;
				ThreeGrave[0].GetComponent<SpriteRenderer>().enabled = true;
				ThreeCanvas[0].GetComponent<Canvas>().enabled = true;

				Three[1].text= remaining[1].Class;
				ThreeGrave[1].GetComponent<SpriteRenderer>().enabled = true;
				ThreeCanvas[1].GetComponent<Canvas>().enabled = true;

				Three[2].text= remaining[2].Class;
				ThreeGrave[2].GetComponent<SpriteRenderer>().enabled = true;
				ThreeCanvas[2].GetComponent<Canvas>().enabled = true;
				break;
			case 2:
				Two[0].text= remaining[0].Class;
				TwoGrave[0].GetComponent<SpriteRenderer>().enabled = true;
				TwoCanvas[0].GetComponent<Canvas>().enabled = true;

				Two[1].text= remaining[1].Class;
				TwoGrave[1].GetComponent<SpriteRenderer>().enabled = true;
				TwoCanvas[1].GetComponent<Canvas>().enabled = true;
				break;
			case 1:
				One.text= remaining[0].Class;
				OneGrave.GetComponent<SpriteRenderer>().enabled = true;
				OneCanvas.GetComponent<Canvas>().enabled = true;
				break;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (done)
		{
			GetBackToPlayerSelect();
		}
	}

	private static void GetBackToPlayerSelect()
	{
		playerSelect.PlayerList = new List<playerSelect.Player>();
		SceneManager.LoadScene("LocalPlayerSelect");
	}
}
