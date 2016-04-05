using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class graveyardOnlineScript : graveyardScript {

	protected override void Start()
	{
		base.Start();
		WinnerText.text = WinnerText.text.Replace("{class}", localSceneHelper.Winner);
		var remaining = playerSelect.PlayerList;

		remaining.Remove(remaining.Find(p=>p.Class.Equals(localSceneHelper.Winner)));

		switch (playerSelect.PlayerList.Count -1)
		{
			case 3:
				var graveFirstThree = Instantiate(Graveyard, One.gameObject.transform.position, Quaternion.identity) as Transform;
				var graveSecondThree = Instantiate(Graveyard, One.gameObject.transform.position, Quaternion.identity) as Transform;
				var graveThirdThree = Instantiate(Graveyard, One.gameObject.transform.position, Quaternion.identity) as Transform;

				graveFirstThree.GetComponent<Canvas>().GetComponents<Text>().First(d => d.gameObject.name.Equals("className")).text = remaining[0].Class;
				graveFirstThree.GetComponent<Canvas>().GetComponents<Text>().First(d => d.gameObject.name.Equals("playerName")).text = remaining[0].OnlinePlayerName;

				graveSecondThree.GetComponent<Canvas>().GetComponents<Text>().First(d => d.gameObject.name.Equals("className")).text = remaining[1].Class;
				graveSecondThree.GetComponent<Canvas>().GetComponents<Text>().First(d => d.gameObject.name.Equals("playerName")).text = remaining[1].OnlinePlayerName;

				graveThirdThree.GetComponent<Canvas>().GetComponents<Text>().First(d => d.gameObject.name.Equals("className")).text = remaining[2].Class;
				graveThirdThree.GetComponent<Canvas>().GetComponents<Text>().First(d => d.gameObject.name.Equals("playerName")).text = remaining[2].OnlinePlayerName;
				break;
			case 2:
				var graveFirstTwo = Instantiate(Graveyard, One.gameObject.transform.position, Quaternion.identity) as Transform;
				var graveSecondTwo = Instantiate(Graveyard, One.gameObject.transform.position, Quaternion.identity) as Transform;

				graveFirstTwo.GetComponent<Canvas>().GetComponents<Text>().First(d => d.gameObject.name.Equals("className")).text = remaining[0].Class;
				graveFirstTwo.GetComponent<Canvas>().GetComponents<Text>().First(d => d.gameObject.name.Equals("playerName")).text = remaining[0].OnlinePlayerName;

				graveSecondTwo.GetComponent<Canvas>().GetComponents<Text>().First(d => d.gameObject.name.Equals("className")).text = remaining[1].Class;
				graveSecondTwo.GetComponent<Canvas>().GetComponents<Text>().First(d => d.gameObject.name.Equals("playerName")).text = remaining[1].OnlinePlayerName;
				break;
			case 1:
				var graveFirstOne = Instantiate(Graveyard, One.gameObject.transform.position, Quaternion.identity) as Transform;

				graveFirstOne.GetComponent<Canvas>().GetComponents<Text>().First(d => d.gameObject.name.Equals("className")).text = remaining[0].Class;
				graveFirstOne.GetComponent<Canvas>().GetComponents<Text>().First(d => d.gameObject.name.Equals("playerName")).text = remaining[0].OnlinePlayerName;
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
		PhotonNetwork.LoadLevel("OnlinePlayerSelect");
	}

}
